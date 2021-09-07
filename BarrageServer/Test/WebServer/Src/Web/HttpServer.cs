using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GameFramework;

namespace WebServer
{
    public class HttpServer
    {
        protected HttpListener m_HttpListener = new HttpListener();
        public bool Start(string uriPrefix)
        {
            bool pret = false;
            if(!HttpListener.IsSupported)
            {
                throw new GameFrameworkException($"使用 HttpListener 必须为 Windows XP SP2 或 Server 2003 以上系统！");
            }
            if(m_HttpListener.IsListening)
            {
                return pret;
            }
            m_HttpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            m_HttpListener.Prefixes.Add(uriPrefix);
            this.m_HttpListener.Start();
            this.m_HttpListener.BeginGetContext(this.GetContext_Async,null);

            return pret;
        }

        protected void GetContext_Async( IAsyncResult  ar)
        {
            this.m_HttpListener.BeginGetContext(this.GetContext_Async, null);//循环监听
            var context = this.m_HttpListener.EndGetContext(ar);
            var request =  context.Request;
            var response = context.Response;

            context.Response.ContentType = "text/plain;charset=UTF-8";
            context.Response.AddHeader("Content-type", "text/plain");
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.StatusCode = 200;
            string str_res = string.Empty;//返回的消息
            if (request.HttpMethod == "POST" && request.InputStream != null)
            {
                str_res = this.HandlePostRequest(request, response);
            }
            else if(request.HttpMethod == "GET" && request.InputStream != null)
            {
                str_res = this.HandleGetRequest(request, response);
            }
            else
            {
                str_res = $"没有处理的HttpMethod";
            }

            //Log.Debug(request.InputStream.);
            var bys_res = Encoding.UTF8.GetBytes(str_res);
            try
            {
                response.ContentLength64 = bys_res.Length;
                using (var stream = response.OutputStream)
                {
                    stream.Write(bys_res, 0, bys_res.Length);
                }
            }
            catch(Exception e)
            {
                Log.Error($"网络崩溃{e.ToString()}");
            }

        }

        protected string HandlePostRequest(HttpListenerRequest request,HttpListenerResponse response)
        {

            return "HandlePostRequest";
        }
        protected string HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            return "HandleGetRequest";
        }

        public void Stop()
        {
            this.m_HttpListener.Stop();
        }
    }
}
