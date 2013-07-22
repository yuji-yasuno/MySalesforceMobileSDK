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

        public HttpContent createContent(Dictionary<string, string> fields)
        {
            HttpContent content;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            String body = json.Stringify();
            Byte[] bodyBytes = Encoding.Unicode.GetBytes(body);
            Byte[] bodyBytesUtf8 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bodyBytes);
            content = new StreamContent(new MemoryStream(bodyBytesUtf8));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            content.Headers.ContentType.CharSet = @"UTF-8";
            content.Headers.ContentLength = bodyBytesUtf8.Length;
            

            return content;
        }

        public HttpContent createContentFromJson(Dictionary<string, JsonObject> fields, String disposition = null, String dispositionName = null)
        {
            HttpContent content;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            String body = json.Stringify();
            Byte[] bodyBytes = Encoding.Unicode.GetBytes(body);
            Byte[] bodyBytesUtf8 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bodyBytes);
            content = new StreamContent(new MemoryStream(bodyBytesUtf8));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            content.Headers.ContentType.CharSet = @"UTF-8";
            content.Headers.ContentLength = bodyBytesUtf8.Length;
            if (disposition != null) {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue(disposition);
                if (dispositionName != null)
                {
                    content.Headers.ContentDisposition.Name = dispositionName;
                }
            }

            return content;
        }

        public HttpContent createContentForAttachingNewFile(Byte[] fileData, String disposition = null, String dispositionName = null, String dispositionFileName = null)
        {
            HttpContent content;

            Byte[] bodyBytes = fileData;
            content = new StreamContent(new MemoryStream(bodyBytes));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/octet-stream");
            content.Headers.ContentLength = bodyBytes.Length;
            
            if (disposition != null)
            {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue(disposition);
                if (dispositionName != null) {
                    content.Headers.ContentDisposition.Name = dispositionName;
                }
                if (dispositionFileName != null) {
                    content.Headers.ContentDisposition.FileName = dispositionFileName;
                }
            }

            return content;
        }

        public HttpContent createContentFromJsonWithExtraFields(Dictionary<string, JsonObject> fields, Dictionary<string, string> extrafields, String disposition = null, String dispositionName = null)
        {
            HttpContent content;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            foreach (KeyValuePair<string, string> pair in extrafields) {
                json.Add(pair.Key, JsonValue.CreateStringValue(pair.Value));
            }
            String body = json.Stringify();
            Byte[] bodyBytes = Encoding.Unicode.GetBytes(body);
            Byte[] bodyBytesUtf8 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bodyBytes);
            content = new StreamContent(new MemoryStream(bodyBytesUtf8));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            content.Headers.ContentType.CharSet = @"UTF-8";
            content.Headers.ContentLength = bodyBytesUtf8.Length;
            if (disposition != null)
            {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue(disposition);
                if (dispositionName != null)
                {
                    content.Headers.ContentDisposition.Name = dispositionName;
                }
            }

            return content;
        }

        public HttpContent createContent(String body)
        {
            HttpContent content;

            Byte[] bodyBytes = Encoding.Unicode.GetBytes(body);
            Byte[] bodyBytesUtf8 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bodyBytes);
            content = new StreamContent(new MemoryStream(bodyBytesUtf8));
            content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
            content.Headers.ContentType.CharSet = @"UTF-8";
            content.Headers.ContentLength = bodyBytesUtf8.Length;

            return content;
        }

        public MultipartFormDataContent createMultiPartContent(String boundary, HttpContent[] contents)
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent(boundary);
            foreach (HttpContent content in contents) 
            {
                multiContent.Add(content);
            }
            return multiContent;
        }

        public String createContentString(Dictionary<string, string> fields)
        {
            String result;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            result = json.Stringify();

            return result;
        }

        public String createContentString(Dictionary<string, JsonObject> fields, Dictionary<string, string> extrafields)
        {
            String result;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            foreach (KeyValuePair<string, string> pair in extrafields)
            {
                json.Add(pair.Key, JsonValue.CreateStringValue(pair.Value));
            }
            result = json.Stringify();

            return result;
        }

        public String createContentStrinFromJson(Dictionary<string, JsonObject> fields)
        {
            String result;

            JsonObject json = MySFMobileSdkUtil.createJsonFromObject(fields);
            result = json.Stringify();

            return result;
        }

        public static MySFRestRequest createNewRequestFromCredentials(MySFOAuthCredentials credentials) 
        {
            MySFRestRequest request = new MySFRestRequest(credentials);
            return request;
        }

    }
}
