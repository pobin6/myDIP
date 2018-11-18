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
    public partial class FormSet : Form
    {
        EntityBase entity = null;
        public FormSet()
        {
            InitializeComponent();
        }

        public FormSet(EntityBase entity)
        {
            InitializeComponent();
            this.entity = entity;
        }

        private void FormSet_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = entity;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            entity.valueChange();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //string localFilePath, fileNameExt, newFileName, FilePath; 
            SaveFileDialog sfd = new SaveFileDialog();
            //设置文件类型 
            sfd.Filter = "(*.*)|*.*";
            //设置默认文件类型显示顺序 
            sfd.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string localFilePath = sfd.FileName.ToString(); //获得文件路径 
                entity.bitmapResult.Save(localFilePath);
            }
        }
    }
}
