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
    public class DIP_SF : EntityBase
    {
        [DescriptionAttribute("滤波模板第一行值")]
        public int[] r1 { get; set; }
        [DescriptionAttribute("滤波模板第二行值")]
        public int[] r2 { get; set; }
        [DescriptionAttribute("滤波模板第三行值")]
        public int[] r3 { get; set; }

        private static DIP_SF _instance = null;
        private static readonly object _locker = new object();
        private static readonly object _locker2 = new object();

        DIP_SF()
        {
            r1 = new int[] { 5, 5, 5 };
            r2 = new int[] { 1, 1, 1 };
            r3 = new int[] { 5, 5, 5 };
            valueChange += valueChangeEvent;
        }

        public static DIP_SF getInstance()
        {
            if (_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_SF();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            bitmapResult = bitmap.Clone() as Bitmap;
            int x = bitmap.Width;
            int y = bitmap.Height;
            int sum = r1[0] + r1[1] + r1[2] + r2[0] + r2[1] + r2[2] + r3[0] + r3[1] + r3[2];
            for (int i = 1; i < x-1; i++)
            {
                for (int j = 1; j < y-1; j++)
                {
                    int[,] r = new int[3, 3];
                    r[0, 0] = Gray(bitmapResult.GetPixel(i - 1, j - 1));
                    r[0, 1] = Gray(bitmapResult.GetPixel(i - 1, j ));
                    r[0, 2] = Gray(bitmapResult.GetPixel(i - 1, j + 1));
                    r[1, 0] = Gray(bitmapResult.GetPixel(i, j - 1));
                    r[1, 1] = Gray(bitmapResult.GetPixel(i, j));
                    r[1, 2] = Gray(bitmapResult.GetPixel(i, j + 1));
                    r[2, 0] = Gray(bitmapResult.GetPixel(i + 1, j - 1));
                    r[2, 1] = Gray(bitmapResult.GetPixel(i + 1, j));
                    r[2, 2] = Gray(bitmapResult.GetPixel(i + 1, j + 1));
                    int sumR = 0;
                    for (int w = 0; w < 3; w++)
                    {
                        for (int h = 0; h < 3; h++)
                        {
                            sumR += r[w, h];
                        }
                    }
                    int s = sumR / sum;
                    bitmapResult.SetPixel(i, j, Color.FromArgb(s, s, s));
                }
            }
        }
        private int Gray(Color color)
        {
            return (color.R + color.G + color.B) / 3;
        }
    }
}
