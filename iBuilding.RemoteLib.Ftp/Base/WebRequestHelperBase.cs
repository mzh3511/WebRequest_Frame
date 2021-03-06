﻿using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using RanOpt.Common.RemoteLib.Http.Consts;
using RanOpt.Common.RemoteLib.Http.Enum;

namespace RanOpt.Common.RemoteLib.Http.Base
{
    public class WebRequestHelperBase
    {
        #region 预定义方变量
        //默认的编码
        protected Encoding Encoding = Encoding.Default;
        //Post数据编码
        protected Encoding Postencoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private WebRequest _request;
        //获取影响流的数据对象
        private WebResponse _response;
        #endregion

        #region internal
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        internal HttpResult GetHtml(HttpItem item)
        {
            //返回参数
            var result = new HttpResult();
            try
            {
                //准备参数
                SetRequest(item);
            }
            catch (Exception ex)
            {
                //配置参数时出错
                return new HttpResult { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "配置参数时出错：" + ex.Message };
            }
            try
            {
                //请求数据
                using (_response = (HttpWebResponse)_request.GetResponse())
                {
                    GetData(item, result);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (_response = (HttpWebResponse)ex.Response)
                    {
                        GetData(item, result);
                    }
                }
                else
                {
                    result.Html = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            if (item.IsToLower)
                result.Html = result.Html.ToLower();
            return result;
        }

        /// <summary>
        /// 快速Post数据这个访求与GetHtml一样，只是不接收返回数据，只做提交。
        /// </summary>
        /// <param name="item">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        internal HttpResult FastRequest(HttpItem item)
        {
            //返回参数
            var result = new HttpResult();
            try
            {
                //准备参数
                SetRequest(item);
            }
            catch (Exception ex)
            {
                //配置参数时出错
                return new HttpResult { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "配置参数时出错：" + ex.Message };
            }
            try
            {
                //请求数据
                using (_response = (HttpWebResponse)_request.GetResponse())
                {
                    //成功 不做处理只回成功状态
                    return new HttpResult { Cookie = string.Empty, Header = _response.Headers, StatusCode = _response.StatusCode, StatusDescription = _response.StatusDescription };
                }
            }
            catch (WebException ex)
            {
                using (_response = (HttpWebResponse)ex.Response)
                {
                    //不做处理只回成功状态
                    return new HttpResult { Cookie = string.Empty, Header = _response.Headers, StatusCode = _response.StatusCode, StatusDescription = _response.StatusDescription };
                }
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            if (item.IsToLower)
                result.Html = result.Html.ToLower();
            return result;
        }
        #endregion

        #region GetData
        /// <summary>
        /// 获取数据的并解析的方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        private void GetData(HttpItem item, HttpResult result)
        {
            if (_response == null)
            {
                return;
            }

            #region base
            //获取StatusCode
            result.StatusCode = _response.StatusCode;
            //获取最后访问的URl
            result.ResponseUri = _response.ResponseUri.ToString();
            //获取StatusDescription
            result.StatusDescription = _response.StatusDescription;
            //获取Headers
            result.Header = _response.Headers;
            //获取CookieCollection
            if (_response.Cookies != null)
                result.CookieCollection = _response.Cookies;
            //获取set-cookie
            if (_response.Headers["set-cookie"] != null)
                result.Cookie = _response.Headers["set-cookie"];
            #endregion

            #region byte
            //处理网页Byte
            var responseByte = GetByte();
            #endregion

            #region Html
            if (responseByte != null && responseByte.Length > 0)
            {
                //设置编码
                SetEncoding(item, result, responseByte);
                //得到返回的HTML
                result.Html = Encoding.GetString(responseByte);
            }
            else
            {
                //没有返回任何Html代码
                result.Html = string.Empty;
            }
            #endregion
        }

        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="item">HttpItem</param>
        /// <param name="result">HttpResult</param>
        /// <param name="responseByte">byte[]</param>
        private void SetEncoding(HttpItem item, HttpResult result, byte[] responseByte)
        {
            //是否返回Byte类型数据
            if (item.ResultType == ResultType.Byte)
                result.ResultByte = responseByte;
            //从这里开始我们要无视编码了
            if (Encoding == null)
            {
                var meta = Regex.Match(Encoding.Default.GetString(responseByte), RegexString.Enconding, RegexOptions.IgnoreCase);
                var c = string.Empty;
                if (meta.Groups.Count > 0)
                {
                    c = meta.Groups[1].Value.ToLower().Trim();
                }
                if (c.Length > 2)
                {
                    try
                    {
                        Encoding = Encoding.GetEncoding(c.Replace("\"", string.Empty).Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                    }
                    catch
                    {
                        Encoding = string.IsNullOrEmpty(_response.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(_response.CharacterSet);
                    }
                }
                else
                {
                    Encoding = string.IsNullOrEmpty(_response.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(_response.CharacterSet);
                }
            }
        }

        /// <summary>
        /// 提取网页Byte
        /// </summary>
        /// <returns></returns>
        private byte[] GetByte()
        {
            if (_response == null)
                return null;
            byte[] responseByte;
            using (var resStream = _response.GetResponseStream())
            {
                if (resStream == null)
                    return null;
                using (var stream = new MemoryStream())
                {
                    //GZIIP处理
                    if (_response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        new GZipStream(resStream, CompressionMode.Decompress).CopyTo(stream, 10240);
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        resStream.CopyTo(stream, 10240);
                    }
                    //获取Byte
                    responseByte = stream.ToArray();
                }
            }
            return responseByte;
        }
        #endregion

        #region SetRequest
        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="item">参数列表</param>
        private void SetRequest(HttpItem item)
        {
            //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            //初始化对像，并设置请求的URL地址
            _request = (HttpWebRequest)WebRequest.Create(item.Url);

            // 验证证书
            SetCer(item);
            SetCerList(item);
            //设置Header参数
            if (item.Header != null && item.Header.Count > 0)
                foreach (var key in item.Header.AllKeys)
                {
                    _request.Headers.Add(key, item.Header[key]);
                }
            // 设置代理
            SetProxy(item);
            if (item.ProtocolVersion != null)
                _request.ProtocolVersion = item.ProtocolVersion;
            _request.ServicePoint.Expect100Continue = item.Expect100Continue;
            //请求方式Get或者Post
            _request.Method = item.Method;
            _request.Timeout = item.Timeout;
            _request.KeepAlive = item.KeepAlive;
            _request.ReadWriteTimeout = item.ReadWriteTimeout;
            if (!string.IsNullOrWhiteSpace(item.Host))
            {
                _request.Host = item.Host;
            }
            if (item.IfModifiedSince != null)
                _request.IfModifiedSince = Convert.ToDateTime(item.IfModifiedSince);
            //Accept
            _request.Accept = item.Accept;
            //ContentType返回类型
            _request.ContentType = item.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            _request.UserAgent = item.UserAgent;
            // 编码
            Encoding = item.Encoding;
            //设置安全凭证
            _request.Credentials = item.Credentials;
            //设置Cookie
            SetCookie(item);
            //来源地址
            _request.Referer = item.Referer;
            //是否执行跳转功能
            _request.AllowAutoRedirect = item.Allowautoredirect;
            if (item.MaximumAutomaticRedirections > 0)
            {
                _request.MaximumAutomaticRedirections = item.MaximumAutomaticRedirections;
            }
            //设置Post数据
            SetPostData(item);
            //设置最大连接
            if (item.Connectionlimit > 0)
                _request.ServicePoint.ConnectionLimit = item.Connectionlimit;
        }

        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCer(HttpItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.CerPath))
            {
                //将证书添加到请求里
                _request.ClientCertificates.Add(!string.IsNullOrWhiteSpace(item.CerPwd) ? new X509Certificate(item.CerPath, item.CerPwd) : new X509Certificate(item.CerPath));
            }
        }

        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpItem item)
        {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0)
            {
                foreach (var c in item.ClentCertificates)
                {
                    _request.ClientCertificates.Add(c);
                }
            }
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetCookie(HttpItem item)
        {
            if (!string.IsNullOrEmpty(item.Cookie))
                _request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            //设置CookieCollection
            if (item.ResultCookieType == ResultCookieType.CookieCollection)
            {
                _request.CookieContainer = new CookieContainer();
                if (item.CookieCollection != null && item.CookieCollection.Count > 0)
                    _request.CookieContainer.Add(item.CookieCollection);
            }
        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="item">Http参数</param>
        private void SetPostData(HttpItem item)
        {
            //验证在得到结果时是否有传入数据
            if (!_request.Method.Trim().ToLower().Contains("get"))
            {
                if (item.PostEncoding != null)
                {
                    Postencoding = item.PostEncoding;
                }
                byte[] buffer = null;
                //写入Byte类型
                if (item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0)
                {
                    //验证在得到结果时是否有传入数据
                    buffer = item.PostdataByte;
                } //写入文件
                else if (item.PostDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(item.Postdata))
                {
                    var r = new StreamReader(item.Postdata, Postencoding);
                    buffer = Postencoding.GetBytes(r.ReadToEnd());
                    r.Close();
                } //写入字符串
                else if (!string.IsNullOrWhiteSpace(item.Postdata))
                {
                    buffer = Postencoding.GetBytes(item.Postdata);
                }
                if (buffer != null)
                {
                    _request.ContentLength = buffer.Length;
                    _request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
            }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="item">参数对象</param>
        private void SetProxy(HttpItem item)
        {
            var isIeProxy = false;
            if (!string.IsNullOrWhiteSpace(item.ProxyIp))
            {
                isIeProxy = item.ProxyIp.ToLower().Contains("ieproxy");
            }
            if (!string.IsNullOrWhiteSpace(item.ProxyIp) && !isIeProxy)
            {
                //设置代理服务器
                if (item.ProxyIp.Contains(":"))
                {
                    var plist = item.ProxyIp.Split(':');
                    var myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()))
                    {
                        Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd)
                    };
                    //建议连接
                    //给当前请求对象
                    _request.Proxy = myProxy;
                }
                else
                {
                    var myProxy = new WebProxy(item.ProxyIp, false)
                    {
                        Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd)
                    };
                    //建议连接
                    //给当前请求对象
                    _request.Proxy = myProxy;
                }
            }
            else if (isIeProxy)
            {
                //设置为IE代理
            }
            else
            {
                _request.Proxy = item.WebProxy;
            }
        }
        #endregion

        #region private main
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        #endregion 
    }
}