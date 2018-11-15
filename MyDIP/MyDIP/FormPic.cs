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
    public partial class FormPic : Form
    {
        public FormPic()
        {
            InitializeComponent();
        }
        public FormPic(Bitmap bitmap)
        {
            InitializeComponent();
            pictureBox1.Image = bitmap;
        }
    }
}
