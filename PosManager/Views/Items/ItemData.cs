using PosLibrary.Controller.Items;
using PosLibrary.Controller.Vendors;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Vendors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PosManager.Views.Items
{
    public partial class ItemData : Form
    {

        ItemController _dataController = new ItemController();
        Item _data = new Item();
        public ItemData()
        {
            InitializeComponent();
            LoadDiscount();
            LoadVendor();
            LoadDepartment();
            LoadTax();
        }

        public ItemData(int Id) : this()
        {
            //InitializeComponent();
            _data = (Item)_dataController.Get(Id).response;
            
            LoadItemData();
        }

        private void LoadItemData() 
        {
            txtCode.Text = _data.Sku;
            txtCode.Enabled = false;
            txtName.Text = _data.Name;
            numPrice.Value = _data.Price;
            cmbDiscount.SelectedIndex = Convert.ToInt32(_data.ItemDiscountId);
            cmbDepartment.SelectedIndex = Convert.ToInt32(_data.ItemDepartmentId);
            cmbTax.SelectedIndex = Convert.ToInt32(_data.ItemTaxId);
            cmbVendor.SelectedIndex = Convert.ToInt32(_data.VendorId);
        }

        private void LoadDiscount()
        {
            var list = new ItemDiscountController().GetList();

            if (list.result)
            {
                try
                {
                    #region Data Table
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Value");
                    dt.Columns.Add("Display");
                    #endregion

                    #region DB DATA
                    dt.Rows.Add(0, "--Seleccionar Opcion--");
                    foreach (var item in list.response as List<ItemDiscount>)
                        dt.Rows.Add(item.Id, string.Format("{0} -> {1}", item.Name, item.AmountPercent.ToString("##0.00")));
                    #endregion

                    cmbDiscount.DisplayMember = "display";
                    cmbDiscount.ValueMember = "value";
                    cmbDiscount.DataSource = dt;
                }
                catch { }
            }
        }

        private void LoadDepartment()
        {
            var list = new ItemDepartmentController().GetList();

            if (list.result)
            {
                try
                {
                    #region Data Table
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Value");
                    dt.Columns.Add("Display");
                    #endregion

                    #region DB DATA
                    dt.Rows.Add(0, "--Seleccionar Opcion--");
                    foreach (var item in list.response as List<ItemDepartment>)
                        dt.Rows.Add(item.Id, item.Name);
                    #endregion

                    cmbDepartment.DisplayMember = "display";
                    cmbDepartment.ValueMember = "value";
                    cmbDepartment.DataSource = dt;
                }
                catch { }
            }
        }

         private void LoadTax()
        {
            var list = new ItemTaxController().GetList();

            if (list.result)
            {
                try
                {
                    #region Data Table
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Value");
                    dt.Columns.Add("Display");
                    #endregion

                    #region DB DATA
                    dt.Rows.Add(0, "--Seleccionar Opcion--");
                    foreach (var item in list.response as List<ItemTax>)
                        dt.Rows.Add(item.Id, string.Format("{0} -> {1}", item.Name, item.AmountPercent.ToString("##0.00")));
                    #endregion

                    cmbTax.DisplayMember = "display";
                    cmbTax.ValueMember = "value";
                    cmbTax.DataSource = dt;
                }
                catch { }
            }
        }

        private void LoadVendor()
        {
            var list = new VendorController().GetList();

            if (list.result)
            {
                try
                {
                    #region Data Table
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Value");
                    dt.Columns.Add("Display");
                    #endregion

                    #region DB DATA
                    dt.Rows.Add(0, "--Seleccionar Opcion--");
                    foreach (var item in list.response as List<Vendor>)
                        dt.Rows.Add(item.Id, string.Format("{0} - {1} {2}", item.VendorId, item.FirstName, item.LastName));
                    #endregion

                    cmbVendor.DisplayMember = "display";
                    cmbVendor.ValueMember = "value";
                    cmbVendor.DataSource = dt;
                }
                catch { }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Item data = new Item()
            {
                Sku = txtCode.Text,
                Name = txtName.Text,
                Price = numPrice.Value,
                ItemDepartmentId = Convert.ToInt32(cmbDepartment.SelectedIndex),
                ItemDiscountId = Convert.ToInt32(cmbDiscount.SelectedIndex),
                ItemTaxId = Convert.ToInt32(cmbTax.SelectedIndex),
                VendorId = Convert.ToInt32(cmbVendor.SelectedIndex),
                Condition_Status = true,
                Deleted = false,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Producto guardado exitosamente");
                _data = null;
                Close();
            }
        }
    }
}
