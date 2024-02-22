using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Coach.Reports
{
    public partial class rptSubscribedCamp : DevExpress.XtraReports.UI.XtraReport
    {
        public double ResevedTotalCost { get; set; }
        public rptSubscribedCamp(double totalCost)
        {
            InitializeComponent();
            ResevedTotalCost = totalCost;
        }

        private void rptSubscribedCamp_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel2.Text = ResevedTotalCost.ToString();
            //xrPictureBox1.ImageUrl = "https://localhost:44354/Public/images/logo.png";
            xrPictureBox1.ImageUrl = "http://codewarenet-001-site8.dtempurl.com/Admin/assets/images/index.jpg";
        }
    }
}
