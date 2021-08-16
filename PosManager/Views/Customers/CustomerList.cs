using PosLibrary.Controller.Customers;
using PosLibrary.Model.Entities.Customers;
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

namespace PosManager.Views.Customers
{
    public partial class CustomerList : UserControl
    {


        CustomerController _dataController = new CustomerController();
        Customer _data = new Customer();
        public CustomerList()
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
                List<Customer> collection = list.response as List<Customer>;
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new { a.Id, a.CustomerId, 
                                                                     a.FirstName, a.LastName, 
                                                                     a.Phone }).ToList();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (CustomerData cust = new CustomerData()) 
            {
                cust.ShowDialog();
            };
            LoadData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (CustomerData cust = new CustomerData(_data.Id))
            {
                cust.ShowDialog();
            };
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_data != null)
                {
                    string text = string.Format("Seguro que desea eliminar el cliente: {0} --> {1} {2}",
                                                _data.CustomerId, _data.FirstName, _data.LastName);
                    if (MessageBox.Show( text, "Cliente",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var dataResp = _dataController.Delete(_data.Id);

                        if (!dataResp.result)
                            MessageBox.Show(dataResp.message);
                        else
                        {
                            MessageBox.Show("Cliente Eliminado");
                            _data = null;
                            LoadData();
                        }
                    }
                }
                else 
                    MessageBox.Show("Debe seleccionar un Cliente");
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
                    _data = new Customer();
                    _data.Id = (int)line.GetType().GetProperty("Id").GetValue(line, null);
                    _data.CustomerId = (string)line.GetType().GetProperty("CustomerId").GetValue(line, null);
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
