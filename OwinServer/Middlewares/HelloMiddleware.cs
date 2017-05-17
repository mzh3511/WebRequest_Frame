using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinServer.Middlewares
{
    public class HelloMiddleware : OwinMiddleware
    {
        public HelloMiddleware(OwinMiddleware next) : base(next)
        {
            //构造函数
        }

        public override Task Invoke(IOwinContext context)
        {
            //中间件的实现代码
            var tickPath = new PathString("/hello");
            //判断Request路径为/tick开头
            if (!context.Request.Path.StartsWithSegments(tickPath))
                return Next.Invoke(context);
            var content = "Hello";
            //输出答案--当前的Tick数字
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength = content.Length;
            context.Response.StatusCode = 200;
            context.Response.Expires = DateTimeOffset.Now;
            context.Response.Write(content);
            //解答者告诉Server解答已经完毕,后续Middleware不需要处理
            return Task.FromResult(0);
        }
    }
}