using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSvn;

namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    class SvnCommitReporter : SvnClientReporter
    {
        private int _progressPadTotalWidth;
        public int Progress { get; private set; }
        public int TotalProgress { get; private set; }
        public Action<string> LogAction { get; set; }

        public SvnCommitReporter(SvnClientArgs clientArgs, StringBuilder sb) :
            base(clientArgs, sb)
        {
        }

        public void InitializeBySvnNodes(List<SvnNode> nodes)
        {
            var fileCount = nodes.Count(cond => cond.NodeKind == SvnNodeKind.File);
            TotalProgress = nodes.Count + fileCount + 1;
            Progress = 0;
            SuppressFinalLine = false;

            _progressPadTotalWidth = TotalProgress.ToString().Length;
        }


        protected override void OnNotify(SvnNotifyEventArgs e)
        {
            base.OnNotify(e);
            ++Progress;
            LogAction?.Invoke($"{Progress.ToString().PadLeft(_progressPadTotalWidth)}/{TotalProgress} {e.Action} {e.FullPath}");
        }
    }
}