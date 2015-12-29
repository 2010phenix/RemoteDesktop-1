using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Client
{
    public static class ImageFunctions
    {
        public static byte[] TakeScreenshot(int index)
        {
            Screen monitor = Screen.AllScreens[index];

            using (var bmp = new Bitmap(monitor.Bounds.Width, monitor.Bounds.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                    g.CopyFromScreen(monitor.Bounds.X, monitor.Bounds.Y, 0, 0, monitor.Bounds.Size);

                using (var ms = new MemoryStream())
                {
                    EncoderParameters myEncoderParameters = new EncoderParameters();
                    myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 20L);

                    bmp.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);

                    return ms.ToArray();
                }
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
