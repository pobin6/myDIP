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
                new FormPic(bmpOrigin).Show();
            }
        }

        private void DIPForm_Load(object sender, EventArgs e)
        {
            dialog = new OpenFileDialog
            {
                Title = "请选择图片",
                Filter = "(*.*)|*.*"
            };
            DIP_Gray.getInstance().valueChange += ValueChange;
            DIP_Milv.getInstance().valueChange += ValueChange;
            DIP_HE.getInstance().valueChange += ValueChange;
            DIP_SF.getInstance().valueChange += ValueChange;
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
            new FormPic(entity.bitmapResult).Show();
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

        private void btnHE_Click(object sender, EventArgs e)
        {
            if (bmpOrigin == null)
            {
                MessageBox.Show("没有加载图片，请先打开一张图片");
                return;
            }
            entity = DIP_HE.getInstance();
            DIP_HE.getInstance().bitmap = bmpOrigin.Clone() as Bitmap;
            DIP_HE.getInstance().width = bmpOrigin.Width;
            DIP_HE.getInstance().height = bmpOrigin.Height;
            FormSet formSet = new FormSet(DIP_HE.getInstance());
            formSet.Show();
        }

        private void btnSF_Click(object sender, EventArgs e)
        {
            if (bmpOrigin == null)
            {
                MessageBox.Show("没有加载图片，请先打开一张图片");
                return;
            }
            entity = DIP_SF.getInstance();
            DIP_SF.getInstance().bitmap = bmpOrigin.Clone() as Bitmap;
            FormSet formSet = new FormSet(DIP_SF.getInstance());
            formSet.Show();
        }
    }
}
