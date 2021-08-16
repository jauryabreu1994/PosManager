using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Users
{
    public partial class UserGroupData : Form
    {

        UserGroupController _dataController = new UserGroupController();
        UserGroup _data = new UserGroup();
        public UserGroupData()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var list = _dataController.GetList();

            if (list.result)
            {
                List<UserGroup> collection = list.response as List<UserGroup>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new {
                        Codigo = a.Id,
                        Descripcion = a.Name
                    }).ToList();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UserGroup data = new UserGroup()
            {
                Name = txtName.Text,
                Deleted = false,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Grupo de Usuario guardado exitosamente");
                _data = null;
                txtName.Text = string.Empty;
                LoadData();
            }
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var line = dtData.Rows[e.RowIndex].DataBoundItem;
                if (line != null)
                {
                    _data = new UserGroup();
                    _data.Id = (int)line.GetType().GetProperty("Codigo").GetValue(line, null);
                    _data.Name = (string)line.GetType().GetProperty("Descripcion").GetValue(line, null);
                    txtName.Text = _data.Name;
                }
            }
            catch { }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            _data = null;
            txtName.Text = string.Empty;
        }

        private void btnPermission_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                if (_data.Id > 0)
                {
                    using (GroupPermissionData cust = new GroupPermissionData(_data.Id, _data.Name))
                    {
                        cust.ShowDialog();
                    };
                }
            }
        }
    }
}
