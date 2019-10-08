using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
//using tessnet2;

namespace VCheckLib
{
    public class cDetect
    {
        //private tessnet2.Tesseract _ocr;
        private int cntWhite = 0;
        private int cntBlack = 0;

        public cDetect()
        {

        }

        // public List<Image<Gray, Byte>> DetectLicensePlate(Image<Bgr, byte> img,
        public Bitmap DetectLicensePlate(Image<Bgr, byte> img,
                List<Image<Gray, Byte>> licensePlateImagesList,
                List<Image<Gray, Byte>> filteredLicensePlateImagesList,
                List<MCvBox2D> detectedLicensePlateRegionList)
        {

            Bitmap bmp = img.ToBitmap();
            try
            {
                using (Image<Gray, byte> gray = img.Convert<Gray, Byte>())
                using (Image<Gray, Byte> canny = new Image<Gray, byte>(gray.Size))
                using (MemStorage stor = new MemStorage())
                {
                    CvInvoke.cvCanny(gray, canny, 100, 50, 3);

                    Contour<Point> contours = canny.FindContours(
                         Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                         Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE,
                         stor);
                    FindLicensePlate(contours, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList);
                }


                foreach (Image<Gray, Byte> i in filteredLicensePlateImagesList)
                {
                    colourCount(i.ToBitmap());
                    double bwRatio = (double)cntBlack / cntWhite;
                    if ((1.5 < bwRatio && bwRatio < 3.9) && (cntBlack != 0) && (cntWhite != 0))
                    {
                        bmp = i.ToBitmap();
                    }
                }
                return bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int GetNumberOfChildren(Contour<Point> contours)
        {
            try
            {
                Contour<Point> child = contours.VNext;
                if (child == null) return 0;
                int count = 0;
                while (child != null)
                {
                    count++;
                    child = child.HNext;
                }
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FindLicensePlate(
                Contour<Point> contours, Image<Gray, Byte> gray, Image<Gray, Byte> canny,
                List<Image<Gray, Byte>> licensePlateImagesList, List<Image<Gray, Byte>> filteredLicensePlateImagesList, List<MCvBox2D> detectedLicensePlateRegionList)
        {
            try
            {
                for (; contours != null; contours = contours.HNext)
                {
                    int numberOfChildren = GetNumberOfChildren(contours);
                    //if it does not contains any children (charactor), it is not a license plate region
                    if (numberOfChildren == 0) continue;

                    if (contours.Area > 400)
                    {
                        if (numberOfChildren < 3)
                        {
                            //If the contour has less than 3 children, it is not a license plate (assuming license plate has at least 3 charactor)
                            //However we should search the children of this contour to see if any of them is a license plate
                            FindLicensePlate(contours.VNext, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList);
                            continue;
                        }

                        MCvBox2D box = contours.GetMinAreaRect();
                        if (box.angle < -45.0)
                        {
                            float tmp = box.size.Width;
                            box.size.Width = box.size.Height;
                            box.size.Height = tmp;
                            box.angle += 90.0f;
                        }
                        else if (box.angle > 45.0)
                        {
                            float tmp = box.size.Width;
                            box.size.Width = box.size.Height;
                            box.size.Height = tmp;
                            box.angle -= 90.0f;
                        }

                        double whRatio = (double)box.size.Width / box.size.Height;
                        //if (!(3.0 < whRatio && whRatio < 10.0))
                        if (!(3.6 < whRatio && whRatio < 5.0))
                        {   //if the width height ratio is not in the specific range,it is not a license plate 
                            //However we should search the children of this contour to see if any of them is a license plate
                            Contour<Point> child = contours.VNext;
                            if (child != null)
                                FindLicensePlate(child, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList);
                            continue;
                        }
                        box.size.Width -= 6;
                        box.size.Height -= 6;
                        Image<Gray, Byte> plate = gray.Copy(box);
                        Image<Gray, Byte> filteredPlate = FilterPlate(plate);

                        using (Bitmap bmp = filteredPlate.Bitmap)

                            licensePlateImagesList.Add(plate);
                        filteredLicensePlateImagesList.Add(filteredPlate);
                        detectedLicensePlateRegionList.Add(box);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Image<Gray, Byte> FilterPlate(Image<Gray, Byte> plate)
        {
            try
            {
                Image<Gray, Byte> thresh = plate.ThresholdBinaryInv(new Gray(120), new Gray(255));

                using (Image<Gray, Byte> plateMask = new Image<Gray, byte>(plate.Size))
                using (Image<Gray, Byte> plateCanny = plate.Canny(new Gray(100), new Gray(50)))
                using (MemStorage stor = new MemStorage())
                {
                    plateMask.SetValue(255.0);
                    for (
                       Contour<Point> contours = plateCanny.FindContours(
                          Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                          Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL,
                          stor);
                       contours != null;
                       contours = contours.HNext)
                    {
                        Rectangle rect = contours.BoundingRectangle;
                        if (rect.Height > (plate.Height >> 1))
                        {
                            rect.X -= 1; rect.Y -= 1; rect.Width += 2; rect.Height += 2;
                            rect.Intersect(plate.ROI);

                            plateMask.Draw(rect, new Gray(0.0), -1);
                        }
                    }

                    thresh.SetValue(0, plateMask);
                }

                thresh._Erode(1);
                thresh._Dilate(1);

                return thresh;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //method to count number of white pixels and number of black pixels
        public void colourCount(Bitmap img)
        {
            try
            {
                cntWhite = 0;
                cntBlack = 0;

                for (int x = 0; x < img.Width; x++)
                {
                    for (int y = 0; y < img.Height; y++)
                    {
                        Color clr = img.GetPixel(x, y);
                        int r = clr.R;
                        int g = clr.G;
                        int b = clr.B;

                        if (r == 255)
                        {
                            cntWhite++;
                        }
                        else
                        {
                            cntBlack++;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
