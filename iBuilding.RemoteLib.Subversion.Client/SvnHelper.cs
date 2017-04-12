using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharpSvn;

namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    public class SvnHelper : IDisposable
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public SvnClient SvnClient { get; private set; }
        public Action<string> LogAction { get; set; }

        private void Authentication_UserNamePasswordHandlers(object sender, SharpSvn.Security.SvnUserNamePasswordEventArgs e)
        {
            e.UserName = Username;
            e.Password = Password;
            e.Save = false;
        }

        public SvnHelper()
        {
            SvnClient = new SvnClient();
            SvnClient.Authentication.Clear();
            SvnClient.Authentication.UserNamePasswordHandlers -= Authentication_UserNamePasswordHandlers;
            SvnClient.Authentication.UserNamePasswordHandlers += Authentication_UserNamePasswordHandlers;
        }

        public bool LocalIsWorkingCopy(string path)
        {
            var uri = SvnClient.GetUriFromWorkingCopy(path);
            return uri != null;
        }

        public bool LocalIsDeleted(string path)
        {
            if (!LocalIsWorkingCopy(path)) return false;

            Collection<SvnStatusEventArgs> args;
            SvnClient.GetStatus(path, out args);
            return args.Count > 0 && args[0].LocalContentStatus == SvnStatus.Deleted;
        }

        public void FetchDirectory(string dir, List<SvnNode> nodes)
        {
            var workingRoot = SvnClient.GetWorkingCopyRoot(dir);
            if (workingRoot == null)
            {
                FetchNotVersionedDir(dir, nodes);
            }
            else
            {
                FetchWorkingCopyDir(dir, nodes);
            }
            PrintSvnNodes(nodes);
        }

        private void FetchWorkingCopyDir(string path, List<SvnNode> nodes)
        {
            var statusArgs = new SvnStatusArgs()
            {
                Depth = SvnDepth.Children,
                RetrieveAllEntries = true,
                ThrowOnError = false
            };
            Collection<SvnStatusEventArgs> list;
            if (!SvnClient.GetStatus(path, statusArgs, out list))
                return;
            for (var i = 1; i < list.Count; i++)
            {
                var argse = list[i];
                if (argse.Versioned)
                {
                    nodes.Add(new SvnNode { FullPath = argse.FullPath, NodeKind = argse.NodeKind, NodeStatus = argse.LocalNodeStatus });
                    if (argse.NodeKind == SvnNodeKind.Directory)
                        FetchWorkingCopyDir(argse.FullPath, nodes);
                }
                else
                {
                    var nodeKind = File.Exists(argse.FullPath) ? SvnNodeKind.File : SvnNodeKind.Directory;
                    nodes.Add(new SvnNode { FullPath = argse.FullPath, NodeKind = nodeKind, NodeStatus = argse.LocalNodeStatus });
                    if (nodeKind == SvnNodeKind.Directory)
                        FetchNotVersionedDir(argse.FullPath, nodes);
                }
            }
        }

        private void FetchNotVersionedDir(string path, List<SvnNode> nodes)
        {
            var files = Directory.GetFiles(path);
            nodes.AddRange(files.Select(file => new SvnNode { FullPath = file, NodeKind = SvnNodeKind.File, NodeStatus = SvnStatus.NotVersioned }));
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                nodes.Add(new SvnNode { FullPath = dir, NodeKind = SvnNodeKind.Directory, NodeStatus = SvnStatus.NotVersioned });
                FetchNotVersionedDir(dir, nodes);
            }
        }

        private void PrintSvnNodes(List<SvnNode> nodes)
        {
            if (nodes == null || nodes.Count == 0)
                return;
            var nodeKindMaxBytes = nodes.Max(cond => cond.NodeKind.ToString().Length);
            var nodeStatusMaxBytes = nodes.Max(cond => cond.NodeStatus.ToString().Length);
            foreach (var node in nodes)
            {
                var message = $"{node.NodeKind.ToString().PadLeft(nodeKindMaxBytes)} {node.NodeStatus.ToString().PadLeft(nodeStatusMaxBytes)} {node.FullPath}";
                PrintLog(message);
            }
        }

        public void PrintLog(string message)
        {
            Console.WriteLine(message);
            LogAction?.Invoke(message);
        }

        public bool Commit(SvnUriTarget remoteTarget, SvnPathTarget localTarget)
        {
            var svnResult = false;
            if (!RemoteIsExist(remoteTarget))
            {
                svnResult = RemoteCreateDirectory(remoteTarget);
                if (!svnResult)
                    return false;
            }
            if (string.IsNullOrEmpty(SvnClient.GetWorkingCopyRoot(localTarget.FullPath)))
            {
                SvnUpdateResult updateResult;
                svnResult = SvnClient.CheckOut(remoteTarget.Uri, localTarget.FullPath,
                    new SvnCheckOutArgs() { Depth = SvnDepth.Children }, out updateResult);
                PrintLog($"CheckOut, result={svnResult}, revision = {updateResult?.Revision}, remote = {remoteTarget.Uri}, local = {localTarget.FullPath}");
                if (!svnResult)
                    return false;
            }
            else
            {
                //SvnUpdateResult updateResult;
                //svnResult = SvnClient.Update(localTarget.FullPath,out updateResult);
                //if (!svnResult)
                //    return false;
            }
            var nodes = new List<SvnNode>();
            FetchDirectory(localTarget.FullPath, nodes);
            foreach (var node in nodes)
            {
                switch (node.NodeStatus)
                {
                    case SvnStatus.Missing:
                        SvnClient.Delete(node.FullPath);
                        break;
                    case SvnStatus.NotVersioned:
                        SvnClient.Add(node.FullPath, SvnDepth.Empty);
                        break;
                }
            }
            var args = new SvnCommitArgs
            {
                LogMessage = $"Commit by {Application.ProductName}, in {DateTime.Now.ToString("G")}"
            };
            SvnCommitResult result1;
            svnResult = SvnClient.Commit(localTarget.FullPath, args, out result1);
            PrintLog($"Commit, result = {svnResult}, revision = {result1?.Revision}");
            return svnResult;
        }

        public bool RemoteIsExist(SvnUriTarget remoteTarget)
        {
            Collection<SvnInfoEventArgs> info;
            var result = SvnClient.GetInfo(remoteTarget, new SvnInfoArgs { ThrowOnError = false }, out info);
            PrintLog($"Remote check exist, result = {result}, uri = {remoteTarget.Uri}");
            return result;
        }

        public bool RemoteCreateDirectory(SvnUriTarget remoteTarget)
        {
            var createDirArg = new SvnCreateDirectoryArgs()
            {
                LogMessage = $"Create directory by {Application.ProductName} in {DateTime.Now.ToString("G")}"
            };
            var result = SvnClient.RemoteCreateDirectory(remoteTarget.Uri, createDirArg);
            PrintLog($"Remote create directory, result = {result}, uri = {remoteTarget.Uri}");
            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            SvnClient.Authentication.Clear();
            SvnClient.Authentication.UserNamePasswordHandlers -= Authentication_UserNamePasswordHandlers;
            SvnClient.Dispose();
            SvnClient = null;
        }
    }
}