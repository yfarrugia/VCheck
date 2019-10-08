using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VCheckLib
{
    public class cImageProcessing
    {
        //Convert RGB To Matrix [Of Double]
        public static double[] ToMatrix(Bitmap BM, int MatrixRowNumber, int MatrixColumnNumber)
        {
            try{
            double HRate = ((Double)MatrixRowNumber / BM.Height);
            double WRate = ((Double)MatrixColumnNumber / BM.Width);
            double[] Result = new double[MatrixColumnNumber * MatrixRowNumber];

            for (int r = 0; r < MatrixRowNumber; r++)
            {
                for (int c = 0; c < MatrixColumnNumber; c++)
                {
                    Color color = BM.GetPixel((int)(c / WRate), (int)(r / HRate));

                    Result[r * MatrixColumnNumber + c] = 1 - (color.R * .3 + color.G * .59 + color.B * .11) / 255;
                }
            }
            return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Convert Double To Grey Level  
        public static Bitmap ToImage(double[] Matrix, int MatrixRowNumber, int MatrixColumnNumber,
                                                     int ImageHeight, int ImageWidth)
        {
            try{
            //Calculate the freq rate of the actual height and width to the matrixs height
            double HRate = ((double)ImageHeight / MatrixRowNumber);
            double WRate = ((double)ImageWidth / MatrixColumnNumber);
            //create a bitmap result place holder 	
            Bitmap Result = new Bitmap(ImageWidth, ImageHeight);
            
            //loop until the image height is reached 	
            for (int i = 0; i < ImageHeight; i++)
            {
                //loop until the image width is reached 	
                for (int j = 0; j < ImageWidth; j++)
                {
                    int x = (int)((double)j / WRate);
                    int y = (int)((double)i / HRate);

                    double temp = Matrix[y * MatrixColumnNumber + x];
                    Result.SetPixel(j, i, Color.FromArgb((int)((1 - temp) * 255), (int)((1 - temp) * 255), (int)((1 - temp) * 255)));

                }
            }
            return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Bitmap resizeImage(Image imgToResize, Size size)
        {
            try{
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nRatio = 0;
            float nRatioW = 0;
            float nRatioH = 0;

            nRatioW = ((float)size.Width / (float)sourceWidth);
            nRatioH = ((float)size.Height / (float)sourceHeight);

            if (nRatioH < nRatioW)
                nRatio = nRatioH;
            else
                nRatio = nRatioW;

            int targetWidth = (int)(sourceWidth * nRatio);
            int targetHeight = (int)(sourceHeight * nRatio);

            Bitmap b = new Bitmap(targetWidth, targetHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, targetWidth, targetHeight);
            g.Dispose();

            return b;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}
