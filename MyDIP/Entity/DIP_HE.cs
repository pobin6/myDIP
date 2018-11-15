using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entity
{
    /// <summary>
    /// 直方图均值化算法 ——还有待改进，在局部均值化时，容易黑白化
    /// </summary>
    public class DIP_HE : EntityBase
    {
        [DescriptionAttribute("直方图应用区域 范围0-图像宽")]
        public int width { get; set; }
        [DescriptionAttribute("直方图应用区域 范围0-图像长")]
        public int height { get; set; }

        private static DIP_HE _instance = null;
        private static readonly object _locker = new object();

        DIP_HE()
        {
            valueChange += valueChangeEvent;
        }

        public static DIP_HE getInstance()
        {
            if (_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_HE();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            try
            {
                bitmapResult = bitmap.Clone() as Bitmap;
                int x = bitmap.Width/width;
                int y = bitmap.Height/height;
                int w=0, h=0;
                for (w = 0; w < x; w++)
                {
                    for (h = 0; h < y; h++)
                    {
                        HE(w * width, h * height, (w + 1) * width, (h + 1) * height);
                    }
                }
                HE((w + 1) * width, (h + 1) * height, bitmapResult.Width, bitmap.Height);
            }
            catch
            {
                MessageBox.Show("图片格式不支持");
            }
            
        }

        private void HE(int w,int h,int we,int he)
        {
            // 计算直方图
            Dictionary<int, double> pixSameNum = new Dictionary<int, double>();
            for (int i = w; i < we; i++)
            {
                for (int j = h; j < he; j++)
                {
                    var color = bitmapResult.GetPixel(i, j);
                    int r = (color.R + color.G + color.B) / 3;
                    if (pixSameNum.Keys.Contains(r))
                    {
                        pixSameNum[r]++;
                    }
                    else
                    {
                        pixSameNum.Add(r, 1);
                    }
                }
            }
            // 计算均衡直方图
            double p = 0;
            double pixAll = bitmapResult.Width * bitmapResult.Height;
            double s0 = 0, s1 = 0;
            int[] L = new int[256];
            Array.Clear(L, 0, 256);
            for (int i = 0; i < pixSameNum.Count; i++)
            {
                p = pixSameNum[pixSameNum.Keys.ElementAt(i)] / pixAll;
                s1 = p + s0;
                L[255 - pixSameNum.Keys.ElementAt(i)] = (int)(s1 * 255);
                s0 = s1;
            }
            // 应用均衡直方图
            for (int i = w; i < we; i++)
            {
                for (int j = h; j < he; j++)
                {
                    var color = bitmapResult.GetPixel(i, j);
                    int r = (color.R + color.G + color.B) / 3;
                    bitmapResult.SetPixel(i, j, Color.FromArgb(L[r], L[r], L[r]));
                }
            }
        }
    }
}
