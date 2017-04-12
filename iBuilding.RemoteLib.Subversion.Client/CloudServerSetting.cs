
namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    public static class CloudServerSetting
    {
        public static string ServerUrl = @"http://192.168.1.115:49152";
        public static string ServerUsername = "rod";
        public static string ServerPassword = "Admin@123";

        public static bool UseProxy = false;
        public static string ProxyUrl = string.Empty;
        public static string ProxyUsername = "rod";
        public static string ProxyPassword = "Admin@123";

        public static string GetReqSvnUrl()
        {
            return new System.Uri(new System.Uri(ServerUrl), "GetSvn").AbsoluteUri;
        }
    }
}