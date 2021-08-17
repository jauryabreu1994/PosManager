using PosLibrary.Controller.Items;
using PosLibrary.Controller.Payments;
using PosLibrary.Model.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Payments
{
    public partial class PaymentList : UserControl
    {


        PaymentMethodController _dataController = new PaymentMethodController();
        PaymentMethod _data = null;
        public PaymentList()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string filter = "") 
        {
            
            var list = string.IsNullOrEmpty(filter) ? 
                                    _dataController.GetList() : 
                                    _dataController.GetList(filter);

            if (list.result)
            {
                List<PaymentMethod> collection = list.response as List<PaymentMethod>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new
                    {
                        a.Id,
                        Descripcion = a.Name,
                        Ventas = a.ToSales,
                        Devoluciones = a.ToReturn,
                        Principal = a.IsMainTender
                    }).ToList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (PaymentData cust = new PaymentData()) 
            {
                cust.ShowDialog();
            };
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                using (PaymentData cust = new PaymentData(_data.Id))
                {
                    cust.ShowDialog();
                };
                LoadData();
            }
            else
                MessageBox.Show("Seleccionar una linea");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_data != null)
                {
                    string text = string.Format("Seguro que desea eliminar el medio de pago: {0}",
                                                _data.Name);
                    if (MessageBox.Show( text, "Medio de Pago",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dataResp = _dataController.Delete(_data.Id);

                        if (!dataResp.result)
                            MessageBox.Show(dataResp.message);
                        else
                        {
                            MessageBox.Show("Medio de Pago Eliminado");
                            _data = null;
                            LoadData();
                        }
                    }
                }
                else 
                    MessageBox.Show("Debe seleccionar un Medio de Pago");
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
                    _data = new PaymentMethod();
                    _data.Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
                    _data.Name = (string)line.GetType().GetProperty("Descripcion").GetValue(line, null);
                }
            }
            catch { }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }
    }
}
