using PosLibrary.Controller.Fiscal;
using PosLibrary.Controller.Items;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Fiscal;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Transactions;
using PosManager.Controller;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PosManager.Views.Pos
{
    public partial class NcfList : Form
    {
        private int ncfId = 2;
        public NcfType ncfType = new NcfType();
        public NcfList()
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
                    dtData.DataSource = collection.Where(a=>a.NcfId != 4).Select(a => new
                    {
                        Ncf_Id = a.NcfId,
                        Descripcion = a.Description
                    }).ToList();
            }
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dtData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var line = dtData.Rows[e.RowIndex].DataBoundItem;
            if (line != null)
            {
                ncfId = (int)line.GetType().GetProperty("Ncf_Id").GetValue(line, null);
            }
        }

        private void dtData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ncfType = new NcfTypeController().Get(ncfId).response as NcfType;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
