using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDIP
{
    public partial class DIPForm : Form
    {
        EntityBase entity = null;
        public DIPForm()
        {
            InitializeComponent();
        }

        private void DIPForm_Load(object sender, EventArgs e)
        {
            DIP_Gray.getInstance().valueChange += ValueChange;
            DIP_Milv.getInstance().valueChange += ValueChange;
            DIP_HE.getInstance().valueChange += ValueChange;
            DIP_SoftFilter.getInstance().valueChange += ValueChange;
            DIP_ShapeFilter.getInstance().valueChange += ValueChange;
            DIP_Math.getInstance().valueChange += ValueChange;
        }

        private void btnGray_Click(object sender, EventArgs e)
        {
            entity = DIP_Gray.getInstance();
            FormSet formSet = new FormSet(DIP_Gray.getInstance());
            formSet.Show();
        }
        private void ValueChange()
        {
            new FormPic(entity.bitmapResult).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            entity = DIP_Milv.getInstance();
            FormSet formSet = new FormSet(DIP_Milv.getInstance());
            formSet.Show();
        }

        private void btnHE_Click(object sender, EventArgs e)
        {
            entity = DIP_HE.getInstance();
            FormSet formSet = new FormSet(DIP_HE.getInstance());
            formSet.Show();
        }

        private void btnSF_Click(object sender, EventArgs e)
        {
            entity = DIP_SoftFilter.getInstance();
            FormSet formSet = new FormSet(DIP_SoftFilter.getInstance());
            formSet.Show();
        }

        private void btnSpF_Click(object sender, EventArgs e)
        {
            entity = DIP_ShapeFilter.getInstance();
            FormSet formSet = new FormSet(DIP_ShapeFilter.getInstance());
            formSet.Show();
        }

        private void btnMath_Click(object sender, EventArgs e)
        {
            entity = DIP_Math.getInstance();
            FormSet formSet = new FormSet(DIP_Math.getInstance());
            formSet.Show();
        }
    }
}
