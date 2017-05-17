using System;
using NetFwTypeLib;

namespace OwinServer
{
    public class FireWallHelper
    {
        private static INetFwMgr CreateNetFwMgr()
        {
            var typeNetFwMgr = Type.GetTypeFromProgID("HNetCfg.FwMgr");
            return Activator.CreateInstance(typeNetFwMgr) as INetFwMgr;
        }

        private static bool GloballyOpenPort(string title, int portNo,
            NET_FW_SCOPE_ scope,
            NET_FW_IP_PROTOCOL_ protocol,
            NET_FW_IP_VERSION_ ipVersion)
        {
            var netFwMgr = CreateNetFwMgr();
            var typeNetOpenPort = Type.GetTypeFromProgID("HNetCfg.FWOpenPort");
            var netOpenPort = Activator.CreateInstance(typeNetOpenPort) as INetFwOpenPort;
            if (netFwMgr == null || netOpenPort == null)
                return false;
            netOpenPort.Name = title;
            netOpenPort.Port = portNo;
            netOpenPort.Scope = scope;
            netOpenPort.Protocol = protocol;
            netOpenPort.IpVersion = ipVersion;
            try
            {
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(netOpenPort);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 打开防火墙端口
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool OpenPort(int port)
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;
            return GloballyOpenPort(appName, port,
                NET_FW_SCOPE_.NET_FW_SCOPE_ALL,
                NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP,
                NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY);
        }

        /// <summary>
        /// 判断防火墙是否已启用
        /// </summary>
        /// <returns></returns>
        public static bool IsEnabled()
        {
            var netFwMgr = CreateNetFwMgr();
            return netFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;
        }
    }
}