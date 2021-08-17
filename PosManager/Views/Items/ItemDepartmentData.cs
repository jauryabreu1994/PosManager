using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Items
{
    public partial class ItemDepartmentData : Form
    {

        ItemDepartmentController _dataController = new ItemDepartmentController();
        ItemDepartment _data = new ItemDepartment();
        public ItemDepartmentData()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var list = _dataController.GetList();

            if (list.result)
            {
                List<ItemDepartment> collection = list.response as List<ItemDepartment>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new {
                        Codigo = a.Id,
                        Descripcion = a.Name
                    }).ToList();
            }
            CleanData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ItemDepartment data = new ItemDepartment()
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
                MessageBox.Show("Departamento guardado exitosamente");
                CleanData();
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
                    _data = new ItemDepartment();
                    _data.Id = (int)line.GetType().GetProperty("Codigo").GetValue(line, null);
                    _data.Name = (string)line.GetType().GetProperty("Descripcion").GetValue(line, null);
                    txtName.Text = _data.Name;
                }
            }
            catch
            {
                CleanData();
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            CleanData();
        }

        private void CleanData()
        {
            _data = null;
            txtName.Text = string.Empty;
        }
    }
}
