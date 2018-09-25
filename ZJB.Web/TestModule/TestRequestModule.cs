using System;
using System.Web;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ZJB.Web.TestModule
{
    /// <summary>
    /// 请求队列探针
    /// </summary>
    public class TestRequestModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += BeginRequest;
            application.EndRequest += EndRequest;
            application.Error += Error;
        }


        private static ConcurrentDictionary<int,ThreadRequestInfo> Requests = new ConcurrentDictionary<int, ThreadRequestInfo>();
        void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application != null && application.Context!=null)
            {
                var request = application.Context.Request;
                var response = application.Context.Response;
                if (request!=null && request.Path.Contains("TestRequest"))
                {
                    if (request["kill"] == "all")
                    {
                        KillAllThread();
                    }
                    TestRequestResponce(response);
                    response.End();
                    return;
                }
                var thread = System.Threading.Thread.CurrentThread;
                var process = System.Diagnostics.Process.GetCurrentProcess();

                ThreadRequestInfo lastRequest = null;
                Requests.TryGetValue(thread.ManagedThreadId,out lastRequest);
                if (lastRequest == null)
                {
                    lastRequest = new ThreadRequestInfo()
                    {
                        ThreadId = thread.ManagedThreadId,
                        LastUrl = request.RawUrl,
                        Start = DateTime.Now,
                        End = DateTime.Now,
                        LastChange = DateTime.Now,
                        RepeatCount=0,
                        UsedMSecond = 0,
                        UsedProcessTime= process.UserProcessorTime.TotalMilliseconds,
                        State="Begin",
                    };
                    Requests.TryAdd(thread.ManagedThreadId,lastRequest);
                }
                else
                {
                    lastRequest.ThreadId = thread.ManagedThreadId;
                    lastRequest.LastUrl = request.RawUrl;
                    lastRequest.LastUrl = request.RawUrl;
                    lastRequest.Start = DateTime.Now;
                    lastRequest.End = DateTime.Now;
                    lastRequest.LastChange = DateTime.Now;
                    lastRequest.RepeatCount++;
                    lastRequest.UsedMSecond = 0;
                    lastRequest.UsedProcessTime = process.UserProcessorTime.TotalMilliseconds;
                    lastRequest.State = "Begin";
                }
            }
        }

        void EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application != null)
            {
                var request = application.Context.Request;
                var response = application.Context.Response;
                var process=System.Diagnostics.Process.GetCurrentProcess();
                var thread = System.Threading.Thread.CurrentThread;
                ThreadRequestInfo lastRequest = null;
                Requests.TryGetValue(thread.ManagedThreadId, out lastRequest);
                if (lastRequest != null)
                {
                    lastRequest.ThreadId = thread.ManagedThreadId;
                    lastRequest.End = DateTime.Now;
                    lastRequest.UsedMSecond = (lastRequest.End - lastRequest.Start).TotalMilliseconds;
                    lastRequest.UsedProcessTime = (process.UserProcessorTime.TotalMilliseconds - lastRequest.UsedProcessTime);
                    lastRequest.UsedProcessTime2 = process.Threads.Cast<ProcessThread>().Where(p=>p.Id==thread.ManagedThreadId).Select(p=>p.PrivilegedProcessorTime.TotalMilliseconds).FirstOrDefault();
                    lastRequest.State = "End";
                    Requests.TryRemove(thread.ManagedThreadId,out lastRequest);
                }
            }
        }

        void Error(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            try
            {

            }
            catch
            {
                //do nothing
            }
        }
        private void KillAllThread()
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            var requestThreads = process.Threads;
            foreach (ProcessThread item in requestThreads)
            {
               
            }
        }
        public static int GetNativeThreadId(Thread thread)
        {
            var thread2 = System.Threading.Thread.CurrentThread;
            if (thread2.ManagedThreadId == thread2.ManagedThreadId)
            {
                return System.AppDomain.GetCurrentThreadId();
            }
            var f = typeof(Thread).GetField("DONT_USE_InternalThread",
             BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
            var pInternalThread = (IntPtr)f.GetValue(thread);
            var nativeId = Marshal.ReadInt32(pInternalThread, (IntPtr.Size == 8) ? 548 : 348); // found by analyzing the memory
            return nativeId;
        }

        private void TestRequestResponce(HttpResponse response)
        {
            response.Clear();
            response.ContentType = "application/json";
            var reqs = Requests.Select(p=>p.Value).OrderBy(p=>p.Start).ToList();
            var process=System.Diagnostics.Process.GetCurrentProcess();
            var requestThreads=process.Threads.Cast<ProcessThread>().Where(p => Requests.ContainsKey(p.Id)).ToList();
            var result = new
            {
                usedProcessorTime = process.PrivilegedProcessorTime,
                usedMemorySize= process.PrivateMemorySize64,
                threadCount = process.Threads.Count,
                requestThreadCount = requestThreads.Count,
                requestCount = reqs.Count,
                requests = reqs,
            };
            response.Write(JsonConvert.SerializeObject(result));
            response.End();
        }

        public class ThreadRequestInfo
        {
            public int ThreadId { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public DateTime LastChange { get; set; }
            public string LastUrl { get; set; }
            public string State { get; set; }
            public double UsedMSecond { get; set; }
            public double UsedProcessTime { get; set; }
            public double UsedProcessTime2 { get; set; }
            public int UserMem { get; set; }
            public int RepeatCount { get; set; }
        }

    }
}
