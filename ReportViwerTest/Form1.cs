using Microsoft.Reporting.WinForms;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows.Forms;

namespace ReportViwerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var bs = new BindingSource();
            ////////////////////////////////////////////////////////////////////////////////
            //データベースからデータを取得する
            ////////////////////////////////////////////////////////////////////////////////
            using (var db = new Model1())
            {
                try
                {
                    //データベースよりデータを100件取得する
                    var query = db.Customer.Take(100).ToList();
                    //バインディングソースにデータをセットする
                    bs.DataSource = query;
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessage = "";
                    ex.EntityValidationErrors.SelectMany(error => error.ValidationErrors).ToList()
                        .ForEach(model => errorMessage = errorMessage + model.PropertyName + " - " + model.ErrorMessage);
                    MessageBox.Show(errorMessage);
                }
            }

            ////////////////////////////////////////////////////////////////////////////////
            //レポートにデータを設定する
            ////////////////////////////////////////////////////////////////////////////////
            ReportDataSource rs = new ReportDataSource();
            rs.Name = "DataSet1";
            //BindingSourceを指定する。一般的にここはDataTable
            rs.Value = bs;

            reportViewer2.Reset();
            reportViewer2.ProcessingMode = ProcessingMode.Local;
            //作成したレポートをここで指定する
            reportViewer2.LocalReport.ReportPath = "Report1.rdlc";
            reportViewer2.LocalReport.DataSources.Add(rs);
            reportViewer2.RefreshReport();
        }
    }
}
