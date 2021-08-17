using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views.DashBoard
{
    public partial class LogIn : Form
    {
        private GenericController genericController = new GenericController();
        public LogIn()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Environment.Exit(1);
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            UserController userController = new UserController();
            if (!string.IsNullOrEmpty(txtUser.Text) && !string.IsNullOrEmpty(txtPass.Text))
            { 
                var response = userController.LogIn(txtUser.Text, txtPass.Text);
                if (response.result)
                {
                    genericController.SetCurrentUser((User)response.response);
                    if(genericController.ValidateAccess(PermissionAlias.LoginManager))
                        this.DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show(response.message);
            }
            else
                MessageBox.Show("Usuario o Contraseña no puede estar vacio.");
        }

        private void enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (string.IsNullOrEmpty(txtUser.Text))
                    txtUser.Focus();
                else if (string.IsNullOrEmpty(txtPass.Text))
                    txtPass.Focus();
                else
                    btnLogIn_Click(null, null);
            }
        }
    }
}
