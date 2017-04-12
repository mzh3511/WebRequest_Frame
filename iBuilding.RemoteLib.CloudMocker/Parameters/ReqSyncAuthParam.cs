namespace iBuilding.RemoteLib.CloudMocker.Parameters
{
    /// <summary>
    /// 请求远程同步权限的参数
    /// </summary>
    public class ReqSyncAuthParam
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProjectId { get; set; }
    }
}