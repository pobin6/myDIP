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
    /// 平滑滤波算法
    /// </summary>
    public class DIP_Math : EntityBase
    {
        public enum Math_TYPE
        {
            Math_Add,
            Math_Multi,
            Math_Sub,
            Math_Div
        }
        public Math_TYPE math;
        [DescriptionAttribute("运算选择")]
        public Math_TYPE Math
        {
            get { return math; }
            set
            {
                switch(value)
                {
                    case Math_TYPE.Math_Add:
                        valueFilterChange = null;
                        valueFilterChange += Math_Add;
                        break;
                    case Math_TYPE.Math_Sub:
                        valueFilterChange = null; 
                         valueFilterChange += Math_MultiEvent;
                        break;
                    case Math_TYPE.Math_Multi:
                        valueFilterChange = null;
                        valueFilterChange += Math_MultiEvent;
                        break;
                    case Math_TYPE.Math_Div:
                        valueFilterChange = null;
                        valueFilterChange += Math_MultiEvent;
                        break;
                }
                math = value;
            }
        }
        public Bitmap BitmapO2 { get; set; }
        [DescriptionAttribute("系数c图2，c1图选择")]
        public double c { get; set; }
        public double c1 { get; set; }

        public ValueChange valueFilterChange = null;
        private static DIP_Math _instance = null;
        private static readonly object _locker = new object();



        DIP_Math()
        {
            valueChange += valueChangeEvent;
            math = Math_TYPE.Math_Add;
            valueFilterChange += Math_Add;
            c1 = 1;
            c = 0.5;
        }
        
        public static DIP_Math getInstance()
        {
            if (_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_Math();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            valueFilterChange();
        }
        private void Math_MultiEvent()
        {
            if(BitmapOrigin.Size != BitmapO2.Size)
            {
                return;
            }
            x = BitmapO2.Width;
            y = BitmapO2.Height;
            bitmapResult = new Bitmap(x, y);
            for (int i = 0; i < bitmapResult.Width; i++)
            {
                for (int j = 0; j < bitmapResult.Height; j++)
                {
                    var colorO = BitmapOrigin.GetPixel(i, j);
                    var colorR = BitmapO2.GetPixel(i, j);
                    int r = colorO.R * colorR.R / 255;
                    int g = colorO.G * colorR.G / 255;
                    int b = colorO.B * colorR.B / 255;
                    bitmapResult.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
        }
        private void Math_Add()
        {
            x = BitmapO2.Width;
            y = BitmapO2.Height;
            int[,,] color = new int[3,x,y];
            int start = 1;
            //叠加原图
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    Color pixR = BitmapO2.GetPixel(i, j);
                    Color pixO = BitmapOrigin.GetPixel(i, j);
                    color[0, i, j] = (int)(pixR.R * c1 - pixO.R * c);
                    color[1, i, j] = (int)(pixR.G * c1 - pixO.G * c);
                    color[2, i, j] = (int)(pixR.B * c1 - pixO.B * c);
                }
            }
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        color[c, i, j] = color[c, i, j] > 255 ? 255 : color[c, i, j];
                        color[c, i, j] = color[c, i, j] < 0 ? 0 : color[c, i, j];
                    }
                }
            }
            //color = RealBD(color);
            bitmapResult = new Bitmap(x, y);
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    bitmapResult.SetPixel(i, j, Color.FromArgb(color[0, i, j], color[1, i, j], color[2, i, j]));
                }
            }
        }

        private int[,,] RealBD(int[,,] color)
        {
            int start = 1;
            int[] max = new int[3] { 0, 0, 0 };
            int[] min = new int[3] { 256, 256, 256 };
            for (int c = 0; c < 3; c++)
            {
                for (int i = start; i < x - start; i++)
                {
                    for (int j = start; j < y - start; j++)
                    {
                        if (color[c, i, j] < min[c]) min[c] = color[c, i, j];
                        if (color[c, i, j] > max[c]) max[c] = color[c, i, j];
                    }
                }
            }
            for (int c = 0; c < 3; c++)
            {
                double range = 255 / (double)(max[c] - min[c]);
                for (int i = start; i < x - start; i++)
                {
                    for (int j = start; j < y - start; j++)
                    {

                        color[c, i, j] = (int)((color[c, i, j] - min[c]) * range);
                    }
                }
            }
            return color;
        }
    }
}
