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
    public class DIP_SoftFilter : EntityBase
    {
        public enum Filter_TYPE
        {
            Mean_Value_Filter,
            Middle_Value_Filter
        }

        [DescriptionAttribute("滤波模板")]
        public int[][] Model { get; set; }

        public Filter_TYPE filter;
        [DescriptionAttribute("滤波模板第三行值")]
        public Filter_TYPE Filter
        {
            get { return filter;}
            set
            {
                switch(value)
                {
                    case Filter_TYPE.Mean_Value_Filter:
                        valueFilterChange = null;
                        valueFilterChange += valueChangeMeanEvent;
                        break;
                    case Filter_TYPE.Middle_Value_Filter:
                        valueFilterChange = null; 
                         valueFilterChange += valueChangeMiddleEvent;
                        break;
                }
                filter = value;
            }
        }
        public ValueChange valueFilterChange = null;
        private static DIP_SoftFilter _instance = null;
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();


        DIP_SoftFilter()
        {
            Model = new int[][] { new int[3]{ 1, 1, 1 }, new int[3]{ 1, 1, 1 }, new int[3]{ 1, 1, 1 } };
            valueChange += valueChangeEvent;
            filter = Filter_TYPE.Mean_Value_Filter;
            valueFilterChange += valueChangeMeanEvent;
        }
        
        public static DIP_SoftFilter getInstance()
        {
            if (_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_SoftFilter();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            valueFilterChange();
        }
        private void valueChangeMeanEvent()
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
            int start = Model.Length / 2;

            for (int i = start; i < x - start; i++) 
            {
                for (int j = start; j < y - start; j++)
                {
                    int sumR = 0, sumG = 0, sumB = 0;
                    for (int w = 0; w < Model.Length; w++)
                    {
                        for (int h = 0; h < Model.Length; h++)
                        {
                            sumR += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 0) * Model[w][h];
                            sumG += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 1) * Model[w][h];
                            sumB += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 2) * Model[w][h];
                        }
                    }
                    bitmapResult.SetPixel(i, j, Color.FromArgb(sumR / sum, sumG / sum, sumB / sum));
                }
            }

        }

        private void valueChangeMiddleEvent()
        {
            bitmapResult = BitmapOrigin.Clone() as Bitmap;
            int start = Model.Length / 2;
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    int[,] PixR = new int[Model.Length, Model.Length];
                    int[,] PixG = new int[Model.Length, Model.Length];
                    int[,] PixB = new int[Model.Length, Model.Length];
                    for (int w = 0; w < Model.Length; w++)
                    {
                        for (int h = 0; h < Model.Length; h++)
                        {
                            PixR[w, h] = Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 0) * Model[w][h];
                            PixG[w, h] = Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 1) * Model[w][h];
                            PixB[w, h] = Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 2) * Model[w][h];
                        }
                    }
                    int middleR = PixR[0, 0], middleG = PixG[0, 0], middleB = PixB[0, 0];
                    int x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0, x6 = 0;
                    for (int m = 0; m < (Model.Length * Model.Length)/2 + 1; m++)
                    {
                        middleR = 255;
                        middleG = 255;
                        middleB = 255;
                        for (int n = 0; n < Model.Length; n++)
                        {
                            for (int l = 0; l < Model.Length; l++)
                            {
                                if (middleR > PixR[n,l])
                                {
                                    middleR = PixR[n,l];
                                    x1 = n;
                                    x2 = l;
                                }
                                if (middleG > PixG[n, l])
                                {
                                    middleG = PixG[n, l];
                                    x3 = n;
                                    x4 = l;
                                }
                                if (middleB > PixB[n, l])
                                {
                                    middleB = PixB[n, l];
                                    x5 = n;
                                    x6 = l;
                                }
                            }
                        }
                        PixR[x1, x2] = 255;
                        PixG[x3, x4] = 255;
                        PixB[x5, x6] = 255;
                    }
                    bitmapResult.SetPixel(i, j, Color.FromArgb(middleR, middleG, middleB));
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
    }
}
