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
    public partial class frmLop : Form
    {
        public frmLop()
        {
            InitializeComponent();      
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadDSLop();
        }

        private void loadDSLop()
        {
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            dgvLop.DataSource = db.Lops.ToList()
                .Select((p, index) => new { STT = index + 1, p.MaLop, p.TenLop, p.SiSo }).OrderBy(p => p.MaLop)
                .ToList();
        }

        private void dgvLop_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idrowclick = e.RowIndex;
            
            if (idrowclick == -1) return; //dong header
            //tim malop dang chon tai dong do
            string malop =dgvLop[1, idrowclick].Value.ToString();
            //hien thi du lieu
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            Lop l = db.Lops.Where(p => p.MaLop == malop).SingleOrDefault();
            txtMaLop.Text = l.MaLop;
            txtTenLop.Text = l.TenLop;
            txtSiSo.Text = l.SiSo.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string malop = txtMaLop.Text;
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            //kiem tra ton tai
            Lop l = db.Lops.Where(p => p.MaLop == malop).SingleOrDefault();
            if (l != null){
                //du lieu da ton tai => THEM: FAILED
                MessageBox.Show("Mã lớp đã tồn tại","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaLop.Focus();
                return;
            }
            l = new Lop();
            l.MaLop = txtMaLop.Text;
            l.TenLop = txtTenLop.Text;
            l.SiSo = short.Parse(txtSiSo.Text);
            db.Lops.InsertOnSubmit(l);
            db.SubmitChanges();
            //reload du lieu
            loadDSLop();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string malop = txtMaLop.Text;
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            //kiem tra ton tai
            Lop l = db.Lops.Where(p => p.MaLop == malop).SingleOrDefault();
            if (l == null)
            {
                //du lieu khong ton tai => XOA: FAILED
                MessageBox.Show("Mã lớp không tồn tại","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaLop.Focus();
                return;
            }
            db.Lops.DeleteOnSubmit(l);
            db.SubmitChanges();

            //reload du lieu
            loadDSLop();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string malop = txtMaLop.Text;
            dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
            //kiem tra ton tai
            Lop l = db.Lops.Where(p => p.MaLop == malop).SingleOrDefault();
            if (l == null)
            {
                //du lieu chua ton tai => SUA: FAILED
                MessageBox.Show("Mã lớp chưa tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaLop.Focus();
                return;
            }
            //ko update malop
            l.TenLop = txtTenLop.Text;
            l.SiSo = short.Parse(txtSiSo.Text);
            db.SubmitChanges();
            //reload du lieu
            loadDSLop();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            //tenlop = null => FULL DS
            string tenlop = txtTenLop.Text;
            if (tenlop == "") loadDSLop();
            else
            {
                dbQLSinhVienDataContext db = new dbQLSinhVienDataContext();
                dgvLop.DataSource = db.Lops.Where(p=> p.TenLop.Contains(tenlop)).ToList()
                    .Select((p, index) => new { STT = index + 1, p.MaLop, p.TenLop, p.SiSo }).OrderBy(p => p.MaLop)
                    .ToList();
            }
        }
    }
}
