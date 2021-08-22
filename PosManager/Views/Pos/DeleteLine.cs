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
    public partial class DeleteLine : Form
    {

        public PaymentMethod payment = null;
        public DeleteLine(string header, string line1, string line2)
        {
            InitializeComponent();
            lblHeader.Text = header;
            lblLine1.Text = line1;
            lblLine2.Text = line2;
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnYes_Click(object sender, System.EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
