#region using
using Ninject;
using RpR.Infrastructure;
using RpR.ResponseEngines.Infrastructure;
using System;
using System.Net;
using System.Threading;
using System.Web; 
#endregion

namespace RpR
{
    public class RprAsyncHandler : IHttpAsyncHandler
    {
        #region Fields&Props
        public bool IsReusable { get { return false; } } 
        #endregion

        #region ProcessRequest
        public void ProcessRequest(HttpContext context)
        {
            string requestTarget = context.Request.Url.AbsolutePath
                .Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            try
            {
                DependencyResolution.Kernel.Get<IRequestEngineFactory>().CreateRequestEngine(requestTarget);
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
                Error();
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

        #region EndProcessRequest
        public void EndProcessRequest(IAsyncResult result) { } 
        #endregion

        #region Error
        private void Error()
        {
            IResponse response = new Response();
            response.StatusCode = HttpStatusCode.InternalServerError;
            try
            {
                string result = new ResponseStrategies().GetByRoutes("error.rpr", (string)null);
                response.Send(result);
            }
            catch
            {
                LogMessage.Add("Page error.rpr doesn't exist", Level.Error);
                response.Send("Internal Server Error...");
            }
        } 
        #endregion
    }

    #region AsyncResult
    public class AsyncResult : IAsyncResult
    {
        #region Fields&Props
        private bool _comleted;
        private readonly object _data;
        private readonly AsyncCallback _callback;
        private readonly HttpContext _context;
        bool IAsyncResult.IsCompleted { get { return _comleted; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        object IAsyncResult.AsyncState { get { return _data; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } } 
        #endregion

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