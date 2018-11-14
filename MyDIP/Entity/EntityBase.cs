using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public abstract class EntityBase
    {
        public Bitmap bitmap { get; set; }
        public Bitmap bitmapResult { get; set; }

        public delegate void ValueChange();
        public ValueChange valueChange = null;
    }
}
