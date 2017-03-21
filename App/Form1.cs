using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RanOpt.Common.RemoteLib.Http;
using RanOpt.Common.RemoteLib.Http.Consts;

namespace App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            var http = new HttpHelper();
            //创建Httphelper参数对象
            var item = new HttpItem
            {
                Url = txtUrl.Text.Trim(),
                Method = HttpMethodConst.Get,
                ContentType = ContentTypeConst.AppFormUrlencoded,
                PostEncoding = Encoding.UTF8,
                Postdata = string.Empty
            };
            //请求的返回值对象
            var result = http.GetHtml(item);
            txtResult.AppendText($"Request Url = \r\n{item.Url}");
            txtResult.AppendText("\r\n");
            txtResult.AppendText($"Result.StatusCode = \r\n{result.StatusCode}\r\n");
            txtResult.AppendText($"Result.StatusDescription = \r\n{result.StatusDescription}\r\n");
            txtResult.AppendText("\r\n");
            txtResult.AppendText($"Result.Html = \r\n{result.Html}\r\n");
        }
    }
}
