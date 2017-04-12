using System.Net;
using System.Text;
using System.Windows.Forms;
using RanOpt.Common.RemoteLib.Http.Client;
using RanOpt.Common.RemoteLib.Sync.Client.Core;

namespace RanOpt.Common.RemoteLib.Sync.Client
{
    public partial class FormClient : Form
    {
        public FormClient()
        {
            InitializeComponent();
        }

        private void btnCheckDiff_Click(object sender, System.EventArgs e)
        {
            var project = Project.Create(txtLocalFolder.Text.Trim(), "aa");
            var diffList = project.BuildDiff();
            txtDiff.Clear();
            foreach (var diff in diffList)
            {
                txtDiff.AppendText($"{diff.DiffType}   {diff.FileName}");
            }
        }

        private void btnSync_Click(object sender, System.EventArgs e)
        {
            var project = Project.Create(txtLocalFolder.Text.Trim(), "aa");
            var diffList = project.BuildDiff();

        }

        private void btnTestServer_Click(object sender, System.EventArgs e)
        {
            var http = new HttpHelper();
            var item = CreateHttpItem();
            var result = http.GetHtml(item);
            MessageBox.Show(this, result.Html, Text, MessageBoxButtons.OK);
        }

        private HttpItem CreateHttpItem()
        {
            var item = new HttpItem
            {
                Url = txtServerURL.Text.Trim(),
                Method = WebRequestMethods.Http.Post,
                PostEncoding = Encoding.UTF8,
                Postdata = $"Username={txtServerUsername.Text.Trim()}&Password={txtProxyPassword.Text.Trim()}"
            };
            if (chkProxyUseIt.Checked)
            {
                var proxy = new WebProxy
                {
                    Address = new System.UriBuilder(txtProxyURL.Text.Trim()).Uri,
                    Credentials = new NetworkCredential(txtProxyUsername.Text.Trim(), txtProxyPassword.Text.Trim())
                };
                item.WebProxy = proxy;
            }
            return item;
        }
    }
}
