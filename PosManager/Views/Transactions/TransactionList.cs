using PosLibrary.Controller.Customers;
using PosLibrary.Controller.Fiscal;
using PosLibrary.Controller.StoreSetting;
using PosLibrary.Controller.Transactions;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.StoreSetting;
using PosLibrary.Model.Entities.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosManager.Views.Transactions
{
    public partial class TransactionList : UserControl
    {


        TransactionHeaderController _dataController = new TransactionHeaderController();
        TransactionLinesController _lineController = new TransactionLinesController();
        TransactionPaymentsController _paymentController = new TransactionPaymentsController();
        TransactionHeader _data = null;
        List<TransactionHeader> lines = new List<TransactionHeader>();
        Store store = new Store();
        public TransactionList()
        {
            InitializeComponent();
            store = new StoreController().Get(1).response as Store;
            LoadData();
        }

        public TransactionList(TransactionHeader data)
        {
            store = new StoreController().Get(1).response as Store;
            _data = data;
            GenerateSale();
        }

        private void LoadData(string filter = "") 
        {
            try
            {
                var list = string.IsNullOrEmpty(filter) ?
                                        _dataController.GetList() :
                                        _dataController.GetList(filter);

                if (list.result)
                {
                    lines = list.response as List<TransactionHeader>;
                    if (lines.Count > 0)
                        dtData.DataSource = lines.Select(a => new
                        {
                            Recibo = a.ReceiptId,
                            Cliente = string.Format("{0} {1}", a.Customer.FirstName, a.Customer.LastName),
                            Monto = a.TotalPayment,
                            Fecha = a.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss")
                        }).ToList();
                }
            }
            catch { }
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var line = dtData.Rows[e.RowIndex].DataBoundItem;
                if (line != null)
                {
                    string receipt = (string)line.GetType().GetProperty("Recibo").GetValue(line, null);

                    _data = lines.Where(a => a.ReceiptId == receipt).FirstOrDefault();

                    LoadItems();
                    LoadPayments();
                    CustomerSelected();
                }
            }
            catch { }
        }

        private void LoadItems()
        {
            _data.TransactionLines = _lineController.GetList(_data.ReceiptId).response as List<TransactionLines>;
            dtLine.DataSource = _data.TransactionLines.Select(a => new {
                Sku = a.Item.Sku,
                Cantidad = a.Quantity.ToString("###,###,##0.00"),
                Total = a.TotalAmount.ToString("###,###,##0.00")
            }).ToList();

        }

        private void LoadPayments()
        {

            _data.TransactionPayments = _paymentController.GetList(_data.ReceiptId).response as List<TransactionPayments>;
            dtPayment.DataSource = _data.TransactionPayments.Select(a => new {
                Descripcion = a.PaymentMethod.Name,
                Pago = a.TotalAmount.ToString("###,###,##0.00")
            }).ToList();

        }

        private void CustomerSelected()
        {
            txtCustomer.Text = string.Format("Codigo: {0}\r\nNombre: {1} {2}\r\n",
                                            _data.Customer.CustomerId, _data.Customer.FirstName,
                                            _data.Customer.LastName);

            txtCustomer.Text += !string.IsNullOrEmpty(_data.Customer.VatNumber) ? string.Format("Cedula/RNC: {0}\r\n",
                                            _data.Customer.VatNumber) : "";

            txtCustomer.Text += !string.IsNullOrEmpty(_data.Customer.CompanyName) ? string.Format("Empresa: {0}\r\n",
                                           _data.Customer.CompanyName) : "";
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GenerateSale();
        }
        private void GenerateSale()
        {
            string htmlData = string.Empty;
            string path = Directory.GetCurrentDirectory() + "\\pos_logo.jpg";
            NcfHistory ncf = (new NcfHistoryController().GetList(_data.ReceiptId).response as List<NcfHistory>).FirstOrDefault();
            try
            {
                #region Report
                htmlData += @"<!DOCTYPE html>
                            <html lang='en'>
                               <head>
                                  <meta charset='utf-8'>
                                  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                  <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
                                  <meta name='description' content=''>
                                  <meta name='author' content=''>
                                  <link rel='icon' type='image/png' href='img/favicom-nj.png'>
                                  <title>" + store.CompanyName + @"</title>
                                  <link href='https://fonts.googleapis.com/css2?family=Lexend:wght@100;400;500&display=swap' rel='stylesheet'>
                                  
                               </head>
                               <style type='text/css'>
                                  table tbody tr td {
                                  border-right: 1px #e4e4e4 solid;
                                  padding: 6px;
                                  border-bottom: 1px #e4e4e4 solid;
                                  padding: 6px;
                                  }
                                  table tr{
                                  height: 23px;
                                  }
                                  table thead th{
                                  background: #dcdcdc;
                                  border-bottom: 1px #e4e4e4 solid;
                                  border-right: 1px #e4e4e4 solid;
                                  }
                                  table tfoot {
                                  background: #dcdcdc;
                                  font-size: 14px;
                                  color: #000;
                                  padding: 10px 0px;
                                  font-weight: 900;
                                  }
                                  table tfoot td{
                                  border-right: 1px #e4e4e4 solid;
                                  border-bottom: 1px #e4e4e4 solid;
                                  }
                                  #footer {
                                  position: fixed;
                                  padding: 10px 10px 0px 10px;
                                  bottom: 0;
                                  width: 100%;
                                  /* Height of the footer*/ 
                                  }
                                  body {
                                  -webkit-print-color-adjust: exact !important;
                                  }
                               </style>
                               <body style='background: linear-gradient(to bottom, #d6d6d6, #dedcdc);
                                     font-family: Lexend, sans-serif;'>
                               <div class='contenedor-invoice' style='margin-top: 3%;background: #fff;width: 90%; 
                                    padding: 10px 27px 10px 27px; margin: 0 auto;border-top: 5px #c01419 solid;
                                    border-radius: 5px;'>
                               <div class='logo-invoice'>
                               </div>
                               <div class='logo-invoice' style='display: inline-flex;width: 100%;'>
                                  <div style='width: 33%;'>
                                     <br/><br/>
                                     <p style='text-align: left'><b>RNC: </b> " + SetFormatVatNumber(store.VatNumber) + @" <br/> <b>Tel.:</b> " + Convert.ToInt64(store.Phone).ToString("###-###-####") + @"<br>
                                  </div>
                                  <div style='width: 33%;'>
                                     <img src='"+ path + @"' asp-append-version='true' style='width: 125px;margin: 0 auto;display: block;margin-bottom: 0px;    margin-top: 10px;'>
                                  </div>
                                  <div style='width: 33%;'>
                                     <br/><br/>
                                     <p style='text-align: right'><b>" + _data.CreatedDate.ToString("dd/MM/yyyy") + @", <br/>" + _data.CreatedDate.ToString("hh:mm:ss tt") + @" <br/> </b> <br></p>
                                  </div>
                               </div>
                               <div class='detail-invoice' style='display: inline-flex;width: 100%;'>
                                  <div style='width: 80%;'>
                                     <p> <b>Cliente /  Razón Social:</b> ";
                string name = (string.IsNullOrEmpty(_data.Customer.CompanyName)) ?
                               string.Format("{0} {1}", _data.Customer.FirstName,
                                                    _data.Customer.LastName) :
                               _data.Customer.CompanyName;
                htmlData += name + @"<br/>
                                        <b>RNC / Cédula:</b>  " + SetFormatVatNumber(_data.Customer.VatNumber) + @"<br/>
                                        <b>No. Documento:</b>  " + _data.ReceiptId;

                htmlData += @"<br/><b>Ncf:</b>  " + ncf.NcfNumber;
                if (!string.IsNullOrEmpty(ncf.ReturnNcfNumber))
                    htmlData += @"<br/><b>Ncf Afectado:</b>  " + ncf.ReturnNcfNumber;

                htmlData += @" </p>
                                  </div>
                                  <div style='width: 20%;'>
                                  </div>
                               </div>
                               <br/>
                               <br/>
                               <div class='tabla-invoice'>
                                  <table style='width: 100%;border: 1px solid #dedbdb;'>
                                     <thead>
                                        <tr style='padding: 5px; background: #dcdcdc;font-size: 15px;'>
                                            <th style='background: #dcdcdc;font-weight: 800;'>Sku</th>
                                            <th style='background: #dcdcdc;font-weight: 800;'>Descripción</th>
                                           <th style='background: #dcdcdc;font-weight: 800;'>Cant.</th>
                                           <th style='background: #dcdcdc;font-weight: 800;'>Precio Und</th>
                                           <th style='background: #dcdcdc;font-weight: 800;'>Total Imp.</th>
                                           <th style='background: #dcdcdc;font-weight: 800;'>% Desc.</th>
                                           <th style='background: #dcdcdc;font-weight: 800;'>Total</th>
                                        </tr>
                                     </thead>
                                     <tbody>";
                foreach (var item in _data.TransactionLines)
                {
                    htmlData += @"<tr>
                                             <td style='text-align: left;'> " + item.Item.Sku + @"</td>
                                             <td style='text-align: left;'> " + item.Description + @"</td>
                                             <td style='text-align: center;'> " + item.Quantity.ToString("###,###,##0.00") + @"</td>
                                             <td style='text-align: right;'> " + item.Price.ToString("###,###,##0.00") + @"</td>
                                             <td style='text-align: right;'> " + item.TaxAmount.ToString("###,###,##0.00") + @"</td>
                                             <td style='text-align: right;'> " + item.DiscountPercent.ToString("###,###,##0.00") + @"</td>
                                             <td style='text-align: right;'> " + item.TotalAmount.ToString("###,###,##0.00") + @"</td>
                                        </tr>";
                }

                htmlData += @"
                                     </tbody>
                                     <tfoot>
                                        <tr>
                                           <td colspan='5' style='background: #fff; border: 1px solid white;' ></td>
                                           <td style='padding: 2px 10px; border: 1px solid #dedbdb;' >Subtotal</td>
                                           <td style='border: 1px solid #dedbdb; text-align: right; border-left: 0px'> $" + _data.TotalAmount.ToString("###,###,##0.00") + @" </td>
                                        </tr>
                                        <tr>
                                           <td colspan='5' style='background: #fff; border: 1px solid white;' ></td>
                                           <td style='padding: 2px 10px; border: 1px solid #dedbdb;'>ITBIS</td>
                                           <td style='border: 1px solid #dedbdb; text-align: right; border-left: 0px'> $" + _data.TaxAmount.ToString("###,###,##0.00") + @" </td>
                                        </tr>
                                        <tr>
                                           <td colspan='5' style='background: #fff; border: 1px solid white;' ></td>
                                           <td style='padding: 2px 10px; border: 1px solid #dedbdb;'>Total</td>
                                           <td style='border: 1px solid #dedbdb; text-align: right; border-left: 0px'> $" + _data.TotalPayment.ToString("###,###,##0.00") + @" </td>
                                        </tr>
                                     </tfoot>
                                  </table>
                                <br/><br/>
                                 <table style='width: 60%;  margin-left:40%;border: 1px solid #dedbdb;'>
                                     <thead>
                                        <tr style='padding: 5px; background: #dcdcdc;font-size: 15px;'>
                                            <th style='background: #dcdcdc;font-weight: 800;'>Descripcion</th>
                                           <th style='background: #dcdcdc;font-weight: 800;'>Total</th>
                                        </tr>
                                     </thead>
                                     <tbody>";
                foreach (var item in _data.TransactionPayments)
                {
                    htmlData += @"<tr>
                                             <td style='text-align: left;'> " + item.PaymentMethod.Name + @"</td>
                                             <td style='text-align: right;'> " + item.TotalAmount.ToString("###,###,##0.00") + @"</td>
                                        </tr>";
                }
                if (Math.Abs(_data.TotalPayment) != Math.Abs(_data.TransactionPayments.Sum(a => a.TotalAmount)))
                {
                    htmlData += @"<tr>
                                             <td style='text-align: left;'> Cambio</td>
                                             <td style='text-align: right;'> " + (Math.Abs(_data.TotalPayment) - Math.Abs(_data.TransactionPayments.Sum(a => a.TotalAmount))).ToString("###,###,##0.00") + @"</td>
                                        </tr>";
                }

                htmlData += @"
                                     </tbody>
                                  </table>
                                    <br/>
                                  <div style='width: 100%;'>
                                     <p style='font-size: 13px;margin-top: 32px;'><b>Nota: </b>no se haran devoluciones de efectivo.</p>
                                  </div>
                               </div>

                               </body>
                            </html>";

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            GenerateHTMLFile(htmlData);
        }

        private void GenerateHTMLFile(string html)
        {
            try
            {
                string htmlResult = html;
                string path = Directory.GetCurrentDirectory();
                string urlPath = Path.Combine(path, "Report");
                try
                {
                    if (!Directory.Exists(urlPath))
                        Directory.CreateDirectory(urlPath);
                }
                catch { }
                try
                {
                    DeleteOldFile(urlPath);
                }
                catch { }

                urlPath += @"\" + _data.ReceiptId + ".html";
                try
                {

                    FileStream fs = File.Create(urlPath);
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(html);
                    }
                    Thread.Sleep(5000);
                }
                catch { }


                System.Diagnostics.Process.Start(urlPath);
            }
            catch
            {
            }
        }

        private void DeleteOldFile(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.Date.AddDays(-1))
                        fi.Delete();
                }
            }
            catch
            {
            }
        }

        private string NumberToText(decimal numberAsString)
        {
            string dec;

            var number = Convert.ToInt64(Math.Truncate(numberAsString));
            var decimals = Convert.ToInt32(Math.Round((numberAsString - number) * 100, 2));

            if (decimals > 0)
                dec = $" CON {decimals:0,0} /100";
            else
                dec = $" CON {decimals:0,0} /100";

            var res = NumberToText(Convert.ToDecimal(number)) + dec;
            return res;
        }

        private string SetFormatVatNumber(string VatNumber)
        {

            if (new String(VatNumber.Where(Char.IsLetter).ToArray()).Length == 0)
            {
                if (VatNumber.Length == 9)
                {
                    VatNumber = Convert.ToInt64(new String(VatNumber.Where(Char.IsDigit).ToArray())).ToString("###-#####-#");
                }
                else if (VatNumber.Length == 11)
                {
                    VatNumber = Convert.ToInt64(new String(VatNumber.Where(Char.IsDigit).ToArray())).ToString("###-#######-#");
                }
            }

            return VatNumber;
        }


    }
}
