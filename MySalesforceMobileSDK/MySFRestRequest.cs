using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Windows.Data.Json;

namespace MySalesforceMobileSDK
{
    public class MySFRestRequest : HttpRequestMessage
    {
        private const string PARTIAL_RELATIVE_ADDRESS = @"services/data";
        public MySFOAuthCredentials credentials { get; private set; }

        private MySFRestRequest() { }

        public MySFRestRequest(MySFOAuthCredentials credentials) {
            this.credentials = credentials;
            this.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.credentials.accessToken);
        }

        public Uri createApiUri(String apiVersion, String partialUrl)
        {
            Uri result;
            result = new Uri(this.credentials.instanceUrl.AbsoluteUri + PARTIAL_RELATIVE_ADDRESS + "/" + apiVersion + partialUrl);
            return result;
        }

        public Uri createApiUri(String apiVersion, String partialUrl, Dictionary<String, String> urlParams)
        {
            Uri result;
            String p = "";
            int count = 0;
            foreach (KeyValuePair<String, String> pair in urlParams) 
            {
                if (count == 0)
                {
                    p = pair.Key + @"=" + pair.Value;
                }
                else 
                {
                    p += @"&" + pair.Key + @"=" + pair.Value;
                }
                count++;
            }
            result = new Uri(this.credentials.instanceUrl.AbsoluteUri + PARTIAL_RELATIVE_ADDRESS + "/" + apiVersion + partialUrl + @"?" + p);
            return result;
        }

        public Uri createApiUri(String apiVersion, String partialUrl, List<String> fields)
        {
            Uri result;
            String p = "fields=";
            int count = 0;
            foreach (String field in fields)
            {
                if (count == 0)
                {
                    p += field;
                }
                else
                {
                    p += @"," + field;
                }
                count++;
            }
            result = new Uri(this.credentials.instanceUrl.AbsoluteUri + PARTIAL_RELATIVE_ADDRESS + "/" + apiVersion + partialUrl + @"?" + p);
            return result;
        }

        public HttpContent createContent(Dictionary<String, String> fields)
        {
            HttpContent content;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            String body = json.Stringify();
            Byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            content = new StreamContent(new MemoryStream(bodyBytes));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            content.Headers.ContentLength = bodyBytes.Length;

            return content;
        }

        public HttpContent createContent(String body)
        {
            HttpContent content;

            Byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
            content = new StreamContent(new MemoryStream(bodyBytes));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            content.Headers.ContentLength = bodyBytes.Length;

            return content;
        }

        public static MySFRestRequest createNewRequestFromCredentials(MySFOAuthCredentials credentials) 
        {
            MySFRestRequest request = new MySFRestRequest(credentials);
            return request;
        }

    }
}
