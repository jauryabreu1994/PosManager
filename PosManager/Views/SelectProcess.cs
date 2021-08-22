using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views
{
    public partial class SelectProcess : Form
    {
        public SelectProcess()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void btnPos_Click(object sender, EventArgs e)
        {

            new Pos.PosView().Show();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {

            new DashBoard.MainMenu().Show();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}