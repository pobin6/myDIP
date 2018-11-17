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
        private Bitmap bitmapOrigin;
        public Bitmap BitmapOrigin {
            get
            {
                return bitmapOrigin;
            }
            set
            {
                bitmapOrigin = value;
                x = bitmapOrigin.Width;
                y = bitmapOrigin.Height;
            }
        }
        public Bitmap bitmapResult { get; set; }
        protected int x;
        protected int y;

        public delegate void ValueChange();
        public ValueChange valueChange = null;
    }
}
