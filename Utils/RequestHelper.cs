using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace XlcToolBox.Utils
{
    /// <summary>
    /// 请求帮助类
    /// </summary>
    public class RequestHelper
    {

        public static string Post(string postData, string Url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "POST";
            req.Timeout = 3000;//设置请求超时时间，单位为毫秒 
            req.ContentType = "application/json";

            byte[] data = Encoding.UTF8.GetBytes(postData);

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);

                reqStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            StreamReader reader = new StreamReader(stream, encode);
            string content = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            return content;
        }
        public static string Get(string Url)
        {
            string content = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = "GET";
            req.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            StreamReader reader = new StreamReader(stream, encode);
            content = reader.ReadToEnd();
            stream.Close();
            reader.Close();
            return content;
        }
        public static string GetResponseByGet(string url)
        {
            string result = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                //statusCode = response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }
        /// <summary>
        /// http异步请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="reqMethod">请求方法 GET、POST</param>
        /// <param name="callback">回调函数</param>
        /// <param name="ob">回传对象</param>
        /// <param name="postData">post数据</param>
        public static void HttpAsyncRequest(string url, string reqMethod, AsyRequetCallback callback, object ob = null, string postData = "")
        {
            Stream requestStream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = reqMethod;
                if (reqMethod.ToUpper() == "POST")
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = bytes.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                //开始调用异步请求 
                //AsyResultTag 是自定义类 用于传递调用时信息 其中HttpWebRequest 是必须传递对象。
                //因为回调需要用HttpWebRequest来获取HttpWebResponse 
                request.BeginGetResponse(new AsyncCallback(HttpCallback), new AsyResultTag() { obj = ob, callback = callback, req = request });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }
        }


        /// <summary>
        /// http请求回调 由.net内部调用 参数必须为IAsyncResult
        /// </summary>
        /// <param name="asynchronousResult">http回调时回传对象</param>
        private static void HttpCallback(IAsyncResult asynchronousResult)
        {
            int statusCode = 0;
            string retString = "";
            AsyResultTag tag = new AsyResultTag();
            WebException webEx = null;
            try
            {
                //获取请求时传递的对象
                tag = asynchronousResult.AsyncState as AsyResultTag;
                HttpWebRequest req = tag.req;
                //获取异步返回的http结果
                HttpWebResponse response = req.EndGetResponse(asynchronousResult) as HttpWebResponse;
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                statusCode = (int)response.StatusCode;

            }
            catch (WebException ex)
            {
                if ((HttpWebResponse)ex.Response != null)
                {
                    statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                }

                webEx = ex;
            }
            //调用外部回调 即最外层的回调
            tag.callback(tag.obj, retString, statusCode, webEx);

        }

        /// <summary>
        /// 异步请求回调委托
        /// </summary>
        /// <param name="asyObj">回传对象</param>
        /// <param name="resStr">http响应结果</param>
        /// <param name="statusCode">http状态码</param>
        /// <param name="webEx">异常</param>
        public delegate void AsyRequetCallback(object asyObj, string respStr, int statusCode, WebException webEx);

        /// <summary>
        /// 异步返回对象
        /// </summary>
        public class AsyResultTag
        {
            /// <summary>
            /// 回传对象
            /// </summary>
            public object obj { get; set; }
            /// <summary>
            /// 当前httpRequest请求实例
            /// </summary>
            public HttpWebRequest req { get; set; }
            /// <summary>
            /// 回调函数委托
            /// </summary>
            public AsyRequetCallback callback { get; set; }
        }

        public static async  Task<string> GetAsync(string url, object queryParams)
        {
            // 构建 URL 字符串
            string fullUrl = url + ToQueryString(queryParams);

            // 创建 HttpClient 实例
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 发送 GET 请求并等待响应
                    HttpResponseMessage response = await client.GetAsync(fullUrl);

                    // 确保请求成功
                    response.EnsureSuccessStatusCode();

                    // 读取响应内容并打印
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response:");
                    Console.WriteLine(responseBody);
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request failed: {e.Message}");
                    return null;
                }
            }
        }

        // 将参数对象转换为查询字符串
        static string ToQueryString(object queryParams)
        {
            if (queryParams == null) return string.Empty;

            var properties = queryParams.GetType().GetProperties();
            if (properties.Length == 0) return string.Empty;

            var queryString = string.Join("&", properties.Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(queryParams)?.ToString())}"));
            return "?" + queryString;
        }
    }
}
