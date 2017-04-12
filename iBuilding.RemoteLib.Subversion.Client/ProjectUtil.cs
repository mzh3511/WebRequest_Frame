using System;
using System.IO;
using System.Xml;

namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    public static class ProjectUtil
    {
        public static bool IsProjectDir(string projectDir)
        {
            var files = Directory.GetFiles(projectDir, "*.ibp", SearchOption.TopDirectoryOnly);
            return files.Length == 1;
        }
        public static string GetPrjectId(string projectDir)
        {
            var files = Directory.GetFiles(projectDir, "*.ibp", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
                return string.Empty;
            var result = string.Empty;
            try
            {
                using (var xmlReader = XmlReader.Create(files[0]))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType != XmlNodeType.Element)
                            continue;
                        if (string.Compare(xmlReader.Name, "Project", StringComparison.OrdinalIgnoreCase) != 0)
                            continue;
                        result = xmlReader.GetAttribute("theUUID");
                        break;
                    }
                }
            }
            catch
            {
                // ignored
            }
            return result;
        }

        public static void SetPrjectId(string projectDir, string id)
        {
            var files = Directory.GetFiles(projectDir, "*.ibp", SearchOption.TopDirectoryOnly);
            if (files.Length == 0)
                return;
            try
            {
                var doc = new XmlDocument();
                doc.Load(files[0]);
                var nodeProject = doc.LastChild;
                if (nodeProject?.Attributes == null)
                    return;
                var attrId = nodeProject.Attributes["theUUID"];
                if (attrId != null)
                {
                    attrId.Value = id;
                }
                else
                {
                    attrId = doc.CreateAttribute("theUUID");
                    attrId.Value = id;
                    nodeProject.Attributes.Append(attrId);
                }
                doc.Save(files[0]);
            }
            catch
            {
                // ignored
            }
        }
    }
}