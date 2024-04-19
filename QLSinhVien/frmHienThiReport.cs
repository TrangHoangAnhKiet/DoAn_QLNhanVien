using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSinhVien
{
    public partial class frmHienThiReport : Form
    {
        public frmHienThiReport()
        {
            InitializeComponent();
        }
        string _malop, _tenlop; //nhan du lieu tu formHocSinh (in theo lop nao?)
        public frmHienThiReport( string malop, string tenlop)
        {
            InitializeComponent();
            _malop = malop; _tenlop = tenlop;
        }

        private void frmHienThiReport_Load(object sender, EventArgs e)
        {
            //Nap du lieu
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            List<HocSinh> lst = new List<HocSinh>();
            if (_malop == ""){
                _malop = "All";
                lst = db.HocSinhs.ToList();
            }
            else lst = db.HocSinhs.Where(p => p.MaLop == _malop).ToList();
            //Nap du lieu parameter report
            ReportParameter[] parameter = new ReportParameter[2];
            parameter[0] = new ReportParameter("paMaLop", _malop);
            parameter[1] = new ReportParameter("paTenLop", _tenlop);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSetHocSinh",lst));
            reportViewer1.LocalReport.SetParameters(parameter);
            //Xuat du lieu
            this.reportViewer1.RefreshReport();
        }
    }
}
