using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace MVCWeb.Libraries
{
    public class Utility
    {
        public static void SetSession(string sessionName, object sessionValue)
        {
            HttpContext.Current.Session[sessionName] = sessionValue;
        }
        public static void RemoveSession(string sessionName)
        {
            HttpContext.Current.Session.Remove(sessionName);
        }
        public static object GetSession(string sessionName)
        {
            return HttpContext.Current.Session[sessionName];
        }
        public static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
        {
            if (image.Width <= size.Width && image.Height <= size.Height)
                return image;
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                var originalWidth = image.Width;
                var originalHeight = image.Height;
                var percentWidth = (float)size.Width / originalWidth;
                var percentHeight = (float)size.Height / originalHeight;
                var percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (var graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public static Image ResizeImageWithBackground(Image img, Size size)
        {
            int sourceWidth = img.Width;
            int sourceHeight = img.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent;

            var nPercentW = ((float)size.Width / sourceWidth);
            var nPercentH = ((float)size.Height / sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((size.Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((size.Height -
                              (sourceHeight * nPercent)) / 2);
            }

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var bmPhoto = new Bitmap(size.Width, size.Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(img.HorizontalResolution,
                             img.VerticalResolution);

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(img,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static void SaveJpeg(Bitmap img, string path, int quality)
        {
            // Encoder parameter for image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // Jpeg image codec
            var jpegCodec = GetEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }
    }
}