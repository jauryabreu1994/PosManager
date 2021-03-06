using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
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

namespace PosManager.Views.Items
{
    public partial class ItemList : UserControl
    {
        private GenericController genericController = new GenericController();
        ItemController _dataController = new ItemController();
        Item _data = null;
        public ItemList()
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
                List<Item> collection = list.response as List<Item>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new { a.Id, a.Sku, 
                                                                     a.Name, Price = a.Price.ToString("###,###,##0.00") }).ToList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (ItemData cust = new ItemData()) 
            {
                cust.ShowDialog();
            };
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                using (ItemData cust = new ItemData(_data.Id))
                {
                    cust.ShowDialog();
                };
                LoadData();
            }
            else
                MessageBox.Show("Seleccionar una linea");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_data != null)
                {
                    string text = string.Format("Seguro que desea eliminar el producto: {0} --> {1} {2}",
                                                _data.Sku, _data.Name);
                    if (MessageBox.Show( text, "producto",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dataResp = _dataController.Delete(_data.Id);

                        if (!dataResp.result)
                            MessageBox.Show(dataResp.message);
                        else
                        {
                            MessageBox.Show("producto Eliminado");
                            _data = null;
                            LoadData();
                        }
                    }
                }
                else 
                    MessageBox.Show("Debe seleccionar un producto");
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
                    _data = new Item();
                    _data.Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
                    _data.Sku = (string)line.GetType().GetProperty("Sku").GetValue(line, null);
                    _data.Name = (string)line.GetType().GetProperty("Name").GetValue(line, null);
                }
            }
            catch { }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (! genericController.ValidateAccess(PermissionAlias.Discount))
                return;
            using (ItemDiscountData cust = new ItemDiscountData())
            {
                cust.ShowDialog();
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Tax))
                return;
            using (ItemTaxData cust = new ItemTaxData())
            {
                cust.ShowDialog();
            };
        }

        private void btnUserGroup_Click(object sender, EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Group))
                return;
            using (ItemDepartmentData cust = new ItemDepartmentData())
            {
                cust.ShowDialog();
            };
        }
    }
}
