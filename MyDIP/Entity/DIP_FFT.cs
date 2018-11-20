using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace Entity
{
    /// <summary>
    /// 图片灰度化算法
    /// </summary>
    public class DIP_FFT:EntityBase
    {

        private static DIP_FFT _instance = null;
        private static readonly object _locker = new object();

        DIP_FFT()
        {
            valueChange += valueChangeEvent;
        }

        public static DIP_FFT getInstance()
        {
            if(_instance == null)
            {

                lock (_locker)
                {
                    _instance = new DIP_FFT();
                }
            }
            return _instance;
        }

        private void valueChangeEvent()
        {
            bitmapResult = BitmapOrigin.Clone() as Bitmap;
            x = bitmapResult.Width;
            y = bitmapResult.Height;
            Complex32[] matrix = new Complex32[x * y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    matrix[ i * x + j ] = bitmapResult.GetPixel(i, j).R;
                }
            }
            var ms = matrix.Clone();
            Fourier.Forward2D(matrix, x, y);

            //for (int i = 0; i < x; i++)
            //{
            //    for (int j = 0; j < y; j++)
            //    {
            //        var gray =  (byte)matrix[i * x + j].r
            //        bitmapResult.SetPixel(i, j, Color.FromArgb(matrix[i * x + j], matrix[i * x + j], matrix[i * x + j]));
            //    }
            //}
        }
    }
}
