using PosLibrary.Controller.Vendors;
using PosLibrary.Model.Entities.Vendors;
using System;
using System.Windows.Forms;

namespace PosManager.Views.Vendors
{
    public partial class VendorData : Form
    {

        VendorController _dataController = new VendorController();
        Vendor _data = new Vendor();
        public VendorData()
        {
            InitializeComponent();
        }

        public VendorData(int Id)
        {
            InitializeComponent();
            _data = (Vendor)_dataController.Get(Id).response;
            LoadVendorData();
        }

        private void LoadVendorData() 
        {
            txtCode.Text = _data.VendorId;
            txtCode.Enabled = false;
            txtName.Text = _data.FirstName;
            txtLastName.Text = _data.Address;
            txtCompany.Text = _data.CompanyName;
            txtPhone.Text = _data.Phone;
            txtVatNumber.Text = _data.VatNumber;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Vendor data = new Vendor()
            {
                Address = txtAddress.Text,
                CompanyName = txtCompany.Text,
                VendorId = txtCode.Text,
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
                MessageBox.Show("Suplidor guardado exitosamente");
                _data = null;
                Close();
            }
        }
    }
}
