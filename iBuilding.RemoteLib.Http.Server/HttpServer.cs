﻿using System;
using System.Net;

namespace RanOpt.Common.RemoteLib.Http.Server
{
    /// <summary>
    /// Http服务器
    /// </summary>
    public class HttpServer : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Default ServerCore class constructor.
        /// </summary>
        public HttpServer()
        {
            _isDisposed = false;
        }

        /// <summary>
        /// Run the ServerCore.
        /// </summary>
        public void Run()
        {
            _isDisposed = false;

            // initialize http listener
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:49152/");  // TODO: https! http://stackoverflow.com/questions/11403333/httplistener-with-https-support
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            // start listening
            listener.Start();
            PrintCopyright();

            // listener loop
            while (!_isDisposed)
            {
                var result = listener.BeginGetContext(ProcessCallback, listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Dispose the server core.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
        }

        // private
        private void ProcessCallback(IAsyncResult result)
        {
            var listener = (HttpListener)result.AsyncState;
            var context = listener.EndGetContext(result);
            var request = context.Request;
            PrintRequestInfo(request);
            // handle request
            //_processor.Process(context);
            var response = context.Response;
            ProcessHelloResponse(response);
            context.Response.Close();
        }

        private void ProcessHelloResponse(HttpListenerResponse response)
        {
            // 构造回应内容
            var responseString
                = @"<html>
                <head><title>From HttpListener Server</title></head>
                <body><h1>Hello, world.</h1></body>
            </html>";
            // 设置回应头部内容，长度，编码
            response.ContentLength64
                = System.Text.Encoding.UTF8.GetByteCount(responseString);
            response.ContentType = "text/html; charset=UTF-8";
            // 输出回应内容
            var output = response.OutputStream;
            var writer = new System.IO.StreamWriter(output);
            writer.Write(responseString);
            // 必须关闭输出流
            writer.Close();
        }

        private void PrintCopyright()
        {
            Console.WriteLine($"Zhenhua's http server (c) 2016-{DateTime.Now.Year}");
            Console.WriteLine($"Zhenhua's http server is running on {49152} port");
        }

        private void PrintRequestInfo(HttpListenerRequest request)
        {
            Console.WriteLine($"{request.HttpMethod} {request.RawUrl} HTTP/{request.ProtocolVersion}");
            //Console.WriteLine("Accept: {0}", request.AcceptTypes == null ? string.Empty : string.Join(", ", request.AcceptTypes));
            //Console.WriteLine("Accept-Language: {0}", request.UserLanguages == null ? string.Empty : string.Join(",", request.UserLanguages));
            //Console.WriteLine($"User-Agent: {request.UserAgent}");
            Console.WriteLine("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "Close");
            //Console.WriteLine("Host: {0}", request.UserHostName);
            Console.WriteLine("Client: {0}", request.RemoteEndPoint);
            foreach (var key in request.Headers.AllKeys)
            {
                Console.WriteLine($"{key}: {request.Headers[key]}");
            }
        }
    }
}