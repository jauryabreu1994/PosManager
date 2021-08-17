using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views.DashBoard
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            UserController userController = new UserController();
            User user = new GenericController().GetCurrentUser();
            if (!string.IsNullOrEmpty(txtCurrentPass.Text) && !string.IsNullOrEmpty(txtNewPass.Text)
                && !string.IsNullOrEmpty(txtNewPassValidate.Text))
            {
                if (txtNewPassValidate.Text == txtNewPass.Text)
                {
                    var response = userController.UpdatePassword(user.Id, txtCurrentPass.Text, txtNewPass.Text);
                    if (response.result)
                    {
                        new GenericController().SetCurrentUser((User)response.response);
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                        MessageBox.Show(response.message);
                }
                else
                    MessageBox.Show("Contraseña nueva y repetir contraseña estan diferentes.");

            }
            else
                MessageBox.Show("No puede tener campos vacios.");
        }

        private void enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (string.IsNullOrEmpty(txtCurrentPass.Text))
                    txtCurrentPass.Focus();
                else if (string.IsNullOrEmpty(txtNewPass.Text))
                    txtNewPass.Focus();
                else if (string.IsNullOrEmpty(txtNewPassValidate.Text))
                    txtNewPassValidate.Focus();
                else
                    btnChangePassword_Click(null, null);
            }
        }
    }
}
