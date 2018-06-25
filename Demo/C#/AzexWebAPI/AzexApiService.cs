using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AzexWebAPI
{
    public class AzexApiService
    {
        const string API_DOMAIN = "https://openapi.azex.io";
        const string API_CURRENCY = "/Market/GetCurrencys";
        const string API_KLINE = "/Market/GetKline";
        const string API_BALANCE = "/Account/Balance";
        /// <summary>
        /// 公钥
        /// </summary>
        private string API_KEY = "";
        /// <summary>
        /// 密钥
        /// </summary>
        private string API_SECRET = "";


        private static HttpClient client = new HttpClient();


        public AzexApiService(string apikey, string apisecret)
        {
            
            this.API_KEY = apikey;
            this.API_SECRET = apisecret;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OPENAPI", apikey);
        }
        /// <summary>
        /// 获取平台所有币种
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetCurrencies()
        {
            var url = $"{API_DOMAIN + API_CURRENCY}";
            var responseStr = await GetAsync(url);
            var result = new List<string>();
            if (!string.IsNullOrEmpty(responseStr))
            {
                foreach (var item in ((dynamic)JsonConvert.DeserializeObject(responseStr)).value)
                {
                    result.Add((string)item);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取个人资产
        /// </summary>
        /// <param name="currency">币种</param>
        /// <returns></returns>
        public async Task<string> GetBalance(List<string> currency)
        {
            var url = $"{API_DOMAIN + API_BALANCE}";
            var parms = new Dictionary<string, string>();
            var parmstr = new StringBuilder();
            parms.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            if (currency != null && currency.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in currency)
                {
                    builder.Append(item + ",");
                }
                parms.Add("currencies", builder.ToString().TrimEnd(','));
            }

            foreach (var kvPair in parms.OrderBy(a => a.Key))
            {
                if (parmstr.Length > 0)
                    parmstr.Append("&");
                parmstr.Append($"{kvPair.Key}={kvPair.Value}");
            }
            //计算签名
            var sign = EncryptHMACSHA256(API_SECRET, parmstr.ToString());
            parmstr.Append($"&sign={sign}");

            var response = await PostAsync(url, parmstr.ToString());
            return response;
        }



        public async Task<string> GetAsync(string url)
        {
            var response = await client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync();
            return string.Empty;
        }

        public async Task<string> PostAsync(string url, string strData)
        {
            var content = new StringContent(strData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync();
            return string.Empty;
        }

        public static string EncryptHMACSHA256(string key, string value)
        {
            using (var sha = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
                string result = string.Empty;
                foreach (var item in bytes)
                {
                    result += item.ToString("x");
                }
                return result;
            }
        }
    }


}
