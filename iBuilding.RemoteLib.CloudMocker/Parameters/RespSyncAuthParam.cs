namespace iBuilding.RemoteLib.CloudMocker.Parameters
{
    /// <summary>
    /// 响应远程同步权限的参数
    /// </summary>
    public class RespSyncAuthParam
    {
        public string ProjectId { get; set; }
        public string SyncServerUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}