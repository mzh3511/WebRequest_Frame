using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinServer.Middlewares
{
    public class DownFileMiddleware : OwinMiddleware
    {
        public DownFileMiddleware(OwinMiddleware next)
            : base(next)
        {
            //构造函数
        }

        public override Task Invoke(IOwinContext context)
        {
            //中间件的实现代码
            var path = new PathString("/download");
            //判断Request路径为/tick开头
            if (!context.Request.Path.StartsWithSegments(path))
                return Next.Invoke(context);

            var filePath = @"E:\workGit\ExpCADForm.cs";
            var fileName = Path.GetFileName(filePath);
            var fileLength = new FileInfo(filePath).Length;
            context.Response.Headers.Append("Content-Disposition", "attachment;filename=" + fileName);//设置文件名
            context.Response.Headers.Append("Content-Length", fileLength.ToString());//设置下载文件大小
            context.Response.ContentType = "application/octet-stream";//设置文件类型
            //读取文件数据
            using (var fileStream = File.OpenRead(filePath))
            {
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                context.Response.Write(buffer);
            }
            //解答者告诉Server解答已经完毕,后续Middleware不需要处理
            return Task.FromResult(0);
        }
    }
}