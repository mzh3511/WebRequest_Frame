using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace iBuilding.RemoteLib.CloudMocker
{
    public class Settings
    {
        private static readonly Settings _default;

        private static readonly string _filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Settings.json");

        public static Settings Default
        {
            get
            {
                if (_default == null)
                {
                    //var jsonString = File.ReadAllText()
                    //_default = JsonConvert.DeserializeObject<Settings>()
                }
                return _default;
            }
        }

        public string SyncUrl { get; set; }
        public string SyncUsername { get; set; }
        public string SyncPassword { get; set; }
    }
}