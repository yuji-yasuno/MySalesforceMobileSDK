using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using Windows.Data.Json;

namespace MySalesforceMobileSDK
{
    public class MySFRestAPI
    {
        #region Events

        public event MySFRestEventHandler onCompletedLoadResponse;
        public event MySFRestEventHandler onFailedLoadResponse;
        public event MySFRestEventHandler onCanceldLoadResponse;
        public event MySFRestEventHandler onTimeoutLoadResponse;
        public event MySFRestEventHandler onFailedRetry;
        public delegate void MySFRestEventHandler(Object sender, MySFRestEventArgs e);

        #endregion

        #region Property

        public MySFOAuthCoordinator coordinator { get; set; }
        public String apiVersion { get; set; }

        #endregion

        #region Member

        private static MySFRestAPI _instance;
        private HttpMethod _prevRequestMethod;
        private String _prevRequestUrl;
        private String _prevRequestBody;

        #endregion

        #region Constractor

        private MySFRestAPI() { }

        #endregion

        #region Public Methods

        public MySFRestRequest requestForCreateWithObjectType(string objectType, Dictionary<String, String> fields)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType);
            request.Content = request.createContent(fields);
            return request;
        }

        public MySFRestRequest requestForDeleteWithObjectType(string objectType, string objectId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Delete;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType + @"/" + objectId);
            return request;
        }

        public MySFRestRequest requestForDescribeGlobal()
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects");
            return request;
        }

        public MySFRestRequest requestForDescribeWithObjectType(string objectType)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType + @"/describe");
            return request;
        }

        public MySFRestRequest requestForMetadataWithObjectType(string objectType)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType);
            return request;
        }

        public MySFRestRequest requestForQuery(string soql)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/query?q=" + WebUtility.UrlEncode(soql));
            return request;
        }

        public MySFRestRequest requestForResources()
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/");
            return request;
        }

        public MySFRestRequest requestForRetrieveWithObjectType(string objectType, string objectId, List<String> fieldList)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType + @"/" + objectId, fieldList);
            return request;
        }

        public MySFRestRequest requestForSearch(string sosl)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/search?q=" + WebUtility.UrlEncode(sosl));
            return request;
        }

        public MySFRestRequest requestForUpdateWithObjectType(string objectType, string objectId, Dictionary<String, String> fields)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = new HttpMethod("PATCH");
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType + @"/" + objectId);
            request.Content = request.createContent(fields);
            return request;
        }

        public MySFRestRequest requestForUpsertObjectType(string objectType, string externalIdField, String externalId, Dictionary<String, String> fields)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(this.coordinator.credentials);
            request.Method = new HttpMethod("PATCH");
            request.RequestUri = request.createApiUri(this.apiVersion, @"/sobjects/" + objectType + @"/" + externalIdField + "/" + externalId);
            request.Content = request.createContent(fields);
            return request;
        }

        public async void send(MySFRestRequest request, Boolean isRetry = false)
        {

            _prevRequestMethod = request.Method;
            _prevRequestUrl = request.RequestUri.AbsoluteUri;
            _prevRequestBody = null;
            if (request.Content != null) 
            {
                _prevRequestBody = await request.Content.ReadAsStringAsync();
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            String responseBody = null;
            try
            {
                response = await client.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex) 
            {
                MySFRestEventArgs args1 = new MySFRestEventArgs();
                args1.error = ex;
                args1.status = response.StatusCode;
                if (onFailedLoadResponse != null)
                {
                    onFailedLoadResponse(this, args1);
                }
                return;
            }

            if (!isRetry)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized) {
                    this.coordinator.onCompletedRefreshForRetry += coordinator_onCompletedRefreshForRetry;
                    this.coordinator.onFailedRefreshForRetry += coordinator_onFailedRefreshForRetry;
                    this.coordinator.onRequestFailedRefreshForRetry += coordinator_onRequestFailedRefreshForRetry;
                    this.coordinator.refresh(true);
                    return;
                }
            }

            MySFRestEventArgs args2 = new MySFRestEventArgs();
            args2.status = response.StatusCode;
            args2.responseData = MySFMobileSdkUtil.createObjectFromJson(responseBody);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                case HttpStatusCode.NoContent:
                    if (onCompletedLoadResponse != null) {
                        onCompletedLoadResponse(this, args2);
                    }
                    break;

                case HttpStatusCode.PartialContent:
                    if (onCanceldLoadResponse != null) {
                        onCanceldLoadResponse(this, args2);
                    }
                    break;

                case HttpStatusCode.RequestTimeout:
                    if (onTimeoutLoadResponse != null) {
                        onTimeoutLoadResponse(this, args2);
                    }
                    break;

                case HttpStatusCode.Unauthorized:
                    if (onFailedRetry != null) {
                        onFailedRetry(this, args2);
                    }
                    break;

                default:
                    if (onFailedLoadResponse != null) {
                        onFailedLoadResponse(this, args2);
                    }
                    break;
            }
        }

        public static MySFRestAPI getInstance() 
        {
            if (_instance == null) _instance = new MySFRestAPI();
            return _instance;
        }

        #endregion

        #region MySFOAuthcoordinator Events

        private void coordinator_onCompletedRefreshForRetry(Object sender, MySFOAuthEventArgs e) 
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(e.credentials);
            request.Method = _prevRequestMethod;
            request.RequestUri = new Uri(_prevRequestUrl);
            if (_prevRequestBody != null) 
            {
                request.Content = request.createContent(_prevRequestBody);
            }
            this.send(request, true); 
        }

        private void coordinator_onFailedRefreshForRetry(Object sender, MySFOAuthEventArgs e)
        {
            MySFRestEventArgs args = new MySFRestEventArgs();
            args.error = e.error;
            args.status = e.status;
            if (onFailedRetry != null) { 
                onFailedRetry(this, args);
            }
        }

        private void coordinator_onRequestFailedRefreshForRetry(Object sender, MySFOAuthEventArgs e)
        {
            MySFRestEventArgs args = new MySFRestEventArgs();
            args.error = e.error;
            args.status = e.status;
            if (onFailedRetry != null)
            {
                onFailedRetry(this, args);
            }
        }

        #endregion

    }
}
