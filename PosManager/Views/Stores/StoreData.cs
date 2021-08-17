using PosLibrary.Controller.StoreSetting;
using PosLibrary.Model.Entities.StoreSetting;
using System;
using System.Windows.Forms;

namespace PosManager.Views.Stores
{
    public partial class StoreData : Form
    {

        StoreController _dataController = new StoreController();
        Store _data = new Store();
        public StoreData()
        {
            InitializeComponent();
            _data = (Store)_dataController.Get(0).response;
            LoadStoreData();
        }

        private void LoadStoreData() 
        {
            txtDescription.Text = _data.Name;
            txtAddress.Text = _data.Address;
            txtCompany.Text = _data.CompanyName;
            txtVatNumber.Text = _data.VatNumber;
            txtEmail.Text = _data.Email;
            txtPhone.Text = _data.Phone;
            numCustomer.Value = _data.CustomerId;
            numReceipt.Value = _data.ReceiptId;
            numVendor.Value = _data.VendorId;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Store data = new Store()
            {
                Name = txtDescription.Text,
                Address = txtAddress.Text,
                CompanyName = txtCompany.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                VatNumber = txtVatNumber.Text,
                Condition_Status = true,
                Deleted = false,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Configuracion guardada exitosamente");
                _data = null;
                Close();
            }
        }
    }
}
