using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Entity
{
    public class DIP_Gray:EntityBase
    {
        private double _R;
        [DescriptionAttribute("设置灰度值中R的比例 范围0-1")]
        public double R {
            get { return _R; }
            set { _R = value;}
        }
        private double _G;
        [DescriptionAttribute("设置灰度值中G的比例 范围0-1")]
        public double G
        {
            get { return _G; }
            set { _G = value;}
        }
        private double _B;
        [DescriptionAttribute("设置灰度值中B的比例 范围0-1")]
        public double B
        {
            get { return _B; }
            set { _B = value;}
        }

        private static DIP_Gray _instance = null;
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();

        DIP_Gray()
        {
            valueChange += valueChangeEvent;
        }

        public static DIP_Gray getInstance()
        {
            if(_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_Gray();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            bitmapResult = bitmap.Clone() as Bitmap;
            double sum = _R + _G + _B;
            double r = _R / sum;
            double g = _G / sum;
            double b = _B / sum;

            int x = bitmap.Width;
            int y = bitmap.Height;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var color = bitmapResult.GetPixel(i, j);
                    int gray = (int)(color.R * r + color.G * g + color.B * b);
                    bitmapResult.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
        }
    }
}
