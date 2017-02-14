using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace SCML.App_Code
{
    public static class PhotoManager
    {
        public static byte[] ResizeImage(HttpPostedFileBase fileUploadControl, int maxHeight, int maxWidth)
        {
            string fileName = Path.GetFileName(fileUploadControl.FileName); 
            string fileExtension = System.IO.Path.GetExtension(fileName);
            System.Drawing.Image imageFile = System.Drawing.Image.FromStream(fileUploadControl.InputStream);
            int imageHeight = imageFile.Height;
            int imageWidth = imageFile.Width;

            imageHeight = (imageHeight * maxWidth) / imageWidth;
            imageWidth = maxWidth;

            if (imageHeight > maxWidth)
            {
                imageWidth = (imageWidth * maxHeight) / imageHeight;
                imageHeight = maxHeight;
            }

            Bitmap bitmapFile = new Bitmap(imageFile, maxWidth, maxHeight);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmapFile.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;

            byte[] imageData = new byte[stream.Length + 1];
            stream.Read(imageData, 0, imageData.Length);

            return imageData;
        }

        public static byte[] ResizeImageFile(byte[] imageFile, int targetSize)
        {
            try
            {
                using (System.Drawing.Image oldImage = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
                {
                    Size newSize = CalculateDimensions(oldImage.Size, targetSize);
                    using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format16bppRgb555))
                    {
                        using (Graphics canvas = Graphics.FromImage(newImage))
                        {
                            canvas.SmoothingMode = SmoothingMode.AntiAlias;
                            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                            MemoryStream m = new MemoryStream();
                            newImage.Save(m, ImageFormat.Jpeg);
                            return m.GetBuffer();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }


        }

        public static Size CalculateDimensions(Size oldSize, int targetSize)
        {
            Size newSize = new Size();
            if (oldSize.Height > oldSize.Width)
            {
                newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
                newSize.Height = targetSize;
            }
            else
            {
                newSize.Width = targetSize;
                newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
            }
            return newSize;
        }
    }
}