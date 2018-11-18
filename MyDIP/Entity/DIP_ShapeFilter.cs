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
            Unsharp_Masking_Filter,
            Gradient_Filter
        }
        public enum Laplace_BD
        {
            Real_BD,
            Fake_BD
        }
        Laplace_BD bd;
        [DescriptionAttribute("Laplace标定模式选择")]
        public Laplace_BD BD
        {
            get { return bd; }
            set
            {
                switch (value)
                {
                    case Laplace_BD.Real_BD:
                        BDFunc = null;
                        BDFunc += RealBD;
                        break;
                    case Laplace_BD.Fake_BD:
                        BDFunc = null;
                        BDFunc += FakeBD;
                        break;
                }
                bd = value;
            }
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
                    case Filter_TYPE.Gradient_Filter:
                        valueFilterChange = null;
                        valueFilterChange += valueChangeGradientEvent;
                        break;
                }
                filter = value;
            }
        }
        [DescriptionAttribute("增强倍率")]
        public double c { get; set; }

        public ValueChange valueFilterChange = null;
        public delegate int[,,] BDDelegate(int[,,] color);
        public BDDelegate BDFunc = null;
        private static DIP_ShapeFilter _instance = null;
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();


        DIP_ShapeFilter()
        {
            Model = new List<int[]> { new int[3]{ 0, 1, 0 }, new int[3]{ 1, -4, 1 }, new int[3]{ 0, 1, 0 } };
            valueChange += valueChangeEvent;
            filter = Filter_TYPE.Laplace_Filter;
            BD = Laplace_BD.Real_BD;
            valueFilterChange += valueChangeLaplaceEvent;
            c = 1;
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

            int count = Model.Count;
            int start = count / 2;

            for (int i = start; i < x - start; i++) 
            {
                for (int j = start; j < y - start; j++)
                {
                    int[] sumColor = new int[3] { 0, 0, 0 };
                    for (int w = 0; w < count; w++)
                    {
                        for (int h = 0; h < count; h++)
                        {
                            for (int c = 0; c < 3; c++)
                            {
                               sumColor[c] += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), c) * Model[w][h];
                            }
                        }
                    }
                    color[0, i, j] = sumColor[0];
                    color[1, i, j] = sumColor[1];
                    color[2, i, j] = sumColor[2];
                }
            }
            // 标定
            color = BDFunc(color);
            // 叠加原图
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    Color pix = bitmapResult.GetPixel(i, j);
                    color[0, i, j] = (int)(pix.R - color[0, i, j] * c);
                    color[1, i, j] = (int)(pix.G - color[1, i, j] * c);
                    color[2, i, j] = (int)(pix.B - color[2, i, j] * c);
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
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    bitmapResult.SetPixel(i, j, Color.FromArgb(color[0, i, j], color[1, i, j], color[2, i, j]));
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
            int count = Model.Count;
            int start = count / 2;

            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    for (int w = 0; w < count; w++)
                    {
                        for (int h = 0; h < count; h++)
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

        private void valueChangeGradientEvent()
        {
            bitmapResult = BitmapOrigin.Clone() as Bitmap;

            int[,,] color = new int[3, x, y];
            int[] max = new int[3] { 0, 0, 0 };
            int[] min = new int[3] { 256, 256, 256 };
            int[,,] M = new int[2, 3, 3] 
            { 
                { 
                    { -1, -2, -1 }, 
                    { 0, 0, 0 }, 
                    { 1, 2, 1 } } , 
                { 
                    { -1, 0, 1 }, 
                    { -2, 0, 2 }, 
                    { -1, 0, 1 }
                }
            };
            int start = 1;
            int count = 3;
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    int[,] sumColor = new int[2, 3] { { 0, 0, 0 },{ 0, 0, 0 } };
                    for (int s = 0; s < 2; s++)
                    {

                        for (int w = 0; w < count; w++)
                        {
                            for (int h = 0; h < count; h++)
                            {
                                for (int c = 0; c < 3; c++)
                                {
                                    sumColor[s,c] += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), c) * M[s,w,h];
                                }
                            }
                        }
                        sumColor[s, 0] = Math.Abs(sumColor[s,0]);
                        sumColor[s, 1] = Math.Abs(sumColor[s,1]);
                        sumColor[s, 2] = Math.Abs(sumColor[s,2]);
                    }
                    color[0, i, j] = (sumColor[0, 0] + sumColor[1, 0]);
                    color[1, i, j] = (sumColor[0, 1] + sumColor[1, 1]);
                    color[2, i, j] = (sumColor[0, 2] + sumColor[1, 2]);
                }
            }
            color = BDFunc(color);
            //叠加原图
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    Color pix = bitmapResult.GetPixel(i, j);
                    color[0, i, j] = (int)(pix.R - color[0, i, j] * c);
                    color[1, i, j] = (int)(pix.G - color[1, i, j] * c);
                    color[2, i, j] = (int)(pix.B - color[2, i, j] * c);
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
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    bitmapResult.SetPixel(i, j, Color.FromArgb(color[0, i, j], color[1, i, j], color[2, i, j]));
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
        private int[,,] FakeBD(int[,,] color)
        {
            int start = 1;
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
            return color;
        }
        
    }
}
