using PosLibrary.Controller.Users;
using PosLibrary.Model.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Users
{
    public partial class GroupPermissionData : Form
    {

        GroupPermissionController _dataController = new GroupPermissionController();
        GroupPermission _data = new GroupPermission();
        private int UserGroupId = 0;
        public GroupPermissionData(int Id, string Description)
        {
            InitializeComponent();
            UserGroupId = Id;
            LoadData();
            lblHeader.Text = "Permisos de: " + Description;
        }

        private void LoadData()
        {
            var line = new GroupPermissionController().GetList(UserGroupId);

            if (line.result)
            {
                var groupPermissions = line.response as List<GroupPermission>;
                cmbCustomer.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Customer && 
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbDiscount.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Discount &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbFiscal.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Fiscal &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbDepartment.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Group &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbItem.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Item &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbManager.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.LoginManager &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbPos.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.LoginPos &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbPayment.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.PaymentMethod &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbSales.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Sales &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbTax.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Tax &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbUser.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.User &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;
                cmbVendor.SelectedIndex = groupPermissions.Any(a => a.PermissionCode == PermissionAlias.Vendor &&
                                                                      a.Condition_Status && !a.Deleted) ? 1 : 0;

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<GroupPermission> groups = new List<GroupPermission>();

            if (cmbCustomer.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Customer });

            if (cmbDepartment.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Group });
            if (cmbDiscount.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Discount });
            if (cmbFiscal.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Fiscal });
            if (cmbItem.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Item });
            if (cmbManager.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.LoginManager });
            if (cmbPayment.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.PaymentMethod });
            if (cmbPos.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.LoginPos });
            if (cmbSales.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Sales });
            if (cmbTax.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Tax });
            if (cmbUser.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.User });
            if (cmbVendor.SelectedIndex == 1)
                groups.Add(new GroupPermission() { UserGroupId = this.UserGroupId, PermissionCode = PermissionAlias.Vendor });

            _dataController.ChangeGeneralStatus(this.UserGroupId);

            foreach (var data in groups)
            {
                var dataResp = _dataController.Save(data);
            }
            MessageBox.Show("Asignacion de Permisos guardada exitosamente");
            _data = null;
            Close();
        }

    }
}
