
namespace PosManager.Views.DashBoard
{
    partial class ChartMain
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.charPie = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dtData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.charPie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtData)).BeginInit();
            this.SuspendLayout();
            // 
            // charPie
            // 
            chartArea1.Name = "ChartArea1";
            this.charPie.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.charPie.Legends.Add(legend1);
            this.charPie.Location = new System.Drawing.Point(3, 3);
            this.charPie.Name = "charPie";
            this.charPie.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Departamento";
            this.charPie.Series.Add(series1);
            this.charPie.Size = new System.Drawing.Size(620, 172);
            this.charPie.TabIndex = 0;
            this.charPie.Text = "charPie";
            // 
            // chartLine
            // 
            chartArea2.Name = "ChartArea1";
            this.chartLine.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartLine.Legends.Add(legend2);
            this.chartLine.Location = new System.Drawing.Point(3, 181);
            this.chartLine.Name = "chartLine";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Ventas";
            this.chartLine.Series.Add(series2);
            this.chartLine.Size = new System.Drawing.Size(620, 165);
            this.chartLine.TabIndex = 1;
            this.chartLine.Text = "Chart Line";
            // 
            // dtData
            // 
            this.dtData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.dtData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dtData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dtData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dtData.ColumnHeadersHeight = 30;
            this.dtData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dtData.EnableHeadersVisualStyles = false;
            this.dtData.GridColor = System.Drawing.Color.White;
            this.dtData.Location = new System.Drawing.Point(3, 352);
            this.dtData.Name = "dtData";
            this.dtData.RowHeadersVisible = false;
            this.dtData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.dtData.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dtData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtData.Size = new System.Drawing.Size(620, 165);
            this.dtData.TabIndex = 2;
            // 
            // ChartMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.dtData);
            this.Controls.Add(this.chartLine);
            this.Controls.Add(this.charPie);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(626, 520);
            this.MinimumSize = new System.Drawing.Size(626, 520);
            this.Name = "ChartMain";
            this.Size = new System.Drawing.Size(626, 520);
            ((System.ComponentModel.ISupportInitialize)(this.charPie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart charPie;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLine;
        private System.Windows.Forms.DataGridView dtData;
    }
}
