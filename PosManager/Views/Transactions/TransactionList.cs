using PosLibrary.Controller.Customers;
using PosLibrary.Controller.Transactions;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public TransactionList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string filter = "") 
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
                        Cliente = string.Format("{0} {1}", a.Customer.FirstName,a.Customer.LastName),
                        Monto = a.TotalPayment,
                        Fecha = a.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss")
                    }).ToList();
            }
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var line = dtData.Rows[e.RowIndex].DataBoundItem;
                if (line != null)
                {
                    string receipt = (string)line.GetType().GetProperty("Recibo").GetValue(line, null);

                    _data= lines.Where(a => a.ReceiptId == receipt).FirstOrDefault();

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
    }
}
