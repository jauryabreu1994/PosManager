
using HashLib;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using PosManager.Views.Customers;
using PosManager.Views.Fiscals;
using PosManager.Views.Items;
using PosManager.Views.Payments;
using PosManager.Views.Stores;
using PosManager.Views.Transactions;
using PosManager.Views.Users;
using PosManager.Views.Vendors;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PosManager.Views.DashBoard
{
    public partial class MainMenu : Form
    {
        private GenericController genericController = new GenericController();
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btnDashBoard_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Sales))
                return;

            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new ChartMain());

        }

        private void MainMenu_Shown(object sender, System.EventArgs e)
        {
            using (LogIn frm = new LogIn())
            {
                while (genericController.GetCurrentUser() == null)
                {
                    frm.ShowDialog();

                    if (frm.DialogResult == DialogResult.OK)
                    {
                        var user = genericController.GetCurrentUser();
                        if (user != null)
                        {
                            lblUserName.Text = user.UserId;
                            lblInfoUser.Text = string.Format("{0} {1}", user.FirstName, user.LastName);
                            btnDashBoard_Click(null, null);
                            break;
                        }
                    }
                }
            }
        }

        private void btnCustomer_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Customer))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new CustomerList());
        }

        private void btnItem_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Item))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new ItemList());
        }

        private void btnUser_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.User))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new UserList());
        }

        private void picture_Click(object sender, System.EventArgs e)
        {
            genericController.SetCurrentUser(null);
            lblUserName.Text = lblInfoUser.Text = string.Empty;
            MainMenu_Shown(sender, e);
        }

        private void btnFiscal_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Fiscal))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new FiscalList());
        }

        private void btnVendor_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Vendor))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new VendorList());
        }

        private void btnPayment_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.PaymentMethod))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new PaymentList());
        }

        private void btnSetting_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.LoginManager))
                return;
            using (StoreData store = new StoreData())
            {
                store.ShowDialog();
            };
        }

        private void btnChancePassword_Click(object sender, System.EventArgs e)
        {
            using (ChangePassword password = new ChangePassword())
            {
                password.ShowDialog();
            };
        }

        private void btnTransactions_Click(object sender, System.EventArgs e)
        {
            if (!genericController.ValidateAccess(PermissionAlias.Sales))
                return;
            this.panelView.Controls.Clear();
            this.panelView.Controls.Add(new TransactionList());
        }
    }
}
