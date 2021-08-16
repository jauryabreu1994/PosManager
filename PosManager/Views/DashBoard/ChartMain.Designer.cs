
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
            this.dtData = new System.Windows.Forms.DataGridView();
            this.charPie = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartLine = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dtData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.charPie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLine)).BeginInit();
            this.SuspendLayout();
            // 
            // dtData
            // 
            this.dtData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtData.GridColor = System.Drawing.Color.White;
            this.dtData.Location = new System.Drawing.Point(3, 352);
            this.dtData.Name = "dtData";
            this.dtData.Size = new System.Drawing.Size(620, 165);
            this.dtData.TabIndex = 12;
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
            this.charPie.TabIndex = 13;
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
            this.chartLine.TabIndex = 14;
            this.chartLine.Text = "Chart Line";
            // 
            // ChartMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.chartLine);
            this.Controls.Add(this.charPie);
            this.Controls.Add(this.dtData);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(626, 520);
            this.MinimumSize = new System.Drawing.Size(626, 520);
            this.Name = "ChartMain";
            this.Size = new System.Drawing.Size(626, 520);
            ((System.ComponentModel.ISupportInitialize)(this.dtData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.charPie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dtData;
        private System.Windows.Forms.DataVisualization.Charting.Chart charPie;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLine;
    }
}
