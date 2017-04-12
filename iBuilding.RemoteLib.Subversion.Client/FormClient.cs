using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using RanOpt.Common.RemoteLib.Http.Client;
using RanOpt.Common.RemoteLib.Http.Client.Consts;
using RanOpt.Common.RemoteLib.Subversion.Client.Parameters;
using SharpSvn;

namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    public partial class FormClient : Form
    {
        public FormClient()
        {
            InitializeComponent();
        }

        private void btnCheckDiff_Click(object sender, EventArgs e)
        {
            using (var helper = new SvnHelper())
            {
                helper.LogAction = str => txtResult.AppendText(str + "\r\n");
                var localTarget = SvnPathTarget.FromString(txtLocalFolder.Text.Trim());
                var nodes = new List<SvnNode>();
                helper.FetchDirectory(localTarget.FullPath, nodes);
                helper.PrintLog($"Total {nodes.Count}");
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            using (var helper = new SvnHelper())
            {
                helper.LogAction = str => txtResult.AppendText(str + "\r\n");

                var projectDir = txtLocalFolder.Text.Trim();
                if (!ProjectUtil.IsProjectDir(projectDir))
                {
                    helper.PrintLog($"{projectDir}不是一个有效的项目文件夹");
                    return;
                }

                var projectId = ProjectUtil.GetPrjectId(projectDir);
                var http = new HttpHelper();
                var item = new HttpItem
                {
                    Url = CloudServerSetting.GetReqSvnUrl(),
                    Method = WebRequestMethods.Http.Post,
                    ContentType = ContentTypeConst.AppFormUrlencoded,
                    PostEncoding = Encoding.UTF8,
                    Postdata = JsonConvert.SerializeObject(new ReqSyncAuthParam { Username = CloudServerSetting.ServerUsername, Password = CloudServerSetting.ServerPassword, ProjectId = projectId })// $"Username={CloudServerSetting.ServerUsername}&Password={CloudServerSetting.ServerPassword}&ProjectId={projectId}"
                };
                helper.PrintLog($"Request {item.Url}?{item.Postdata}");

                var reqSvnResult = http.GetHtml(item);
                if (reqSvnResult.StatusCode != HttpStatusCode.OK)
                {
                    helper.PrintLog($"Request failed, detail = {reqSvnResult.Html}");
                    return;
                }

                var remoteAuth = JsonConvert.DeserializeObject<RespSyncAuthParam>(reqSvnResult.Html);
                if (string.IsNullOrEmpty(remoteAuth?.SyncServerUrl))
                {
                    helper.PrintLog($"Request got a wrong auth, detail = {reqSvnResult.Html}");
                    return;
                }

                helper.PrintLog($"Request return svn auth {remoteAuth.SyncServerUrl} {remoteAuth.Username} {remoteAuth.Password}");
                helper.Username = remoteAuth.Username;
                helper.Password = remoteAuth.Password;
                ProjectUtil.SetPrjectId(projectDir, remoteAuth.ProjectId);
                var remoteTarget = SvnUriTarget.FromString(remoteAuth.SyncServerUrl);
                var localTarget = SvnPathTarget.FromString(txtLocalFolder.Text.Trim());
                var result = helper.Commit(remoteTarget, localTarget);
            }
        }

        private void btnTestServer_Click(object sender, EventArgs e)
        {
            var http = new HttpHelper();
            //创建Httphelper参数对象
            var item = new HttpItem
            {
                Url = txtServerURL.Text.Trim(),
                Method = WebRequestMethods.Http.Get,
                ContentType = ContentTypeConst.AppFormUrlencoded,
                PostEncoding = Encoding.UTF8,
                Postdata = string.Empty
            };
            var result = http.GetHtml(item);
            MessageBox.Show($"Test server, result = {result.StatusCode}");
        }

        private void ApplyServerSetting()
        {
            CloudServerSetting.ServerUrl = txtServerURL.Text.Trim();
            CloudServerSetting.ServerUsername = txtServerPassword.Text.Trim();
            CloudServerSetting.ServerPassword = txtServerPassword.Text.Trim();
            CloudServerSetting.UseProxy = chkProxyUseIt.Checked;
            CloudServerSetting.ProxyUrl = txtProxyURL.Text.Trim();
            CloudServerSetting.ProxyUsername = txtProxyUsername.Text.Trim();
            CloudServerSetting.ProxyPassword = txtProxyPassword.Text.Trim();
        }

        private void btnApplySetting_Click(object sender, EventArgs e)
        {
            ApplyServerSetting();
        }
    }
}
