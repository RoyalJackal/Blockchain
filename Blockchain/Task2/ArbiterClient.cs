using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Blockchain.Task2
{
    public class ArbiterClient
    {
        public const string Scheme = "http";
        public const string Host = "188.93.211.195";
        public const int Port = 8090;
        public const string TimeStampPath = "ts";
        public const string PublicKeyPath = "public";
        public const string PrivateKeyPath = "private";
        public const string ValidateHashPath = "verify";

        private readonly HttpClient _client;

        public ArbiterClient()
        {
            _client = new HttpClient();
        }

        public static string DateToString(DateTimeOffset date) =>
            date.ToString("yyyy-MM-ddTHH:mm:ss.fffzz");

        public TimeStampDto GetTimeStamp(string hash)
        {
            var builder = new UriBuilder(Scheme, Host, Port, TimeStampPath);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["digest"] = hash;
            builder.Query = query.ToString() ?? string.Empty;
            var url = builder.ToString();

            var response = _client.GetAsync(url).GetAwaiter().GetResult();
            var str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<TimeStampDto>(str);
        }

        public bool VerifySignature(DateTimeOffset date, string hash, string sign)
        {
            var dateString = DateToString(date);
            /*Console.WriteLine(@"Put the following string to https://string-functions.com/urlencode.aspx");
            Console.WriteLine(dateString);
            var dateUrl = Console.ReadLine();
            if (string.IsNullOrEmpty(dateUrl))
                throw new Exception("String is empty.");
            var byteDate = Encoding.Latin1.GetBytes(dateUrl.ToUpper());*/
            var byteDate = Encoding.UTF8.GetBytes(dateString);
            var byteHash = Convert.FromHexString(hash);
            var dateHash = Convert.ToHexString(byteDate.Concat(byteHash).ToArray()).ToLower();

            var builder = new UriBuilder(Scheme, Host, Port, ValidateHashPath);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["sign"] = sign;
            query["digest"] = dateHash;
            builder.Query = query.ToString() ?? string.Empty;
            var url = builder.ToString();

            var response = _client.GetAsync(url).GetAwaiter().GetResult();
            var str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return bool.Parse(str);
        }

        public string GetPublicKey()
        {
            var builder = new UriBuilder(Scheme, Host, Port, PublicKeyPath);
            var url = builder.ToString();

            var response = _client.GetAsync(url).GetAwaiter().GetResult();
            var str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return str;
        }
    }
}
