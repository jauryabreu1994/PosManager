using PosLibrary.Controller.Vendors;
using PosLibrary.Model.Entities.Vendors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosManager.Views.Vendors
{
    public partial class VendorList : UserControl
    {


        VendorController _dataController = new VendorController();
        Vendor _data = null;
        public VendorList()
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
                List<Vendor> collection = list.response as List<Vendor>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new { a.Id, a.VendorId, 
                                                                     a.FirstName, a.LastName, 
                                                                     a.Phone }).ToList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (VendorData cust = new VendorData()) 
            {
                cust.ShowDialog();
            };
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_data != null)
            {
                using (VendorData cust = new VendorData(_data.Id))
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
                    string text = string.Format("Seguro que desea eliminar el Suplidor: {0} --> {1} {2}",
                                                _data.VendorId, _data.FirstName, _data.LastName);
                    if (MessageBox.Show( text, "Suplidor",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dataResp = _dataController.Delete(_data.Id);

                        if (!dataResp.result)
                            MessageBox.Show(dataResp.message);
                        else
                        {
                            MessageBox.Show("Suplidor Eliminado");
                            _data = null;
                            LoadData();
                        }
                    }
                }
                else 
                    MessageBox.Show("Debe seleccionar un Suplidor");
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
                    _data = new Vendor();
                    _data.Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
                    _data.VendorId = (string)line.GetType().GetProperty("VendorId").GetValue(line, null);
                    _data.FirstName = (string)line.GetType().GetProperty("FirstName").GetValue(line, null);
                    _data.LastName = (string)line.GetType().GetProperty("LastName").GetValue(line, null);
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
