using PosLibrary.Controller.Items;
using PosLibrary.Controller.Payments;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Payments;
using PosLibrary.Model.Entities.Transactions;
using PosManager.Controller;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class PaymentList : Form
    {

        PaymentMethodController _dataController = new PaymentMethodController();
        public PaymentMethod payment = null;
        private int Id = 0;
        public PaymentList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {

            var list = _dataController.GetList();

            if (list.result)
            {
                List<PaymentMethod> collection = list.response as List<PaymentMethod>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Where(a=>a.ToSales).Select(a => new
                    {
                        a.Id,
                        Descripcion = a.Name
                    }).ToList();
            }
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
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
                payment = _dataController.Get(Id).response as PaymentMethod;
                if (payment != null)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch { }
        }
    }
}
