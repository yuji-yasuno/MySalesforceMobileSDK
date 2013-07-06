using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.Data.Json;
using System.Runtime.Serialization.Json;
using System.IO;

namespace MySalesforceMobileSDK
{
    public class MySFMobileSdkUtil
    {
        public static Dictionary<String, String> analyzeParamOfOAuthAuthorizeParams(String returnUrl) 
        {
            Dictionary<String, String> result = new Dictionary<String, String>();

            String regexStrForUrl = @".+#";
            Regex regexForUrl = new Regex(regexStrForUrl, RegexOptions.IgnoreCase);
            String urlStr = regexForUrl.Match(returnUrl).Value;
            String urlParamPart = returnUrl.Replace(urlStr, "");

            String spliter = @"&";
            String[] paramParts = urlParamPart.Split(spliter.ToCharArray());

            foreach (String item in paramParts) {
                String regexStr = @"[\w]+=";
                Regex regex = new Regex(regexStr, RegexOptions.IgnoreCase);
                String tmpStr = regex.Match(item).Value;
                String param = tmpStr.Replace("=", "");
                String value = item.Replace(tmpStr, "");
                result.Add(param, value);
            }

            return result;
        }

        public static JsonObject createJsonFromObject(Dictionary<String, String> fields) 
        {
            JsonObject result = new JsonObject();
            foreach (KeyValuePair<String, String> pair in fields) {
                result.Add(pair.Key, JsonValue.CreateStringValue(pair.Value));
            }
            return result;
        }

        public static JsonValue createObjectFromJson(String jsonstr) 
        {
            JsonValue jsonval;
            Boolean isSuccessParse = JsonValue.TryParse(jsonstr, out jsonval);            
            return isSuccessParse ? jsonval : null;
        }

        public static Object convertJsonValue(JsonValue jsonval) 
        {
            Object result = null;
            switch (jsonval.ValueType) 
            {
                case JsonValueType.Array:
                    result = jsonval.GetArray();
                    break;
                case JsonValueType.Boolean:
                    result = jsonval.GetBoolean();
                    break;
                case JsonValueType.Null:
                    result = null;
                    break;
                case JsonValueType.Number:
                    result = jsonval.GetNumber();
                    break;
                case JsonValueType.Object:
                    result = jsonval.GetObject();
                    break;
                case JsonValueType.String:
                    result = jsonval.GetString();
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
