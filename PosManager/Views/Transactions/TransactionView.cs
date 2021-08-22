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

namespace PosManager.Views.Transactions
{
    public partial class TransactionView : Form
    {

        PaymentMethodController _dataController = new PaymentMethodController();
        public PaymentMethod payment = null;
        private int Id = 0;
        public TransactionView()
        {
            InitializeComponent();
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new TransactionList());
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }


    }
}
