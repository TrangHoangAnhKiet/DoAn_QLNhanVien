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
    public partial class frmHocSinh : Form
    {
        public frmHocSinh()
        {
            InitializeComponent();
        }

        private void frmHocSinh_Load(object sender, EventArgs e)
        {
            loadDSLop();
            loadDSHocSinh();
        }
        private void loadDSHocSinh(string malop="")
        {
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            if (malop=="")
            dgvHS.DataSource = db.HocSinhs.ToList()
                .Select((p, index) => new {STT=index+1, p.MaHS, p.TenHS,
                    NgaySinh = string.Format("{0:dd-MM-yyyy}", p.NgaySinh),
                    p.DiaChi, p.DTB}).ToList();
            else dgvHS.DataSource = db.HocSinhs.Where(p=> p.MaLop ==malop).ToList()
                .Select((p, index) => new { STT = index + 1,p.MaHS,p.TenHS,
                    NgaySinh = string.Format("{0:dd-MM-yyyy}", p.NgaySinh), p.DiaChi,p.DTB}).ToList();
        }

        private void loadDSLop()
        {
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            List<Lop> lst =  db.Lops.OrderBy(p => p.MaLop).ToList();
            Lop l = new Lop();  l.MaLop = ""; l.TenLop = "Tất cả";
            lst.Insert(0, l);
            cbbLop.DataSource = lst;
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";
        }

        private void cbbLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDSHocSinh(cbbLop.SelectedValue.ToString());

        }

        private void dgvHS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow < 0) return;
            string mahs = dgvHS[1, indexRow].Value.ToString();

            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            HocSinh hs = db.HocSinhs.Where(p => p.MaHS == mahs).SingleOrDefault();
            txtMaHS.Text = hs.MaHS;
            txtHoTen.Text = hs.TenHS;
            txtDiaChi.Text = hs.DiaChi;
            txtDiemTB.Text = hs.DTB.ToString();
            cbbLop.SelectedValue = hs.MaLop;
            dtpNgaySinh.Value = (DateTime)hs.NgaySinh;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if( !kiemtraMaLop()) return;    
            string mahs = txtMaHS.Text;
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            HocSinh hs = db.HocSinhs.Where(p => p.MaHS == mahs).SingleOrDefault();
            if(hs != null)
            {
                MessageBox.Show("Trùng mã học sinh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hs = new HocSinh();
            hs.MaHS = txtMaHS.Text;
            hs.TenHS = txtHoTen.Text;
            hs.DiaChi = txtDiaChi.Text;
            hs.DTB = float.Parse(txtDiemTB.Text);
            hs.NgaySinh = dtpNgaySinh.Value;
            hs.MaLop = cbbLop.SelectedValue.ToString();
            db.HocSinhs.InsertOnSubmit(hs);
            db.SubmitChanges();

            loadDSHocSinh(cbbLop.SelectedValue.ToString());
        }

        private bool kiemtraMaLop()
        {
            if (cbbLop.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Chưa chọn lớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string mahs = txtMaHS.Text;
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            HocSinh hs = db.HocSinhs.Where(p => p.MaHS == mahs).SingleOrDefault();
            if (hs == null)
            {
                MessageBox.Show("Mã học sinh không tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            db.HocSinhs.DeleteOnSubmit(hs);
            db.SubmitChanges();
            loadDSHocSinh(cbbLop.SelectedValue.ToString());
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!kiemtraMaLop()) return;
            string mahs = txtMaHS.Text;
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            HocSinh hs = db.HocSinhs.Where(p => p.MaHS == mahs).SingleOrDefault();
            if (hs == null)
            {
                MessageBox.Show("Mã học sinh chưa tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hs.TenHS = txtHoTen.Text;
            hs.DiaChi = txtDiaChi.Text;
            hs.DTB = float.Parse(txtDiemTB.Text);
            hs.NgaySinh = dtpNgaySinh.Value;
            hs.MaLop = cbbLop.SelectedValue.ToString();
            db.SubmitChanges();
            loadDSHocSinh(cbbLop.SelectedValue.ToString());
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            string hoten = txtHoTen.Text;
            if(hoten == "")
            {
                dgvHS.DataSource = db.HocSinhs.Where(p => p.MaLop == cbbLop.SelectedValue.ToString()).ToList()
                .Select((p, index) => new {STT = index + 1, p.MaHS,p.TenHS,
                    NgaySinh = string.Format("{0:dd-MM-yyyy}", p.NgaySinh),
                    p.DiaChi,p.DTB
                }).ToList();
            }
            else
            {
                dgvHS.DataSource = db.HocSinhs.Where(p => p.TenHS.Contains(hoten)).ToList()
                .Select((p, index) => new {
                    STT = index + 1,p.MaHS,p.TenHS,
                    NgaySinh = string.Format("{0:dd-MM-yyyy}", p.NgaySinh),
                    p.DiaChi, p.DTB
                }).ToList();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            loadDSHocSinh(cbbLop.SelectedValue.ToString());
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            //frmHienThi frm = new frmHienThi(cbbLop.SelectedValue.ToString(), cbbLop.Text);
            frmHienThiReport frm = new frmHienThiReport(cbbLop.SelectedValue.ToString(), cbbLop.Text);
            frm.ShowDialog();
        }
    }
}
