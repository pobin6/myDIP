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
    }
}
