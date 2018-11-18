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

        //[DescriptionAttribute("滤波模板")]
        private int[,] Model { get; set; }
        private int r = 3;
        public int Row
        {
            get
            {
                return r;
            }
            set
            {
                r = value;
                Model = new int[r, r];
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < r; j++)
                    {
                        Model[i, j] = 1;
                    }
                }
            }
        }
        [DescriptionAttribute("平滑计算的RGB范围")]
        public int[][] RGB_Range { get; set; }
        public Filter_TYPE filter;
        [DescriptionAttribute("滤波器选择")]
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
            Row = 3;
            RGB_Range = new int[][] { new int[2] { 200, 250 }, new int[2] { 150, 200 }, new int[2] { 120, 180 } };
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
                    sum += item;
            }
            int count = Row;
            int start = count / 2;

            for (int i = start; i < x - start; i++) 
            {
                for (int j = start; j < y - start; j++)
                {
                    if(IsRange(bitmapResult.GetPixel(i, j)))
                    {
                        int sumR = 0, sumG = 0, sumB = 0;
                        for (int w = 0; w < count; w++)
                        {
                            for (int h = 0; h < count; h++)
                            {
                                sumR += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 0) * Model[w,h];
                                sumG += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 1) * Model[w,h];
                                sumB += Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 2) * Model[w,h];
                            }
                        }
                        bitmapResult.SetPixel(i, j, Color.FromArgb(sumR / sum, sumG / sum, sumB / sum));
                    }
                }
            }

        }

        private void valueChangeMiddleEvent()
        {
            bitmapResult = BitmapOrigin.Clone() as Bitmap;
            int count = Model.Length;
            int start = count / 2;
            for (int i = start; i < x - start; i++)
            {
                for (int j = start; j < y - start; j++)
                {
                    if (IsRange(bitmapResult.GetPixel(i, j)))
                    {
                        int[,] PixR = new int[count, count];
                        int[,] PixG = new int[count, count];
                        int[,] PixB = new int[count, count];
                        for (int w = 0; w < count; w++)
                        {
                            for (int h = 0; h < count; h++)
                            {
                                PixR[w, h] = Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 0) * Model[w,h];
                                PixG[w, h] = Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 1) * Model[w,h];
                                PixB[w, h] = Gray(bitmapResult.GetPixel(i + w - start, j + h - start), 2) * Model[w,h];
                            }
                        }
                        int middleR = PixR[0, 0], middleG = PixG[0, 0], middleB = PixB[0, 0];
                        int x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0, x6 = 0;
                        for (int m = 0; m < (count * count) / 2 + 1; m++)
                        {
                            middleR = 255;
                            middleG = 255;
                            middleB = 255;
                            for (int n = 0; n < count; n++)
                            {
                                for (int l = 0; l < count; l++)
                                {
                                    if (middleR > PixR[n, l])
                                    {
                                        middleR = PixR[n, l];
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

        private bool IsRange(Color color)
        {
            if (color.R > RGB_Range[0][0]
                && color.R < RGB_Range[0][1]
                && color.R > RGB_Range[1][0]
                && color.R < RGB_Range[1][1]
                && color.R > RGB_Range[2][0]
                && color.R < RGB_Range[2][1])
                return true;
            return false;
        }
    }
}
