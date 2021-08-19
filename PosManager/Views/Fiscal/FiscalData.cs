using PosLibrary.Controller.Fiscal;
using PosLibrary.Model.Entities.Fiscal;
using PosManager.Controller;
using System;
using System.Windows.Forms;

namespace PosManager.Views.Fiscals
{
    public partial class FiscalData : Form
    {

        NcfSequenceDetailController _dataController = new NcfSequenceDetailController();
        NcfSequenceDetail _data = new NcfSequenceDetail();
        public FiscalData(int ncfId)
        {
            InitializeComponent();
            txtNcf.Text = ncfId.ToString("00");
            numStart.Value = numNext.Value = 1;
            numEnd.Value = 100;
            txtStatus.Text = "0";
            dtEnd.Value = new DateTime(DateTime.Now.AddYears(2).Year, 1, 1).AddSeconds(-1);
        }

        public FiscalData(int ncfId, int Id)
        {
            InitializeComponent();
            _data = (NcfSequenceDetail)_dataController.Get(Id).response;
            LoadFiscalData();
        }

        private void LoadFiscalData() 
        {
            txtSerie.Text = _data.Serie;
            txtNcf.Text = _data.NcfId.ToString("00");
            txtDescription.Text = _data.DGIIDescription;
            txtStatus.Text = _data.SeqStatus.ToString();
            dtStart.Value = _data.DateStart;
            dtEnd.Value = _data.DateEnd;
            numStart.Value = _data.SeqStart;
            numNext.Value = _data.SeqNext;
            numEnd.Value = _data.SeqEnd;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NcfSequenceDetail data = new NcfSequenceDetail()
            {
                Serie = txtSerie.Text,
                NcfId = Convert.ToInt32(txtNcf.Text),
                DGIIDescription = txtDescription.Text,
                SeqStatus = Convert.ToInt32(txtStatus.Text),
                DateStart = dtStart.Value,
                DateEnd = dtEnd.Value,
                SeqStart = (int)numStart.Value,
                SeqNext = (int)numNext.Value,
                SeqEnd = (int)numEnd.Value,
            };
            data.Id = _data != null ? _data.Id : 0;

            var dataResp = _dataController.Save(data);
            
            if (!dataResp.result)
                MessageBox.Show(dataResp.message);
            else
            {
                MessageBox.Show("Configuracion fiscal guardada exitosamente");
                _data = null;
                Close();
            }
        }

        private bool ValidateField()
        {
            if (string.IsNullOrEmpty(txtSerie.Text))
            {
                txtSerie.Focus();
                return new GenericController().MessageError("El campo Serie no puede estar vacio");
            }

            else if (string.IsNullOrEmpty(txtNcf.Text))
            {
                txtSerie.Focus();
                return new GenericController().MessageError("El campo Tipo Ncf no puede estar vacio");
            }

            else if (string.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescription.Focus();
                return new GenericController().MessageError("El campo Descripcion no puede estar vacio");
            }

            else if (numStart.Value < 0)
            {
                numStart.Focus();
                return new GenericController().MessageError("La Seq. Siguiente no debe ser menor o igual a Cero");
            }

            else if (numNext.Value < 0)
            {
                numNext.Focus();
                return new GenericController().MessageError("La Seq. Siguiente no debe ser menor o igual a Cero");
            }

            else if (numEnd.Value < 0)
            {
                numNext.Focus();
                return new GenericController().MessageError("La Seq. Siguiente no debe ser menor o igual a Cero");
            }

            return true;


        }
    }
}
