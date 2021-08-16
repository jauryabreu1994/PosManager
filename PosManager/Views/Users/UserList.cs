using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
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

namespace PosManager.Views.Users
{
    public partial class UserList : UserControl
    {


        UserController _dataController = new UserController();
        User _data = new User();
        public UserList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData() 
        {
            var list = _dataController.GetList();

            if (list.result)
            {
                List<User> collection = list.response as List<User>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new { a.Id, a.UserId, 
                                                                     a.FirstName, a.LastName, 
                                                                     a.Phone }).ToList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (UserData cust = new UserData()) 
            {
                cust.ShowDialog();
            };
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                using (UserData cust = new UserData(_data.Id))
                {
                    cust.ShowDialog();
                };
                LoadData();
            }
            else
                MessageBox.Show("Debe seleccionar un usuario");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_data != null)
                {
                    string text = string.Format("Seguro que desea eliminar el cliente: {0} --> {1} {2}",
                                                _data.UserId, _data.FirstName, _data.LastName);
                    if (MessageBox.Show( text, "Cliente",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dataResp = _dataController.Delete(_data.Id);

                        if (!dataResp.result)
                            MessageBox.Show(dataResp.message);
                        else
                        {
                            MessageBox.Show("Cliente Eliminado");
                            _data = null;
                            LoadData();
                        }
                    }
                }
                else 
                    MessageBox.Show("Debe seleccionar un usuario");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var line = dtData.Rows[e.RowIndex].DataBoundItem;
                if (line != null)
                {
                    _data = new User();
                    _data.Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
                    _data.UserId = (string)line.GetType().GetProperty("UserId").GetValue(line, null);
                    _data.FirstName = (string)line.GetType().GetProperty("FirstName").GetValue(line, null);
                    _data.LastName = (string)line.GetType().GetProperty("LastName").GetValue(line, null);
                }
            }
            catch { }
        }

        private void btnUserGroup_Click(object sender, EventArgs e)
        {
            using (UserGroupData cust = new UserGroupData())
            {
                cust.ShowDialog();
            };
            LoadData();
        }
    }
}
