#region using
using System.Net; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public interface IResponse
    {
        string ContentType { get; set; }
        HttpStatusCode StatusCode { get; set; } 
        void Send(string response);
        void Send(byte[] response);
        void Redirect(string url);
    }
}
