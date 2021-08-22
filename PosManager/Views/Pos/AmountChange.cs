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
    public partial class AmountChange : Form
    {

        PaymentMethodController _dataController = new PaymentMethodController();
        public PaymentMethod payment = null;
        public AmountChange(decimal amount)
        {
            InitializeComponent();
            lblAmount.Text = "RD$ " + amount.ToString("###,###,##0.00");
            LoadData();
        }

        private void LoadData()
        {

            var list = _dataController.GetList();

            if (list.result)
            {
                var method = (list.response as List<PaymentMethod>).Where(a => a.IsMainTender).FirstOrDefault();
                lblMethod.Text = method.Name;
            }
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }


    }
}
