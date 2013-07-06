using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Data.Json;
using System.Net;
using System.Net.Http;

namespace MySalesforceMobileSDK
{
    public class MySFRestEventArgs
    {
        public JsonValue responseData { get; set; }
        public HttpRequestException error { get; set; }
        public HttpStatusCode status { get; set; }
    }
}
