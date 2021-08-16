using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PosManager.Views.Users
{
    public partial class UserData : Form
    {

        UserController _dataController = new UserController();
        User _data = new User();
        public UserData()
        {
            InitializeComponent();
            LoadUserGroup();
        }

        public UserData(int Id)
        {
            InitializeComponent();
            _data = (User)_dataController.Get(Id).response;
            LoadUserGroup();
            LoadUserData();
        }

        private void LoadUserGroup() 
        {
            var list = new UserGroupController().GetList();

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
                    foreach (var item in list.response as List<UserGroup>)
                        dt.Rows.Add(item.Id, item.Name);
                    #endregion

                    cmbUserGroup.DisplayMember = "display";
                    cmbUserGroup.ValueMember = "value";
                    cmbUserGroup.DataSource = dt;
                }
                catch { }
            }
        }

        private void LoadUserData() 
        {
            txtCode.Text = _data.UserId;
            txtCode.Enabled = false;
            txtName.Text = _data.FirstName;
            txtLastName.Text = _data.LastName;
            cmbGender.SelectedIndex = _data.Gender;
            cmbUserGroup.SelectedValue = _data.UserGroupId;
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
            User data = new User()
            {
                Address = txtAddress.Text,
                Gender = Convert.ToByte(cmbGender.SelectedIndex),
                UserGroupId = Convert.ToInt32(cmbUserGroup.SelectedValue),
                UserId = txtCode.Text,
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
                MessageBox.Show("Usuario guardado exitosamente");
                _data = null;
                Close();
            }
        }
    }
}
