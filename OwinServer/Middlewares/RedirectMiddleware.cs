using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinServer.Middlewares
{
    public class RedirectMiddleware : OwinMiddleware
    {
        public RedirectMiddleware(OwinMiddleware next)
            : base(next)
        {
            //构造函数
        }

        public override Task Invoke(IOwinContext context)
        {
            //中间件的实现代码
            var downloadPath = new PathString("/download");
            //判断Request路径为/tick开头
            if (!context.Request.Path.StartsWithSegments(downloadPath))
                return Next.Invoke(context);
            context.Response.Redirect(@"http://www.cnblogs.com/gaobing/p/5076089.html");
            return Task.FromResult(0);
        }
    }
}