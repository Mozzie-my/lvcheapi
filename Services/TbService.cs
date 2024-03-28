using System.Net;
using Wsm.Utils;
using XlcToolBox.Model;
using XlcToolBox.Utils;

namespace XlcToolBox.Services
{
    public class TbService : ITbService
    {
        private readonly string url="";
        private readonly string jaw_uid = "";
        public TbService() {
            url = AppSetting.Configuration["tb:url"];
            jaw_uid = AppSetting.Configuration["tb:jaw_uid"];
        }
        public string Trans(string token)
        {
            var param = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "content", token },
                    { "link_type", "18" },
                    { "jaw_uid", jaw_uid},
                });
            var res = sendPostAsync(url, param);
            return res.Result.ToString();
        }
        public string prasetkl(string token)
        {
            var param = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "content", token },
                { "link_type", "20" },
                { "jaw_uid",  jaw_uid},
            });
            var res = sendPostAsync("https://dtkapi.ffquan.cn/taobaoapi/pwd-analysis", param);
            return res.Result.ToString();
        }

        private async Task<string> sendPostAsync(string url,FormUrlEncodedContent param)
        {
            var clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            };
            var client = new HttpClient(clientHandler);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "authority", "dtkapi.ffquan.cn" },
                    { "accept", "application/json, text/javascript, */*; q=0.01" },
                    { "accept-language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6" },
                    { "origin", "https://www.dataoke.com" },
                    { "referer", "https://www.dataoke.com/" },
                    { "sec-ch-ua", "\\^Not_A" },
                    { "sec-ch-ua-mobile", "?0" },
                    { "sec-ch-ua-platform", "^\\^Windows^^" },
                    { "sec-fetch-dest", "empty" },
                    { "sec-fetch-mode", "cors" },
                    { "sec-fetch-site", "cross-site" },
                    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0" },
                },
                Content = param,
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                return body;
            }
        }
    }
}
