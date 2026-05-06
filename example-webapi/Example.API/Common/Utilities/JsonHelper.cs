using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.API.Common.Utilities
{
    public static class JsonHelper //common
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
        };

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static TObject Deserialize<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json, settings);
        }

        public static string Base64UrlDecode(string input)
        {
            string padded = input.Length % 4 == 0
                ? input
                : input + new string('=', 4 - input.Length % 4);

            string base64 = padded
                .Replace('-', '+')
                .Replace('_', '/');

            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
