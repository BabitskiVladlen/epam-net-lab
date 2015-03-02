using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BLL.Tools
{
    public class ImageProcessor
    {
        #region CompressImage
        public void CompressImage(Image img, string mimeType, int quality, Stream storage)
        {
            try { Validator(img, mimeType, quality, storage); }
            catch (ArgumentException exc) { throw exc; }

            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo codec = GetEncoderInfo(mimeType);
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(storage, codec, encoderParams);
        } 
        #endregion

        #region GetEncoderInfo
        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var c in codecs)
                if (c.MimeType == mimeType)
                    return c;
            return null;
        } 
        #endregion

        #region Validator
        private void Validator(Image img, string mimeType, int quality, Stream storage)
        {
            if (img == null)
                throw new ArgumentNullException("Image is null", (Exception)null);
            if (String.IsNullOrEmpty(mimeType) ||
                String.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentNullException("MIME type is null or empty", (Exception)null);
            if ((quality < 1) || (quality > 100))
                throw new ArgumentException("Quality must be between 1 and 100", (Exception)null);
            if (storage == null) throw new ArgumentNullException("Output stream is null", (Exception)null);
            if (!storage.CanWrite) throw new ArgumentException("Output stream must support writing", (Exception)null);
        } 
        #endregion
    }
}
