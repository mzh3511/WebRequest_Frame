using System.Net;

namespace RanOpt.Common.RemoteLib.Http.Client
{
    public class Class3
    {
        public void AA()
        {
            FtpWebRequest request = FtpWebRequest.Create($"ftp://127.0.01") as FtpWebRequest;
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
        }
    }
}