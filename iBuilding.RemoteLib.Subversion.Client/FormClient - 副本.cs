using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        private byte[] Single2Bytes(float val)
        {
            val = (val + 400)*10;
            var bytes = BitConverter.GetBytes((UInt16)val);
            return bytes;
        }
        private float  Bytes2Single(byte[] bytes)
        {
            var valUInt16 = BitConverter.ToUInt16(bytes,0);
            return valUInt16/10f - 400;
        }

        public static unsafe byte[] GetBytes(float value)
        {
            byte[] buffer1 = new byte[4];
            fixed (byte* numRef = buffer1)
            {
                *((float*)numRef) = value;
                var bb = buffer1[0] << 2;
            }
            
           var aa = (byte)1 << 2;
            return buffer1;
        }

        public static unsafe byte[] GetBytes(int value)
        {
            byte[] buffer1 = new byte[4];
            fixed (byte* numRef = buffer1)
            {
                *((int*)numRef) = value;
            }
            return buffer1;
        }



        private float GetRandom()
        {
            return new Random().Next(1, 9999)/10f;
        }


        private void CreateSingleFile(string filePath, int count)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            using (var stream = System.IO.File.Create(filePath))
            {
                for (var i = 0; i < count; i++)
                {
                    var bytesShort = BitConverter.GetBytes(1f);
                    stream.Write(bytesShort, 0, bytesShort.Length);
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"生成文件" + stopwatch.ElapsedMilliseconds);
        }

        private void ZipFile(string filePath)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var zipPath = Path.ChangeExtension(filePath, ".zip");
            using (var zipToOpen = new FileStream(zipPath, FileMode.Create))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Fastest);
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Zip {filePath}, {stopwatch.ElapsedMilliseconds}");
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            var file = @"G:\original.dat";
            var count = 100000000;

            CreateSingleFile(file, count);
            ZipFile(file);

            var bitConverter = new Bit16SingleConverter();

            //生成1亿个float数据的原始数据文件
            var aa = GetBytes(1f);



            //读取原始数据文件，并写入到另外一个文件中
            var stopwatchSingle = System.Diagnostics.Stopwatch.StartNew();
            using (var stream = System.IO.File.Create(@"G:\bb.dat"))
            {
                using (var srcStream = File.OpenRead(file))
                {
                    for (var i = 0; i < count; i++)
                    {
                        var buffer = new byte[4];
                        srcStream.Read(buffer, 0, buffer.Length);
                        var val = BitConverter.ToSingle(buffer,0);

                        var buffer1 = BitConverter.GetBytes(val);
                        stream.Write(buffer1, 0, buffer1.Length);
                    }
                }
            }
            stopwatchSingle.Stop();
            Console.WriteLine($"读取原始数据文件，并使用float写入到另外文件中" + stopwatchSingle.ElapsedMilliseconds);




            //读取原始数据文件，并写入到另外一个文件中
            var stopwatchBit = System.Diagnostics.Stopwatch.StartNew();
            using (var stream = File.Create(@"G:\cc.dat"))
            {
                using (var srcStream = File.OpenRead(file))
                {
                    for (var i = 0; i < count; i++)
                    {
                        var buffer = new byte[4];
                        srcStream.Read(buffer, 0, buffer.Length);
                        var val = BitConverter.ToSingle(buffer, 0);

                        var buffer1 = bitConverter.GetBytes(val);
                        stream.Write(buffer1, 0, buffer1.Length);
                    }
                }
            }
            stopwatchBit.Stop();
            Console.WriteLine($"读取原始数据文件，并使用Bit16写入到另外文件中" + stopwatchBit.ElapsedMilliseconds);


            //for (var i = 0; i < 10000000; i++)
            //{
            //    var val = new Random().Next(1,9999)/10f;
            //    var bytes = Single2Bytes(val);
            //    var newVal = Bytes2Single(bytes);
            //}

            var maxfloat = BitConverter.ToSingle(new byte[] { 0, 0, 128, 63},0);
            var maxInt = BitConverter.ToSingle(new byte[] { 255, 255, 0, 0 }, 0).ToString("00000000");
            var b1 = BitConverter.GetBytes(-1f);
            var f1 = BitConverter.ToSingle(new byte[] { 0, 0, 128, 63 }, 0);

            var b2 = BitConverter.GetBytes(2);
            var b3 = BitConverter.GetBytes('1');
            var a1 = sizeof (int);
            var a2 = sizeof (float);
            var a3 = sizeof (double);
            var a4 = sizeof (char);
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
