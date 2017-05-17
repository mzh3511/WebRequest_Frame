using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinServer.Middlewares
{
    public class SampleMiddleware : OwinMiddleware
    {
        object m_Options;
        public SampleMiddleware(OwinMiddleware next, object options)
            : base(next)
        {
            //引入参数类,并可以再类中使用
            m_Options = options;
        }
        public SampleMiddleware(OwinMiddleware next)
            : base(next)
        {
            //构造函数
        }

        public override Task Invoke(IOwinContext context)
        {
            //中间件的实现代码
            PathString tickPath = new PathString("/tick");
            //判断Request路径为/tick开头
            if (context.Request.Path.StartsWithSegments(tickPath))
            {
                string content = DateTime.Now.Ticks.ToString();
                //输出答案--当前的Tick数字
                context.Response.ContentType = "text/plain";
                context.Response.ContentLength = content.Length;
                context.Response.StatusCode = 200;
                context.Response.Expires = DateTimeOffset.Now;
                context.Response.Write(content);
                //解答者告诉Server解答已经完毕,后续Middleware不需要处理
                return Task.FromResult(0);
            }
            else
                //如果不是/tick路径,那么交付后续Middleware处理
                return Next.Invoke(context);
        }
    }
}