using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Items
{
    public partial class ItemDiscountData : Form
    {

        ItemDiscountController _dataController = new ItemDiscountController();
        ItemDiscount _data = new ItemDiscount();
        public ItemDiscountData()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var list = _dataController.GetList();

            if (list.result)
            {
                List<ItemDiscount> collection = list.response as List<ItemDiscount>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new {
                        Codigo = a.Id,
                        Descripcion = a.Name,
                        Porcentaje = a.AmountPercent
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
            ItemDiscount data = new ItemDiscount()
            {
                Name = txtName.Text,
                AmountPercent = numPercent.Value,
                Deleted = false,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Grupo de Usuario guardado exitosamente");
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
                    _data = new ItemDiscount();
                    _data.Id = (int)line.GetType().GetProperty("Codigo").GetValue(line, null);
                    _data.Name = (string)line.GetType().GetProperty("Descripcion").GetValue(line, null);
                    _data.AmountPercent = (decimal)line.GetType().GetProperty("Porcentaje").GetValue(line, null);
                    txtName.Text = _data.Name;
                    numPercent.Value = _data.AmountPercent;
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
            numPercent.Value = 0;
        }
    }
}
