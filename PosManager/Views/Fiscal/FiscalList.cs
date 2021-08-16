using PosLibrary.Controller.Fiscal;
using PosLibrary.Model.Entities.Fiscal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Fiscals
{
    public partial class FiscalList : UserControl
    {


        NcfSequenceDetailController _dataController = new NcfSequenceDetailController();
        NcfSequenceDetail _data = null;
        private int ncfId = 2;
        public FiscalList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData() 
        {
            var list = new NcfTypeController().GetList();

            if (list.result)
            {
                List<NcfType> collection = list.response as List<NcfType>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new
                    {
                        Ncf_Id = a.NcfId,
                        Descripcion = a.Description,
                        Principal_Ventas = a.IsDefaultSale,
                        Principal_Nota_de_Credito = a.IsDefaultCreditMemo
                    }).ToList();
            }
        }
        private void LoadSequence()
        {
            var list = _dataController.GetList(ncfId);

            if (list.result)
            {
                List<NcfSequenceDetail> collection = list.response as List<NcfSequenceDetail>;
                if (collection.Count > 0)
                    dtSequence.DataSource = collection.Select(a => new
                    {
                        a.Id,
                        a.Serie,
                        Inicio_Sequencia = a.SeqStart,
                        Siguiente_Sequencia = a.SeqNext,
                        Fin_Sequencia = a.SeqEnd,
                        Inicio_Fecha = a.DateStart,
                        Fin_Fecha = a.DateEnd,
                        Descripcion = a.DGIIDescription

                    }).ToList();
                else
                    dtSequence.DataSource = null;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (FiscalData cust = new FiscalData(ncfId)) 
            {
                cust.ShowDialog();
            };
            LoadSequence();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                using (FiscalData cust = new FiscalData(ncfId, _data.Id))
                {
                    cust.ShowDialog();
                };
                LoadSequence();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_data != null)
                {
                    string text = string.Format("Seguro que desea eliminar el Configuracion de Ncf: {0} --> {1}",
                                                ncfId, _data.DGIIDescription);
                    if (MessageBox.Show( text, "Configuracion de Ncf",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dataResp = _dataController.Delete(_data.Id);

                        if (!dataResp.result)
                            MessageBox.Show(dataResp.message);
                        else
                        {
                            MessageBox.Show("Configuracion de Ncf Eliminado");
                            _data = null;
                            LoadSequence();
                        }
                    }
                }
                else 
                    MessageBox.Show("Debe seleccionar una Configuracion de Ncf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var line = dtData.Rows[e.RowIndex].DataBoundItem;
                if (line != null)
                {
                    ncfId = (int)line.GetType().GetProperty("Ncf_Id").GetValue(line, null);
                    LoadSequence();
                }
            }
            catch { }
        }

        private void dtSequence_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var line = dtSequence.Rows[e.RowIndex].DataBoundItem;
                if (line != null)
                {
                    _data = new NcfSequenceDetail();
                    _data.Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
                    _data.DGIIDescription = (string)line.GetType().GetProperty("Descripcion").GetValue(line, null);
                    LoadSequence();
                }
            }
            catch { _data = null; }
        }
    }
}
