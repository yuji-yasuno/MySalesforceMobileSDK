using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using System.Net;
using System.Net.Http;
using System.IO;
using Windows.Data.Json;

namespace MySalesforceMobileSDK
{
    public class MySFOAuthCoordinator
    {

        #region Constants

        const String PARAM_NAME_ACCESS_TOKEN = "access_token";
        const String PARAM_NAME_REFRESH_TOKEN = "refresh_token";
        const String PARAM_NAME_INSTANCE_URL = "instance_url";
        const String PARAM_NAME_ID = "id";
        const String PARAM_NAME_ISSUED_AT = "issued_at";
        const String PARAM_NAME_SIGNATURE = "signature";
        const String PARAM_NAME_SCOPE = "scope";
        const Int32 DEFAULT_TIMEOUT = 100000;

        #endregion

        #region Events

        public event MySFOAuthEventHandler onCompletedAuthorization;
        public event MySFOAuthEventHandler onFailedAuthorization;
        public event MySFOAuthEventHandler onCanceledAuthorization;
        public event MySFOAuthEventHandler onCompletedRefresh;
        public event MySFOAuthEventHandler onFailedRefresh;
        public event MySFOAuthEventHandler onRequestFailedRefresh;
        public event MySFOAuthEventHandler onCompletedRefreshForRetry;
        public event MySFOAuthEventHandler onFailedRefreshForRetry;
        public event MySFOAuthEventHandler onRequestFailedRefreshForRetry;
        public event MySFOAuthEventHandler onCompletedRevokeToken;
        public event MySFOAuthEventHandler onFailedRevokeToken;
        public event MySFOAuthEventHandler onRequestFailedRevokeToken;
        public delegate void MySFOAuthEventHandler(Object sender, MySFOAuthEventArgs e);

        #endregion

        #region Property

        public int timeout { get; set; }
        public MySFOAuthCredentials credentials { get; set; }

        #endregion

        #region Constractor

        public MySFOAuthCoordinator(String clientId, String callback, String scope) 
        {
            this.credentials = new MySFOAuthCredentials();
            this.credentials.clientId = clientId;
            this.credentials.redirectUri = callback;
            this.credentials.scope = scope;
            this.timeout = DEFAULT_TIMEOUT;
        }

        #endregion

        #region Public Methods

        public async void authenticate()
        {
            String oauthUrl = @"https://login.salesforce.com/services/oauth2/authorize?response_type=token&client_id=" + this.credentials.clientId + @"&redirect_uri=" + this.credentials.redirectUri + @"&scope=" + this.credentials.scope;
            Uri startUri = new Uri(oauthUrl);
            Uri endUri = new Uri(this.credentials.redirectUri);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            if (result.ResponseStatus == WebAuthenticationStatus.Success) {

                Dictionary<String, String> parameters = MySFMobileSdkUtil.analyzeParamOfOAuthAuthorizeParams(WebUtility.UrlDecode(result.ResponseData));
                foreach (KeyValuePair<String, String> pair in parameters) {
                    if (String.Compare(pair.Key, PARAM_NAME_ACCESS_TOKEN) == 0) {
                        this.credentials.accessToken = pair.Value;
                    }
                    else if (String.Compare(pair.Key, PARAM_NAME_REFRESH_TOKEN) == 0) {
                        this.credentials.refreshToken = pair.Value;
                    }
                    else if (String.Compare(pair.Key, PARAM_NAME_INSTANCE_URL) == 0) {
                        this.credentials.instanceUrl = new Uri(pair.Value);
                        this.credentials.domain = this.credentials.instanceUrl.DnsSafeHost;
                    }
                    else if (String.Compare(pair.Key, PARAM_NAME_ID) == 0) {
                        // nothing to do
                    }
                    else if (String.Compare(pair.Key, PARAM_NAME_SIGNATURE) == 0) {
                        // nothing to do
                    }
                    else if (String.Compare(pair.Key, PARAM_NAME_SCOPE) == 0) {
                        // nothing to do
                    }
                }

                if (onCompletedAuthorization != null) {
                    MySFOAuthEventArgs args = new MySFOAuthEventArgs();
                    args.credentials = this.credentials;
                    onCompletedAuthorization(this, args);
                }
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp) {
                if (onFailedAuthorization != null) {
                    MySFOAuthEventArgs args = new MySFOAuthEventArgs();
                    args.credentials = this.credentials;
                    onFailedAuthorization(this, args);
                }
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                if (onCanceledAuthorization != null)
                {
                    onCanceledAuthorization(this, null);
                }
            }
        }

        public async void refresh(Boolean isFromRetry = false)
        {
            String refreshUrl = @"https://login.salesforce.com/services/oauth2/token";
            String postdata = @"grant_type=refresh_token&client_id=" + this.credentials.clientId + @"&refresh_token=" + this.credentials.refreshToken;

            HttpClient client = new HttpClient();

            Byte[] contentBytes = Encoding.UTF8.GetBytes(postdata);
            HttpContent content = new StreamContent(new MemoryStream(contentBytes));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            content.Headers.ContentLength = contentBytes.Length;
            
            HttpResponseMessage response = null;
            String responseContentStr = null;
            MySFOAuthEventArgs args = new MySFOAuthEventArgs();

            try 
            {
                response = await client.PostAsync(refreshUrl, content);
                responseContentStr = await response.Content.ReadAsStringAsync();

                args.credentials = this.credentials;
                args.error = null;
                args.status = response.StatusCode;
            } 
            catch (HttpRequestException ex ) 
            {
                args.credentials = this.credentials;
                args.error = ex;
                args.status = response.StatusCode;

                if (isFromRetry) {
                    if (onRequestFailedRefreshForRetry != null) onRequestFailedRefreshForRetry(this, args);
                }
                else {
                    if (onRequestFailedRefresh != null) onRequestFailedRefresh(this, args);
                }
            }

            if (response.StatusCode == HttpStatusCode.OK) {
                JsonValue jsonval;
                Boolean isSuccessParse = JsonValue.TryParse(responseContentStr, out jsonval);

                if (isSuccessParse) {
                    JsonObject jsonobj = jsonval.GetObject();
                    this.credentials.accessToken = jsonobj.GetNamedString(PARAM_NAME_ACCESS_TOKEN);    
                }

                if (isFromRetry){
                    if (onCompletedRefreshForRetry != null) onCompletedRefreshForRetry(this, args);
                }
                else {
                    if (onCompletedRefresh != null) onCompletedRefresh(this, args);
                }

            }
            else {
                if (isFromRetry) {
                    if (onFailedRefresh != null) onFailedRefresh(this, args);
                }
                else {
                    if (onFailedRefreshForRetry != null) onFailedRefreshForRetry(this, args);
                }
            }
        }

        public async void revoke() 
        {
            String revokeUrl = @"https://login.salesforce.com/services/oauth2/revoke?token=" + this.credentials.accessToken;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            String responseContentStr = null;
            MySFOAuthEventArgs args = new MySFOAuthEventArgs();

            try 
            {
                response = await client.GetAsync(revokeUrl);
                responseContentStr = await response.Content.ReadAsStringAsync();

                args.credentials = this.credentials;
                args.error = null;
                args.status = response.StatusCode;
            }
            catch (HttpRequestException ex) 
            {
                args.credentials = this.credentials;
                args.error = ex;
                args.status = response.StatusCode;
                if (onRequestFailedRevokeToken != null) onRequestFailedRevokeToken(this, args);
            }

            if (response.StatusCode == HttpStatusCode.OK) {
                if (onCompletedRevokeToken != null) onCompletedRevokeToken(this, args);
            }
            else {
                if (onFailedRevokeToken != null) onFailedRevokeToken(this, args);
            }
        }

        #endregion

    }
}
