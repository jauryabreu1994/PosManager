using PosLibrary.Controller.Customers;
using PosLibrary.Controller.StoreSetting;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Enums;
using System;
using System.Windows.Forms;

namespace PosManager.Views.Customers
{
    public partial class CustomerData : Form
    {

        CustomerController _dataController = new CustomerController();
        Customer _data = new Customer();
        public CustomerData()
        {
            InitializeComponent();
            var data = new StoreController().GenerateId(StoreTableType.Customer);
            if (data.result)
                txtCode.Text = data.response.ToString();
        }

        public CustomerData(int Id)
        {
            InitializeComponent();
            _data = (Customer)_dataController.Get(Id).response;
            LoadCustomerData();
        }

        private void LoadCustomerData() 
        {
            txtCode.Text = _data.CustomerId;
            txtCode.Enabled = false;
            txtName.Text = _data.FirstName;
            txtLastName.Text = _data.LastName;
            txtCompany.Text = _data.CompanyName;
            dtDate.Value = _data.DateBorn;
            txtEmail.Text = _data.Email;
            txtPhone.Text = _data.Phone;
            txtVatNumber.Text = _data.VatNumber;
            txtAddress.Text = _data.Address;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Customer data = new Customer()
            {
                Address = txtAddress.Text,
                CompanyName = txtCompany.Text,
                DateBorn = dtDate.Value,
                CustomerId = txtCode.Text,
                Email = txtEmail.Text,
                FirstName = txtName.Text,
                LastName = txtLastName.Text,
                VatNumber = txtVatNumber.Text,
                Phone = txtPhone.Text,
                Condition_Status = true,
                Deleted = false,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Cliente guardado exitosamente");
                _data = null;
                Close();
            }
        }
    }
}
