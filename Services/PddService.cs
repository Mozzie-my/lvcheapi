using System.Net;
using System.Security.Cryptography;
using System.Text;
using Wsm.Utils;
using XlcToolBox.Model;
using XlcToolBox.Utils;

namespace XlcToolBox.Services
{
    public class PddService : IPddService
    {
        private readonly string url = "";
        private readonly string client_id = "";
        private readonly string client_secret = "";
        private readonly string custom_parameters = "";
        private readonly string p_id_list = "";
        private readonly string pid = "";

        public PddService() {
            url = AppSetting.Configuration["pdd:url"];
            client_id = AppSetting.Configuration["pdd:client_id"];
            client_secret = AppSetting.Configuration["pdd:client_secret"];
            custom_parameters = AppSetting.Configuration["pdd:custom_parameters"];
            pid = AppSetting.Configuration["pdd:pid"];
        }
        public string Trans(string pddurl)
        {
            var datenow = DateTimeOffset.Now.ToUnixTimeSeconds();
            var param = new
            {
                type = "pdd.ddk.goods.zs.unit.url.gen",
                data_type = "JSON",
                client_id = client_id,
                custom_parameters = custom_parameters,
                pid = pid,
                timestamp = datenow,
                source_url = pddurl,
            };
            var signs = CalculateSign(param, client_secret);

            var signparam = new
            {
                type = "pdd.ddk.goods.zs.unit.url.gen",
                data_type = "JSON",
                client_id = client_id,
                custom_parameters = custom_parameters,
                pid = pid,
                timestamp = datenow,
                source_url = pddurl,
                sign=signs
            };
            String res = RequestHelper.GetAsync(url, signparam).Result;
            return !String.IsNullOrEmpty(res) ?res:null;

        }
        private static string CalculateSign(object parameters, string clientSecret)
        {
            // 将匿名对象转换为字典
            var parameterDictionary = parameters.GetType().GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(parameters).ToString());

            // 将参数按照字母顺序排序
            var sortedParameters = parameterDictionary.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);

            // 构建待签名的字符串
            StringBuilder signString = new StringBuilder();
            foreach (var parameter in sortedParameters)
            {
                signString.Append(parameter.Key).Append(parameter.Value);
            }

            // 在字符串头部和尾部添加客户端密钥
            signString.Insert(0, clientSecret);
            signString.Append(clientSecret);

            // 使用 MD5 算法计算签名
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(signString.ToString()));

                // 将字节数组转换为十六进制字符串
                StringBuilder result = new StringBuilder();
                foreach (byte b in bytes)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString().ToUpper(); // 转换为大写
            }
        }


    }


}
