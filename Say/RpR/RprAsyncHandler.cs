using Ninject;
using RpR.Infrastructure;
using System;
using System.Threading;
using System.Web;

namespace RpR
{
    public class RprAsyncHandler : IHttpAsyncHandler
    {
        public bool IsReusable { get { return false; } }

        #region ProcessRequest
        public void ProcessRequest(HttpContext context)
        {
            string requestTarget = context.Request.Url.AbsolutePath
                .Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            try
            {
                DependencyResolution.Kernel.Get<IRequestEngineFactory>().CreateEngine(requestTarget);
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
                // error to client
            }
        } 
        #endregion

        #region BeginProcessRequest
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback callback, object data)
        {
            ProcessRequest(context);
            AsyncResult result = new AsyncResult(callback, context, data);
            result.AsyncProcessRequest();
            return result;
        } 
        #endregion

        public void EndProcessRequest(IAsyncResult result) { }
    }

    #region AsyncResult
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

        #region AsyncResult
        public AsyncResult(AsyncCallback callback, HttpContext context, object state)
        {
            _callback = callback;
            _context = context;
            _data = state;
        }
        #endregion

        #region AsyncProcessRequest
        public void AsyncProcessRequest()
        {
            ThreadStart threadStart = new ThreadStart(StartTask);
            Thread thread = new Thread(threadStart);
            thread.Start();
        }
        #endregion

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
    #endregion
}