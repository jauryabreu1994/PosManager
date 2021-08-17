using PosLibrary.Controller.Payments;
using PosLibrary.Model.Entities.Payments;
using System;
using System.Windows.Forms;

namespace PosManager.Views.Payments
{
    public partial class PaymentData : Form
    {

        PaymentMethodController _dataController = new PaymentMethodController();
        PaymentMethod _data = new PaymentMethod();
        public PaymentData()
        {
            InitializeComponent();
        }

        public PaymentData(int Id)
        {
            InitializeComponent();
            _data = (PaymentMethod)_dataController.Get(Id).response;
            LoadPaymentData();
        }

        private void LoadPaymentData() 
        {
            txtName.Text = _data.Name;
            cmbMain.SelectedIndex = Convert.ToInt32(_data.IsMainTender);
            cmbNC.SelectedIndex = Convert.ToInt32(_data.ToReturn);
            cmbPayDown.SelectedIndex = Convert.ToInt32(_data.UnderTender);
            cmbPayUp.SelectedIndex = Convert.ToInt32(_data.OverTender);
            cmbSales.SelectedIndex = Convert.ToInt32(_data.ToSales);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PaymentMethod data = new PaymentMethod()
            {
                Name = txtName.Text,
                IsMainTender = Convert.ToBoolean(cmbMain.SelectedIndex),
                ToSales = Convert.ToBoolean(cmbSales.SelectedIndex),
                ToReturn = Convert.ToBoolean(cmbNC.SelectedIndex),
                OverTender = Convert.ToBoolean(cmbPayUp.SelectedIndex),
                UnderTender = Convert.ToBoolean(cmbPayDown.SelectedIndex),
                Condition_Status = true,
                Deleted = false,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Medio de Pago guardado exitosamente");
                _data = null;
                Close();
            }
        }
    }
}
