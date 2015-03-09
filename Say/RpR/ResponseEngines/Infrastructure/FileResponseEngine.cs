#region using
using RpR.RequestEngines.Infrastructure;
using System; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public class FileResponseEngine : ResponseEngine
    {
        #region Fields&Props
        public string ContentType { get; private set; }
        public byte[] File { get; private set; } 
        #endregion

        #region .ctors
        public FileResponseEngine(byte[] file, string contentType, RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
            : base(requestEngine, response, responseStrategies)
        {
            if ((file == null) || (file.Length == 0))
                throw new ArgumentNullException("File is null or empty", (Exception)null);
            if (String.IsNullOrEmpty(contentType) || String.IsNullOrWhiteSpace(contentType))
                throw new ArgumentNullException("Content type is null or empty", (Exception)null);

            ContentType = contentType;
            Response.ContentType = ContentType;
            File = file;
        } 
        #endregion

        #region GetResponse
        public override void GetResponse()
        {
            if (HttpContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            Response.Send(File);
        } 
        #endregion
    }
}