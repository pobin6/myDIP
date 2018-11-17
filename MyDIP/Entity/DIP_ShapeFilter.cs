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
    public class DIP_ShapeFilter : EntityBase
    {
        public enum Filter_TYPE
        {
            Laplace_Filter,
            Unsharp_Masking_Filter
        }

        [DescriptionAttribute("滤波模板")]
        public List<int[]> Model { get; set; }

        public Filter_TYPE filter;
        [DescriptionAttribute("锐化滤波模式选择")]
        public Filter_TYPE Filter
        {
            get { return filter;}
            set
            {
                switch(value)
                {
                    case Filter_TYPE.Laplace_Filter:
                        valueFilterChange = null;
                        valueFilterChange += valueChangeLaplaceEvent;
                        break;
                    case Filter_TYPE.Unsharp_Masking_Filter:
                        valueFilterChange = null; 
                         valueFilterChange += valueChangeUnsharp_MaskingEvent;
                        break;
                }
                filter = value;
            }
        }
        public ValueChange valueFilterChange = null;
        private static DIP_ShapeFilter _instance = null;
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();


        DIP_ShapeFilter()
        {
            Model = new List<int[]> { new int[3]{ 1, 1, 1 }, new int[3]{ 1, -8, 1 }, new int[3]{ 1, 1, 1 } };
            valueChange += valueChangeEvent;
            filter = Filter_TYPE.Laplace_Filter;
            valueFilterChange += valueChangeLaplaceEvent;
        }
        
        public static DIP_ShapeFilter getInstance()
        {
            if (_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_ShapeFilter();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            valueFilterChange();
        }
        private void valueChangeLaplaceEvent()
        {
            bitmapResult = BitmapOrigin.Clone() as Bitmap;

            int[,,] color = new int[3, x, y];
            int[] max = new int[3] { 0, 0, 0 };
            int[] min = new int[3] { 256, 256, 256 };
            int start = Model.Count / 2;

            for (int i = start; i < x - start; i++) 
            {
                for (int j = start; j < y - start; j++)
                {
                    int[] sumColor = new int[3] { 0, 0, 0 };
                    for (int w = 0; w < Model.Count; w++)
                    {
                        for (int h = 0; h < Model.Count; h++)
                        {
                            for (int c = 0; c < 3; c++)
                            {
                               sumColor[c] += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), c) * Model[w][h];
                            }
                        }
                    }
                    for (int c = 0; c < 3; c++)
                    {
                        color[c,i, j] = sumColor[c];
                        if (color[c, i, j] < min[c]) min[c] = color[c, i, j];
                        if (color[c, i, j] > max[c]) max[c] = color[c, i, j];
                    }
                    //bitmapResult.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            for (int c = 0; c < 3; c++)
            {
                int d = max[c] - min[c];
                if (d < 256 && min[c] > 0) { setBmp(color, 256, 0, start); break; }
                if (d < 256 && min[c] < 0) { setBmp(color, 256 + min[c],min[c], start); break; }
                if(d > 256) { setBmp(color, max[c], min[c], start); break; }
            }
            

        }
        private void setBmp(int[,,] color,int max, int min,int start)
        {
            double range = 255 / (double)(max - min);
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        color[c, i, j] = (int)((color[c, i, j] - min) * range);
                    }
                    Color pix = bitmapResult.GetPixel(i, j);
                    int r = Range(pix.R - color[0, i, j]);
                    int g = Range(pix.G - color[1, i, j]);
                    int b = Range(pix.B - color[2, i, j]);
                    bitmapResult.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
        }
        private void valueChangeUnsharp_MaskingEvent()
        {
            bitmapResult = BitmapOrigin.Clone() as Bitmap;
            int sum = 0;
            foreach (var item in Model)
            {
                foreach (var it in item)
                {
                    sum += it;
                }
            }
            int start = Model.Count / 2;

            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    for (int w = 0; w < Model.Count; w++)
                    {
                        for (int h = 0; h < Model.Count; h++)
                        {
                            sumR += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 0) * Model[w][h];
                            sumG += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 1) * Model[w][h];
                            sumB += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 2) * Model[w][h];
                        }
                    }
                    bitmapResult.SetPixel(i, j, Color.FromArgb(sumR / sum, sumG / sum, sumB / sum));
                }
            }
            // 2 * BitmapOrigin - bitmapResult
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    int[] colorO = new int[3] { BitmapOrigin.GetPixel(i,j).R, BitmapOrigin.GetPixel(i, j).G , BitmapOrigin.GetPixel(i, j).B };
                    int[] colorR = new int[3] { bitmapResult.GetPixel(i, j).R, bitmapResult.GetPixel(i, j).G, bitmapResult.GetPixel(i, j).B };
                    int[] color = new int[3];
                    for (int c = 0; c < 3; c++)
                    {
                        color[c] = Range(2 * colorO[c] - colorR[c]);
                    }
                    bitmapResult.SetPixel(i, j, Color.FromArgb(color[0], color[1], color[2]));
                }
            }
        }

        private int Gray(Color color,int i)
        {
            switch(i)
            {
                case 0: return color.R;
                case 1: return color.G;
                case 2: return color.B;
            }
            return (color.R + color.G + color.B) / 3;
        }

        private int Range(int color)
        {
            if (color > 255)
                return 255;
            else if (color < 0)
                return 0;
            return color;
        }
    }
}
