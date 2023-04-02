using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ExecuteSigningSessions
{
    public static class DrawingEx
    {
        //https://stackoverflow.com/questions/1484759/quality-of-a-saved-jpg-in-c-sharp

        public static ImageCodecInfo GetEncoder(this ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);
        }

        public static void SaveJpegTo(this Image image, Stream stream, byte quality = 100)
        {
            // Get a bitmap.
            var jgpEncoder = ImageFormat.Jpeg.GetEncoder();

            var myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)quality);

            image.Save(stream, jgpEncoder, myEncoderParameters);
        }
    }
}
