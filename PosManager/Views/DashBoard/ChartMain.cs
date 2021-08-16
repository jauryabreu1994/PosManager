using PosLibrary.Controller.Customers;
using PosLibrary.Controller.Items;
using PosLibrary.Controller.Transactions;
using PosLibrary.Model.Entities.Customers;
using PosLibrary.Model.Entities.Items;
using PosLibrary.Model.Entities.Transactions;
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

namespace PosManager.Views.DashBoard
{
    public partial class ChartMain : UserControl
    {
        public ChartMain()
        {
            InitializeComponent();
            LoadData();
            LoadChartDepartment();
        }

        private void LoadData() 
        {
            TransactionHeaderController _transHeaderController = new TransactionHeaderController();
            //List<TransactionHeader> _transHeader = new List<TransactionHeader>();

            var list = _transHeaderController.GetList();
            if (list.result)
            {
                List<TransactionHeader> collection = list.response as List<TransactionHeader>;
                collection = collection.Where(a => a.CreatedDate > DateTime.Now.AddMonths(-1))
                                       .OrderByDescending(a => a.CreatedDate)
                                       .ToList();
                if (collection.Count > 0)
                    dtData.DataSource = collection.Select(a => new {
                        Recibo = a.ReceiptId,
                        Cliente = string.Format("{0} {1}", a.Customer.FirstName, a.Customer.LastName),
                        Impuesto = a.TaxAmount,
                        Descuento = a.DiscountAmount,
                        Total = a.TotalPayment,
                        Fecha = a.CreatedDate.ToString("dd/MM/yyyy")
                    }).ToList();


                DataTable dt = new DataTable();
                dt.Columns.Add("Fecha");
                dt.Columns.Add("Ventas");

                DateTime dateEnd = DateTime.Now.Date.AddDays(-15);
                DateTime end = dateEnd;
                decimal amount = 0;

                for (int i = 0; i < 15; i++)
                {
                    amount = collection.Where(a => a.CreatedDate.Date > dateEnd.Date && 
                                                   a.CreatedDate.Date <= end.Date.AddDays(1))
                                                        .Sum(a => a.TotalAmount);
                    end = dateEnd = dateEnd.AddDays(+1);
                    dt.Rows.Add(string.Format("{0}-{1}", end.Day, end.Month), amount);
                }

                chartLine.DataSource = dt;
                chartLine.Series["Ventas"].XValueMember = "Fecha";
                chartLine.Series["Ventas"].YValueMembers = "Ventas";
                chartLine.DataBind();
            }

        }

        private void LoadChartDepartment() 
        {
            ItemDepartmentController _departmentController = new ItemDepartmentController();

            var list = _departmentController.GetList();
            if (list.result)
            {
                List<ItemDepartment> collection = list.response as List<ItemDepartment>;

                DataTable dt = new DataTable();
                dt.Columns.Add("Departamento");
                dt.Columns.Add("Cantidad");

                foreach (var item in collection)
                    dt.Rows.Add(item.Name, item.Items.Count);

                charPie.DataSource = dt;
                charPie.Series["Departamento"].XValueMember = "Departamento";
                charPie.Series["Departamento"].YValueMembers = "Cantidad";
                charPie.DataBind();
            }
        }
    }
}
