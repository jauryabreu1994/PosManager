using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views
{
    public partial class AddQuantity : Form
    {
        public decimal Qty = 0;
        public AddQuantity(decimal qty)
        {
            InitializeComponent();
            numAmount.Value = qty;
            Qty = qty;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAddQuantity_Click(object sender, EventArgs e)
        {
            Qty = numAmount.Value;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnAddQuantity_Click(null, null);
            }
        }
    }
}
