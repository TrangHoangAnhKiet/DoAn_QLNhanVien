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
    public partial class frmHienThi : Form
    {
        public frmHienThi()
        {
            InitializeComponent();
        }
        string malop, tenlop;
        public frmHienThi(string _malop, string _tenlop)
        {
            InitializeComponent();
            malop = _malop; tenlop = _tenlop;
        }
        private void frmHienThi_Load(object sender, EventArgs e)
        {
            //nap du lieu tu MaLop
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            List<HocSinh> lstHS = new List<HocSinh>();
            //NgaySinh = string.Format("{0:dd-MM-yyyy}", p.NgaySinh)
            if (malop == "") lstHS = db.HocSinhs.ToList();  
            else lstHS = db.HocSinhs.Where(p => p.MaLop == malop).ToList();

            //Tham số bình thường
            ReportParameter[] param = new ReportParameter[2];
            //ABCD là dữ liệu có thể lấy từ Textbox, Form khác
            param[0] = new ReportParameter("paMaLop", malop);
            param[1] = new ReportParameter("paTenLop", tenlop);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new
            ReportDataSource("DataSet1", lstHS));
            reportViewer1.LocalReport.SetParameters(param);

            //Xuat len Report
            this.reportViewer1.RefreshReport();
        }
    }
}
