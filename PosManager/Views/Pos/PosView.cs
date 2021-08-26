using PosLibrary.Controller.Fiscal;
using PosLibrary.Controller.Items;
using PosLibrary.Controller.StoreSetting;
using PosLibrary.Controller.Transactions;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Enums;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Transactions;
using PosManager.Controller;
using PosManager.Views.DashBoard;
using PosManager.Views.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class PosView : Form
    {
        private NcfType _ncfType = new NcfType();
        private NcfTypeController ncfTypeController = new NcfTypeController();
        private NcfSequenceDetailController detailController = new NcfSequenceDetailController();
        private Customer _customer = new Customer(); 
        private Item _item = new Item();
        private List<TransactionLines> _transactionLines = new List<TransactionLines>();
        private List<TransactionPayments> _transactionPayments = new List<TransactionPayments>();
        private GenericController genericController = new GenericController();
        private ItemController itemContoller = new ItemController();
        private decimal[] amounts = new decimal[4];
        private int paymentRow = 0;
        private int lineRow = 0;

        private string NcfSequence = string.Empty;
        public PosView()
        {
            InitializeComponent();
            ClearTransactions();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Environment.Exit(1);
        }

        private void CustomerSelected() 
        {
            txtCustomer.Text = string.Format("Codigo: {0}\r\nNombre: {1} {2}\r\n", 
                                            _customer.CustomerId, _customer.FirstName,
                                            _customer.LastName);

            txtCustomer.Text += !string.IsNullOrEmpty(_customer.VatNumber) ? string.Format("Cedula/RNC: {0}\r\n",
                                            _customer.VatNumber) : "";

            txtCustomer.Text += !string.IsNullOrEmpty(_customer.CompanyName) ? string.Format("Empresa: {0}\r\n",
                                           _customer.CompanyName) : "";
        }

        private void NcfSelected()
        {
            txtFiscal.Text = string.Format("Tipo de NCF: {0}\r\n",
                                            _ncfType.NcfId.ToString("00"));

            txtFiscal.Text += !string.IsNullOrEmpty(_ncfType.Description) ? string.Format("{0}\r\n",
                                            _ncfType.Description) : "";

            txtFiscal.Text += !string.IsNullOrEmpty(NcfSequence) ? string.Format("Secuencia: {0}",
                                           NcfSequence) : string.Format("B{0}00000001", _ncfType.NcfId);
        }

        private decimal CalculateTax(decimal price, decimal tax) 
        {
            return Math.Abs(price - (price * ((tax / 100) + 1)));
        }

        private decimal CalculateDiscount(decimal amount, decimal disc)
        {
            return Math.Abs(amount - (amount * (disc / 100)));
        }

        private void ItemsSelected(decimal qty = 1) 
        {

            TransactionLines line = new TransactionLines()
            {
                Id = 0,
                Condition_Status = true,
                Deleted = false,
                Description = _item.Name,
                DiscountPercent = _item.ItemDiscount.AmountPercent,
                ItemId = _item.Id,
                Price = _item.Price,
                Quantity = qty,
                TaxAmount = (_ncfType.NcfId != 14) ? CalculateTax(_item.Price, _item.ItemTax.AmountPercent) : 0,
                TaxPercent = (_ncfType.NcfId != 14) ? _item.ItemTax.AmountPercent : 0,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Item = _item
                
            };
            //line.TotalAmount = (_ncfType.NcfId != 14) ? line.Price + line.TaxAmount : line.Price;

            //if (line.DiscountPercent != 0) 
            //    line.TotalAmount = CalculateDiscount(line.TotalAmount, line.DiscountPercent);

            _transactionLines.Add(line);

            CalculateLines();
        }

        private void  CalculateLines() 
        {
            amounts = new decimal[4];
            foreach (var line in _transactionLines)
            {

                amounts[0] += CalculateDiscount(line.Price, line.DiscountPercent) * line.Quantity;

                if (_ncfType.NcfId != 14)
                {
                    line.TaxAmount = CalculateTax(CalculateDiscount(line.Price, line.DiscountPercent),
                                                                    line.Item.ItemTax.AmountPercent)
                                                                    * line.Quantity;
                }
                else
                    line.TaxAmount = 0;

                amounts[1] += line.TaxAmount;


                line.TaxPercent = (_ncfType.NcfId != 14) ? line.Item.ItemTax.AmountPercent : 0;

                line.TotalAmount = (CalculateDiscount(line.Price, 
                                    line.DiscountPercent) 
                                    * line.Quantity) 
                                    + line.TaxAmount;

                if (line.DiscountPercent != 0)
                {
                    decimal totalAmount = CalculateTax(line.Price, line.TaxPercent) * line.Quantity;
                    amounts[2] += totalAmount - line.TotalAmount;
                }

                amounts[3] += line.TotalAmount;
            }
            LoadItems();
            LoadCalculatePanel();
        }
        private void LoadItems()
        {

            dtItems.DataSource = _transactionLines.Select(a => new {
                Sku = a.Item.Sku,
                Descripcion = a.Description,
                Cantidad = a.Quantity.ToString("###,###,##0.00"),
                Precio = a.Price.ToString("###,###,##0.00"),
                Impuestos = a.TaxAmount.ToString("###,###,##0.00"),
                Descuento = a.DiscountPercent.ToString("##0.00")+"%",
                Total = a.TotalAmount.ToString("###,###,##0.00")
            }).ToList();

        }

        private void LoadPayments()
        {

            dtPayments.DataSource = _transactionPayments.Select(a => new {
                Descripcion = a.PaymentMethod.Name,
                Pago = a.TotalAmount.ToString("###,###,##0.00")
            }).ToList();

        }

        private void btnNumber_Click(object sender, System.EventArgs e)
        {
            txtSku.Text += (sender as Button).Text;
            txtSku.Focus();
        }

        private void btnJournal_Click(object sender, System.EventArgs e)
        {
            this.Hide();
            using (TransactionView trans = new TransactionView())
            {
                trans.ShowDialog();
            }
            this.Show();
            txtSku.Focus();
        }

        private void txtSku_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string msg = "Sku no encontrado";
                var item = itemContoller.Get(txtSku.Text);

                if (item.result) 
                {
                    if (item.response != null)
                    {
                        _item = (Item)item.response;
                        ItemsSelected();
                    }
                    else
                        MessageBox.Show(msg);
                }
                else
                    MessageBox.Show(msg);
            }

            txtSku.Focus();
        }

        private void btnItem_Click(object sender, System.EventArgs e)
        {

            this.Hide();
            using (ItemList item = new ItemList())
            {
                item.ShowDialog();
                if (item.DialogResult == DialogResult.OK)
                {
                    _item = item.item;
                    ItemsSelected();

                }
            };
            this.Show();
        }

        private void btnFiscal_Click(object sender, System.EventArgs e)
        {
            using (NcfList ncf = new NcfList())
            {
                ncf.ShowDialog();
                if (ncf.DialogResult == DialogResult.OK) 
                {
                    _ncfType = ncf.ncfType;
                    NcfSelected();
                    CalculateLines();
                }
            };
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (CustomerList list = new CustomerList())
            {
                list.ShowDialog();
                if (list.DialogResult == DialogResult.OK)
                {
                    _customer = list.customer;
                    CustomerSelected();

                }
            };
            this.Show();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {

            string msg = "Debe seleccionar un cliente";
            if (_customer == null)
                MessageBox.Show(msg);

            else if (_customer.Id <= 0)
                MessageBox.Show(msg);

            else if (_transactionLines.Count > 0)
            {
                string response = detailController.ValidateNCFSequence(_ncfType.NcfId);

                if (!string.IsNullOrEmpty(response))
                {
                    MessageBox.Show(response);

                    var ncfInformation = new NcfSequenceDetailController().GetNcfStatus(_ncfType.NcfId, 0);

                    if (ncfInformation == null)
                        return;

                    if (ncfInformation.SeqStatus > 1)
                        return;
                }
                    
                using (PaymentList list = new PaymentList())
                {
                    list.ShowDialog();
                    if (list.DialogResult == DialogResult.OK)
                    {
                        var payment = list.payment;

                        using (PaymentAmount pay = new PaymentAmount(amounts[3] - _transactionPayments.Sum(a => a.TotalAmount),
                                                                     payment.OverTender,
                                                                     payment.UnderTender))
                        {
                            pay.ShowDialog();
                            if (pay.DialogResult == DialogResult.OK)
                            {
                                TransactionPayments payments = new TransactionPayments()
                                {
                                    Id = 0,
                                    Condition_Status = true,
                                    Deleted = false,
                                    PaymentMethodId = payment.Id,
                                    PaymentMethod = payment,
                                    TotalAmount = pay.amount,
                                    UpdatedDate = DateTime.Now,
                                    CreatedDate = DateTime.Now,
                                };
                                _transactionPayments.Add(payments);
                                LoadPayments();

                                decimal totalPayment = _transactionPayments.Sum(a => a.TotalAmount);
                                decimal amountChange = amounts[3] - totalPayment;
                                lblPending.Text = (amountChange).ToString("###,###,##0.00");

                                if ((Math.Abs(amounts[3]) - Math.Abs(totalPayment)) <= 0)
                                {

                                    ConcludeTransactions();

                                    if (amountChange < 0)
                                    {
                                        using (AmountChange change = new AmountChange(amountChange))
                                        {
                                            change.ShowDialog();
                                        };
                                    }
                                }
                            }
                        };

                    }
                };
            }
            else 
            {
                MessageBox.Show("Sin articulos seleccionados para facturar.");
            }
        }

        private void ConcludeTransactions() 
        {
            try
            {
                TransactionHeaderController transaction = new TransactionHeaderController();
                TransactionHeader header = new TransactionHeader()
                {
                    Id = 0,
                    ReceiptId = string.Empty,
                    Condition_Status = true,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CustomerId = _customer.Id,
                    Deleted = false,
                    TotalAmount = amounts[0],
                    TaxAmount = amounts[1],
                    DiscountAmount = amounts[2],
                    TotalPayment =amounts[3]  
                };

                var data = new StoreController().GenerateId(StoreTableType.Receipt);
                if (data.result)
                    header.ReceiptId = data.response.ToString();

                var transactionResult = transaction.Save(header);
                if (transactionResult.result)
                {
                    foreach (var line in _transactionLines)
                    {
                        line.ReceiptId = header.ReceiptId;
                        line.TransactionHeaderId = header.Id;
                    }
                    new TransactionLinesController().SaveList(_transactionLines);

                    foreach (var pay in _transactionPayments)
                    {
                        pay.ReceiptId = header.ReceiptId;
                        pay.TransactionHeaderId = header.Id;
                    }
                    new TransactionPaymentsController().SaveList(_transactionPayments);

                    var ncfInformation = new NcfSequenceDetailController().GetNcfStatus(_ncfType.NcfId, 1);
                    NcfHistory ncfHistory = new NcfHistory()
                    {
                        NcfNumber = ncfInformation.DGIIDescription,
                        ReceiptId = header.ReceiptId,
                        VatNumber = _customer.VatNumber,
                        Company = _customer.CompanyName,
                        Condition_Status = true,
                        CreatedDate = DateTime.Now,
                        Deleted = false,
                        Id = 0,
                        NcfTypeId = _ncfType.NcfId,
                        ReturnNcfNumber = string.Empty,
                        ReturnReceiptId = string.Empty,
                        NcfType = null,
                        TaxExempt = _ncfType.NcfId == 14 ? true : false,
                        TotalAmount = amounts[0],
                        TotalTax = amounts[1],
                        TotalAmountWithTax = amounts[3],
                        UpdatedDate = DateTime.Now
                    };

                    new NcfHistoryController().Save(ncfHistory);
                    header.Customer = _customer;
                    header.TransactionLines = new TransactionLinesController().GetList(header.ReceiptId).response as List<TransactionLines>;
                    header.TransactionPayments = new TransactionPaymentsController().GetList(header.ReceiptId).response as List<TransactionPayments>;

                    new TransactionList(header);
                }
                ClearTransactions();
            }
            catch 
            {
            
            }
        }

        private void ClearTransactions() 
        {
            _ncfType = new NcfType();
            _customer = new Customer();
            _item = new Item();
            _transactionLines = new List<TransactionLines>();
            _transactionPayments = new List<TransactionPayments>();
            amounts = new decimal[4];
            txtCustomer.Text = string.Empty;
            txtFiscal.Text = string.Empty;
            txtSku.Text = string.Empty;
            LoadItems();
            LoadPayments();
            LoadCalculatePanel();
            _ncfType = (NcfType)ncfTypeController.Get(2).response;
            NcfSelected();

        }

        private void LoadCalculatePanel() 
        {
            lblSubTotal.Text = amounts[0].ToString("###,###,##0.00");
            lblTax.Text = amounts[1].ToString("###,###,##0.00");
            lblDiscount.Text = amounts[2].ToString("###,###,##0.00");
            lblTotal.Text = amounts[3].ToString("###,###,##0.00");
            lblPending.Text = (amounts[3] - _transactionPayments.Sum(a => a.TotalAmount)).ToString("###,###,##0.00");
        }

        private void dtPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                paymentRow = e.RowIndex;
            }
            catch { }
        }

        private void dtPayments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_transactionPayments.Count > 0)
                {
                    var payment = _transactionPayments[paymentRow];

                    using (DeleteLine delete = new DeleteLine("Desea eliminar el pago:",
                                                            payment.PaymentMethod.Name,
                                                            "RD$ " + payment.TotalAmount.ToString("###,###,###")))
                    {
                        delete.ShowDialog();
                        if (delete.DialogResult == DialogResult.OK)
                        {
                            _transactionPayments.RemoveAt(paymentRow);
                            LoadPayments();
                            LoadCalculatePanel();
                        };
                    }
                }
            }
            catch { }
        }

        private void dtItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lineRow = e.RowIndex;
            }
            catch { }
        }

        private void dtItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (_transactionLines.Count > 0)
                {
                    var item = _transactionLines[lineRow];

                    using (SelectItemProcess process = new SelectItemProcess())
                    {
                        process.ShowDialog();
                        if (process.DialogResult == DialogResult.Yes)
                        {

                            using (DeleteLine delete = new DeleteLine("Desea eliminar el articulo:",
                                                                    item.Item.Sku,
                                                                    item.Description))
                            {
                                delete.ShowDialog();
                                if (delete.DialogResult == DialogResult.OK)
                                {
                                    _transactionLines.RemoveAt(lineRow);
                                    LoadPayments();
                                    CalculateLines();
                                };
                            }
                        }
                        else if (process.DialogResult == DialogResult.OK) 
                        {
                            using (AddQuantity add = new AddQuantity(item.Quantity))
                            {
                                add.ShowDialog();
                                if (add.DialogResult == DialogResult.OK)
                                {
                                    item.Quantity = add.Qty;
                                    CalculateLines();
                                };
                            }
                        }
                    }

                }
            }
            catch { }
        }

        private void btnVoidTransaction_Click(object sender, EventArgs e)
        {
            ClearTransactions();
        }

        private void PosView_Shown(object sender, EventArgs e)
        {
            using (LogIn frm = new LogIn())
            {
                while (genericController.GetCurrentUser() == null)
                {
                    frm.ShowDialog();

                    if (frm.DialogResult == DialogResult.OK)
                    {
                        var user = genericController.GetCurrentUser();
                        if (user != null)
                            break;
                    }
                }
            }
        }
    }
}
