using SharpSvn;

namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    public class SvnNode
    {
        public string FullPath { get; set; }
        public SvnNodeKind NodeKind { get; set; }
        public SvnStatus NodeStatus { get; set; }
    }
}