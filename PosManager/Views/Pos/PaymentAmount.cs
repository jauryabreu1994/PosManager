using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views
{
    public partial class PaymentAmount : Form
    {
        public decimal amount = 0;
        public PaymentAmount(decimal pendingAmount, bool OverTender, bool UnderTender)
        {
            InitializeComponent();
            numAmount.Value = pendingAmount;

            if (!OverTender)
                numAmount.Maximum = numAmount.Value;

            if (!UnderTender)
                numAmount.Minimum = numAmount.Value;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnPaymentAmount_Click(object sender, EventArgs e)
        {
            amount = numAmount.Value;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnPaymentAmount_Click(null, null);
            }
        }
    }
}
