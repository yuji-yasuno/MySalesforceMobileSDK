using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;

namespace MySalesforceMobileSDK
{
    public class MySFOAuthEventArgs : EventArgs
    {
        public MySFOAuthCredentials credentials { get; set;  }
        public HttpRequestException error { get; set; }
        public HttpStatusCode status { get; set; }
    }
}
