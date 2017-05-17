using System;
using Microsoft.Owin.Hosting;
using Owin;
using OwinServer.Middlewares;

namespace OwinServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化StartOptions参数
            StartOptions options = new StartOptions();

            //服务器Url设置
            options.Urls.Add("http://localhost:9000");
            options.Urls.Add("http://192.168.1.115:49152");

            //Server实现类库设置
            options.ServerFactory = "Microsoft.Owin.Host.HttpListener";

            //以当前的Options和Startup启动Server
            using (WebApp.Start(options, Startup))
            {
                //显示启动信息,通过ReadLine驻留当前进程
                Console.WriteLine("Owin Host/Server started,press enter to exit it...");
                Console.ReadLine();
            }//Server在Dispose中关闭
        }
        private static void Startup(IAppBuilder app)
        {
            //加载Sample Middleware
            Console.WriteLine("Sample Middleware loaded...");
            app.Use<SampleMiddleware>();
            app.Use<HelloMiddleware>();
            app.Use<DownFileMiddleware>();
            app.Use<RedirectMiddleware>();
        }
    }
}
