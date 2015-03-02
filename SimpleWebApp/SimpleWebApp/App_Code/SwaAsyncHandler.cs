using SimpleWebApp.RequestResults;
using System;
using System.Threading;
using System.Web;

namespace SimpleWebApp
{
    public class SwaAsyncHandler : IHttpAsyncHandler
    {
        public bool IsReusable { get { return false; } }
        public void ProcessRequest(HttpContext context)
        {
            string requestTarget = context.Request.Url.AbsolutePath
                .Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            RequestResult result = new DefaultRequestResult(context);
            #region Routes
            switch (requestTarget)
            {
                case "info.jhtm":
                    {
                        result = new InfoRequestResult(context);
                    }
                    break;
                case "login.jhtm":
                    {
                        result = new LoginRequestResult(context);
                    }
                    break;
                case "logout.jhtm":
                    {
                        result = new LogoutRequestResult(context);
                    }
                    break;
                case "validate.jhtm":
                    {
                        result = new ValidateRequestResult(context);
                    }
                    break;
                default:
                    {
                        result = new IndexRequestResult(context);
                    }
                    break;
            } 
            #endregion
            result.GetResponse();
        }
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object data)
        {
            ProcessRequest(context);
            AsyncResult result = new AsyncResult(callback, context, data);
            result.AsyncProcessRequest();
            return result;
        }
        public void EndProcessRequest(IAsyncResult result) { }
    }

    public class AsyncResult : IAsyncResult
    {
        private bool _comleted;
        private readonly object _data;
        private readonly AsyncCallback _callback;
        private readonly HttpContext _context;

        bool IAsyncResult.IsCompleted { get { return _comleted; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        object IAsyncResult.AsyncState { get { return _data; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }

        public AsyncResult(AsyncCallback callback, HttpContext context, object state)
        {
            _callback = callback;
            _context = context;
            _data = state;
        }

        public void AsyncProcessRequest()
        {
            ThreadStart threadStart = new ThreadStart(StartTask);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        #region StartTask
        private void StartTask()
        {
            // Thread.Sleep(60000); // try it
            LogMessage.Add(_context.Request.Headers["User-Agent"], Level.Info);
            _comleted = true;
            _callback(this);
        }
        #endregion
    }
}