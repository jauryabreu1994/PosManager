using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Transactions;
using PosManager.Controller;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class ItemList : Form
    {
        private int Id = 0;
        public Item item = new Item();
        private ItemController _dataController = new ItemController();
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
                    dtData.DataSource = collection.Select(a => new {
                        a.Id,
                        a.Sku,
                        Descripcion = a.Name,
                        Precio = a.Price.ToString("###,###,##0.00")
                    }).ToList();
            }
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var line = dtData.Rows[e.RowIndex].DataBoundItem;
            if (line != null)
            {
                Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
            }
        }

        private void dtData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                item = _dataController.Get(Id).response as Item;
                if (item != null)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch { }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            LoadData(txtItem.Text);
        }

        private void txtItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnSearch_Click(null, null);
            }
        }
    }
}
