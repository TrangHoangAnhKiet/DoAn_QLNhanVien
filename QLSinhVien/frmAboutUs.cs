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
    public partial class frmAboutUs : Form
    {
        public frmAboutUs()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmAboutUs
            // 
            this.BackgroundImage = global::QLSinhVien.Properties.Resources.AboutUs;
            this.ClientSize = new System.Drawing.Size(833, 473);
            this.Name = "frmAboutUs";
            this.ResumeLayout(false);

        }
    }
}
