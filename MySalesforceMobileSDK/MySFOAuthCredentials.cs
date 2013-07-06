using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySalesforceMobileSDK
{
    public class MySFOAuthCredentials
    {
        public String domain { get; set; }
        public String clientId { get; set; }
        public String redirectUri { get; set; }
        public string refreshToken { get; set; }
        public string accessToken { get; set; }
        public Uri instanceUrl { get; set; }
        public string scope { get; set; }
    }
}
