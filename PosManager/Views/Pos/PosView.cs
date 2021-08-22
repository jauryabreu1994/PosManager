using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Transactions;
using PosManager.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class PosView : Form
    {
        private NcfType _ncfType = new NcfType();
        private Customer _customer = new Customer(); 
        private Item _item = new Item();
        private List<TransactionLines> _transactionLines = new List<TransactionLines>();
        private List<TransactionPayments> _transactionPayments = new List<TransactionPayments>();
        private GenericController genericController = new GenericController();
        private ItemController itemContoller = new ItemController();
        private decimal[] amounts = new decimal[5];

        private string NcfSequence = string.Empty;
        public PosView()
        {
            InitializeComponent();
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
            amounts = new decimal[5];
            foreach (var line in _transactionLines)
            {
                amounts[0] += line.Price * line.Quantity
                    ;
                line.TaxAmount = (_ncfType.NcfId != 14) ? CalculateTax(line.Price, line.Item.ItemTax.AmountPercent) : 0;
                amounts[1] += line.TaxAmount;

                line.TaxPercent = (_ncfType.NcfId != 14) ? line.Item.ItemTax.AmountPercent : 0;
                line.TotalAmount = (_ncfType.NcfId != 14) ? (line.Price + line.TaxAmount) * line.Quantity : 
                                    line.Price * line.Quantity;

                if (line.DiscountPercent != 0)
                {
                    decimal discountAmount = CalculateDiscount(line.TotalAmount, line.DiscountPercent);
                    amounts[2] += line.TotalAmount - discountAmount;
                    line.TotalAmount = discountAmount;
                }

                amounts[3] += line.TotalAmount;
            }
            LoadItems();

            lblSubTotal.Text = amounts[0].ToString("###,###,##0.00");
            lblTax.Text = amounts[1].ToString("###,###,##0.00");
            lblDiscount.Text = amounts[2].ToString("###,###,##0.00");
            lblTotal.Text = amounts[3].ToString("###,###,##0.00");
            lblPending.Text = (amounts[3] - amounts[4]).ToString("###,###,##0.00");

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
            using (ItemList item = new ItemList())
            {
                item.ShowDialog();
                if (item.DialogResult == DialogResult.OK)
                {
                    _item = item.item;
                    ItemsSelected();

                }
            };
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

            using (CustomerList list = new CustomerList())
            {
                list.ShowDialog();
                if (list.DialogResult == DialogResult.OK)
                {
                    _customer = list.customer;
                    CustomerSelected();

                }
            };
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (_transactionLines.Count > 0)
            {
                using (PaymentList list = new PaymentList())
                {
                    list.ShowDialog();
                    if (list.DialogResult == DialogResult.OK)
                    {
                        var payment = list.payment;

                        using (PaymentAmount pay = new PaymentAmount(amounts[3] - amounts[4],
                                                                     payment.OverTender,
                                                                     payment.UnderTender))
                        {
                            pay.ShowDialog();
                            if (pay.DialogResult == DialogResult.OK)
                            {
                                amounts[4] += pay.amount;
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

                                lblPending.Text = (amounts[3] - amounts[4]).ToString("###,###,##0.00");
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
    }
}
