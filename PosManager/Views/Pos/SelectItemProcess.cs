using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class SelectItemProcess : Form
    {
        public SelectItemProcess()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnQty_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            Close();
        }
    }
}