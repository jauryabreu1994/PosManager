using PosLibrary.Controller.Customers;
using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Transactions;
using PosManager.Controller;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class CustomerList : Form
    {
        private int Id = 0;
        public Customer customer = new Customer();
        private CustomerController _dataController = new CustomerController();

        private string NcfSequence = string.Empty;
        public CustomerList()
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
                List<Customer> collection = list.response as List<Customer>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new {
                        Id = a.Id,
                        Codigo = a.CustomerId,
                        Nomnre = a.FirstName,
                        Apellido = a.LastName,
                        Cedula_RNC = a.VatNumber,
                        Empresa = a.CompanyName
                    }).ToList();
            }
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var line = dtData.Rows[e.RowIndex].DataBoundItem;
            if (line != null)
            {
                Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
            }
        }

        private void dtData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                customer = _dataController.Get(Id).response as Customer;
                if (customer != null)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch { }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            LoadData(txtCustomer.Text);
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnSearch_Click(null, null);
            }
        }
    }
}
