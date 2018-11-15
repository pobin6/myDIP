using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    /// <summary>
    /// 幂律算法
    /// </summary>
    public class DIP_Milv : EntityBase
    {
        [DescriptionAttribute("设置灰度值中R的比例 范围0-1")]
        public double c { get; set; }
        [DescriptionAttribute("设置灰度值中G的比例 范围0-1")]
        public double gamma { get; set; }

        private static DIP_Milv _instance = null;
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();

        DIP_Milv()
        {
            c = 1;
            gamma = 0.6;
            valueChange += valueChangeEvent;
        }

        public static DIP_Milv getInstance()
        {
            if (_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_Milv();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            bitmapResult = bitmap.Clone() as Bitmap;
            int x = bitmap.Width;
            int y = bitmap.Height;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var color = bitmapResult.GetPixel(i, j);
                    double r = (double)(color.R + color.G + color.B) / 3 / 255;
                    int s = (int)(c * Math.Pow(r , gamma) * 255);
                    bitmapResult.SetPixel(i, j, Color.FromArgb(s, s, s));
                }
            }
        }
    }
}
