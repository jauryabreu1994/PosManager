
using HashLib;
using PosManager.Controller;
using PosManager.Views.Customers;
using PosManager.Views.Fiscals;
using PosManager.Views.Item;
using PosManager.Views.Users;
using System.Text;
using System.Windows.Forms;

namespace PosManager.Views.DashBoard
{
    public partial class MainMenu : Form
    {
        private LoginController login = new LoginController();
        public MainMenu()
        {
            InitializeComponent();
            btnDashBoard_Click(null, null);
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btnDashBoard_Click(object sender, System.EventArgs e)
        {
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new ChartMain());
        }

        private void MainMenu_Shown(object sender, System.EventArgs e)
        {
            //using (LogIn frm = new LogIn())
            //{
            //    while (login.GetCurrentUser() == null)
            //    {
            //        frm.ShowDialog();

            //        if (frm.DialogResult == DialogResult.OK)
            //        {
            //            var user = login.GetCurrentUser();
            //            if (user != null)
            //            {
            //                lblUserName.Text = user.UserId;
            //                lblInfoUser.Text = string.Format("{0} {1}", user.FirstName, user.LastName);
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        private void btnCustomer_Click(object sender, System.EventArgs e)
        {

            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new CustomerList());

        }

        private void btnItem_Click(object sender, System.EventArgs e)
        {
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new ItemList());
        }

        private void btnUser_Click(object sender, System.EventArgs e)
        {

            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new UserList());
        }

        private void picture_Click(object sender, System.EventArgs e)
        {
            login.SetCurrentUser(null);
            lblUserName.Text = lblInfoUser.Text = string.Empty;
            MainMenu_Shown(sender, e);
        }

        private void btnFiscal_Click(object sender, System.EventArgs e)
        {
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new FiscalList());
        }
    }
}
