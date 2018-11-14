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
        OpenFileDialog dialog = null;
        Bitmap bmpOrigin = null;
        EntityBase entity = null;
        public DIPForm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string file = dialog.FileName;
                bmpOrigin = new Bitmap(file);
                pictureBox2.Image = bmpOrigin;
            }
        }

        private void DIPForm_Load(object sender, EventArgs e)
        {
            dialog = new OpenFileDialog
            {
                Title = "请选择图片",
                Filter = "图片文件(*.jpg,*.gif,*.bmp,*.png)|*.jpg;*.gif;*.bmp,*.png"
            };
            DIP_Gray.getInstance().valueChange += ValueChange;
            DIP_Milv.getInstance().valueChange += ValueChange;
        }

        private void btnGray_Click(object sender, EventArgs e)
        {
            if(bmpOrigin == null)
            {
                MessageBox.Show("没有加载图片，请先打开一张图片");
                return;
            }
            entity = DIP_Gray.getInstance();
            DIP_Gray.getInstance().bitmap = bmpOrigin.Clone() as Bitmap;
            FormSet formSet = new FormSet(DIP_Gray.getInstance());
            formSet.Show();
        }
        private void ValueChange()
        {
            pictureBox1.Image = entity.bitmapResult;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bmpOrigin == null)
            {
                MessageBox.Show("没有加载图片，请先打开一张图片");
                return;
            }
            entity = DIP_Milv.getInstance();
            DIP_Milv.getInstance().bitmap = bmpOrigin.Clone() as Bitmap;
            FormSet formSet = new FormSet(DIP_Milv.getInstance());
            formSet.Show();
        }
    }
}
