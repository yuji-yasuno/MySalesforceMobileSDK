using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using Windows.Data.Json;

namespace MySalesforceMobileSDK
{
    public class MySFChatterRestAPI
    {
        #region Events

        public event MySFRestChatterEventHandler onCompletedLoadResponse;
        public event MySFRestChatterEventHandler onFailedLoadResponse;
        public event MySFRestChatterEventHandler onCanceldLoadResponse;
        public event MySFRestChatterEventHandler onTimeoutLoadResponse;
        public event MySFRestChatterEventHandler onFailedRetry;
        public delegate void MySFRestChatterEventHandler(Object sender, MySFRestEventArgs e);

        #endregion

        #region Property

        public static MySFOAuthCoordinator coordinator { get; set; }
        public String apiVersion { get; set; }

        #endregion

        #region Member

        private MySFRestAPI _api;

        #endregion

        #region Constractor

        public MySFChatterRestAPI()
        {
            _api = new MySFRestAPI();
            _api.onCanceldLoadResponse += api_onCanceldLoadResponse;
            _api.onCompletedLoadResponse += api_onCompletedLoadResponse;
            _api.onFailedLoadResponse += api_onFailedLoadResponse;
            _api.onFailedRetry += api_onFailedRetry;
            _api.onTimeoutLoadResponse += api_onTimeoutLoadResponse;
        }

        #endregion

        #region Public Events

        public void newsFeed()
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/news/me/feed-items");
            _api.send(request);
        }

        public void newsFeed(String userId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/news/" + userId + @"/feed-items");
            _api.send(request);
        }

        public void profileFeed()
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/user-profile/me/feed-items");
            _api.send(request);
        }

        public void profileFeed(String userId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/user-profile/" + userId + @"/feed-items");
            _api.send(request);
        }

        public void recordFeed()
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/record/me/feed-items");
            _api.send(request);
        }

        public void recordFeed(string recordId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/record/" + recordId + @"/feed-items");
            _api.send(request);
        }

        public void feedItem(String feedId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedId);
            _api.send(request);
        }

        public void feedComments(String feedId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedId + @"/comments");
            _api.send(request);
        }

        public void feedLikes(String feedId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedId + @"/likes");
            _api.send(request);
        }

        public void feedToMe() 
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/to/me/feed-items");
            _api.send(request);
        }

        public void feedToUser(String userId) 
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/to/" + userId + @"/feed-items");
            _api.send(request);
        }

        public void commentLikes(string commentId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Get;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/comments/" + commentId + "/likes");
            _api.send(request);
        }

        public void createNewsFeedAttachingExistingContent(string contentDocumentId, string text)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/news/me/feed-items");

            JsonObject attachmentJson = createJsonForAttachmentAsExsistingContent(contentDocumentId);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            request.Content = request.createContentFromJson(fields);
            _api.send(request);
        }

        public void createRecordFeedAttachingExistingContent(string contentDocumentId, string text, string recordId,Boolean isSetVisibility = false, Boolean isInternal = false)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/record/" + recordId + "/feed-items");

            JsonObject attachmentJson = createJsonForAttachmentAsExsistingContent(contentDocumentId);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            if (isSetVisibility)
            {
                Dictionary<string, string> extrafields = new Dictionary<string, string>();
                if (isInternal)
                {
                    extrafields.Add("visibility", "InternalUsers");
                }
                else
                {
                    extrafields.Add("visibility", "AllUsers");
                }
                request.Content = request.createContentFromJsonWithExtraFields(fields, extrafields);
            }
            else 
            {
                request.Content = request.createContentFromJson(fields);
            }
            _api.send(request);
        }

        public void createNewsFeedAttachingNewFile(Byte[] fileData, string fileName, string title, string description, string text)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/news/me/feed-items");

            JsonObject attachmentJson = createJsonForAttachmentAsNewContent(title, description);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            HttpContent contentOfJson = request.createContentFromJson(fields, "form-data","json");
            HttpContent contentOfFileData = request.createContentForAttachingNewFile(fileData, "form-data", "feedItemFileUpload", fileName);
            
            Random rnd = new Random(90);
            String boundary = Convert.ToChar(rnd.Next(65, 90)).ToString() 
                + Convert.ToChar(rnd.Next(65, 90)).ToString() 
                + Convert.ToChar(rnd.Next(65, 90)).ToString() 
                + Convert.ToChar(rnd.Next(65, 90)).ToString() 
                + Convert.ToChar(rnd.Next(65, 90)).ToString() 
                + Convert.ToChar(rnd.Next(65, 90)).ToString(); // select from // A-Z

            request.Content = request.createMultiPartContent(boundary, new HttpContent[] {contentOfJson, contentOfFileData});
            _api.send(request);
        }

        public void createRecordFeedAttachingNewFile(Byte[] fileData, string fileName, string title, string description, string text, string recordId, Boolean isSetVisibility = false, Boolean isInternal = false)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/record/" + recordId + "/feed-items");

            JsonObject attachmentJson = createJsonForAttachmentAsNewContent(title, description);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            HttpContent contentOfJson = null;
            if (isSetVisibility)
            {
                Dictionary<string, string> extrafields = new Dictionary<string, string>();
                if (isInternal)
                {
                    extrafields.Add("visibility", "InternalUsers");
                }
                else
                {
                    extrafields.Add("visibility", "AllUsers");
                }
                contentOfJson = request.createContentFromJsonWithExtraFields(fields, extrafields, "form-data", "json");
            }
            else
            {
                contentOfJson = request.createContentFromJson(fields, "form-data", "json");
            }
            HttpContent contentOfFileData = request.createContentForAttachingNewFile(fileData, "form-data", "feedItemFileUpload", fileName);

            Random rnd = new Random(90);
            String boundary = Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString(); // select from // A-Z

            request.Content = request.createMultiPartContent(boundary, new HttpContent[] { contentOfJson, contentOfFileData });
            _api.send(request);
        }

        public void createNewsFeedAttachingLink(string url, string urlName, string text, Boolean isSetVisibility = false, Boolean isInternal = false)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/news/me/feed-items");

            JsonObject attachmentJson = createJsonForAttachmentAsLink(url, urlName);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            if (isSetVisibility)
            {
                Dictionary<string, string> extrafields = new Dictionary<string, string>();
                if (isInternal)
                {
                    extrafields.Add("visibility", "InternalUsers");
                }
                else
                {
                    extrafields.Add("visibility", "AllUsers");
                }
                request.Content = request.createContentFromJsonWithExtraFields(fields, extrafields);
            }
            else
            {
                request.Content = request.createContentFromJson(fields);
            }
            _api.send(request);
        }

        public void createRecordFeedAttachingLink(string url, string urlName, string text, string recordId, Boolean isSetVisibility = false, Boolean isInternal = false)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feeds/record/" + recordId + "/feed-items");

            JsonObject attachmentJson = createJsonForAttachmentAsLink(url, urlName);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            if (isSetVisibility)
            {
                Dictionary<string, string> extrafields = new Dictionary<string, string>();
                if (isInternal)
                {
                    extrafields.Add("visibility", "InternalUsers");
                }
                else
                {
                    extrafields.Add("visibility", "AllUsers");
                }
                request.Content = request.createContentFromJsonWithExtraFields(fields, extrafields);
            }
            else
            {
                request.Content = request.createContentFromJson(fields);
            }
            _api.send(request);
        }

        public void createComment(string text, string feedItemId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedItemId + "/comments");

            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("body", bodyJson);

            request.Content = request.createContentFromJson(fields);
            _api.send(request);
        }

        public void createCommenAttachingExistingContent(string contentDocumentId, string text, string feedItemId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedItemId + "/comments");

            JsonObject attachmentJson = createJsonForAttachmentAsExsistingContent(contentDocumentId);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            request.Content = request.createContentFromJson(fields);
            _api.send(request);
        }

        public void createCommenAttachingNewFile(Byte[] fileData, string fileName, string title, string description, string text, string feedItemId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedItemId + "/comments");

            JsonObject attachmentJson = createJsonForAttachmentAsNewContent(title, description);
            JsonObject messageSegmentOfTextJson = createJsonForMessageSegmentOfTextType(text);
            JsonArray messageSegments = new JsonArray();
            messageSegments.Add(messageSegmentOfTextJson);
            JsonObject bodyJson = new JsonObject();
            bodyJson.Add("messageSegments", messageSegments);

            Dictionary<string, JsonObject> fields = new Dictionary<string, JsonObject>();
            fields.Add("attachment", attachmentJson);
            fields.Add("body", bodyJson);

            HttpContent contentOfJson = request.createContentFromJson(fields, "form-data", "json");
            HttpContent contentOfFileData = request.createContentForAttachingNewFile(fileData, "form-data", "feedItemFileUpload", fileName);

            Random rnd = new Random(90);
            String boundary = Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString()
                + Convert.ToChar(rnd.Next(65, 90)).ToString(); // select from // A-Z

            request.Content = request.createMultiPartContent(boundary, new HttpContent[] { contentOfJson, contentOfFileData });
            _api.send(request);
        }

        public void createLikeToFeed(string feedItemId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedItemId + "/likes");
            _api.send(request);
        }

        public void createLikeToComment(string commentId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Post;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/comments/" + commentId + "/likes");
            _api.send(request);
        }

        public void deleteLikeToFeed(string feedItemId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Delete;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/feed-items/" + feedItemId + "/likes");
            _api.send(request);
        }

        public void deleteLikeToComment(string commentId)
        {
            MySFRestRequest request = MySFRestRequest.createNewRequestFromCredentials(MySFChatterRestAPI.coordinator.credentials);
            request.Method = HttpMethod.Delete;
            request.RequestUri = request.createApiUri(this.apiVersion, @"/chatter/comments/" + commentId + "/likes");
            _api.send(request);
        }

        #endregion

        #region Private Methods

        protected void api_onCompletedLoadResponse(Object sender, MySFRestEventArgs e)
        {
            if(onCompletedLoadResponse !=  null) {
                onCompletedLoadResponse(this, e);
            }
        }

        protected void api_onFailedLoadResponse(Object sender, MySFRestEventArgs e)
        {
            if (onFailedLoadResponse != null) {
                onFailedLoadResponse(this, e);
            }
        }

        protected void api_onCanceldLoadResponse(Object sender, MySFRestEventArgs e)
        {
            if (onCanceldLoadResponse != null) {
                onCanceldLoadResponse(this, e);
            }
        }

        protected void api_onTimeoutLoadResponse(Object sender, MySFRestEventArgs e)
        {
            if (onTimeoutLoadResponse != null) {
                onTimeoutLoadResponse(this, e);
            }
        }

        protected void api_onFailedRetry(Object sender, MySFRestEventArgs e)
        {
            if (onFailedRetry != null) {
                onFailedRetry(this, e);
            }
        }

        protected JsonObject createJsonForAttachmentAsExsistingContent(string contentDocumentId) 
        {
            JsonObject result = null;

            Dictionary<string, string> attachmentFields = new Dictionary<string, string>();
            attachmentFields.Add("attachmentType", "ExistingContent");
            attachmentFields.Add("contentDocumentId", contentDocumentId);
            result = MySFMobileSdkUtil.createJsonFromObject(attachmentFields);

            return result;
        }

        protected JsonObject createJsonForAttachmentAsNewContent(string title, string description)
        {
            JsonObject result = null;

            Dictionary<string, string> attachmentFields = new Dictionary<string, string>();
            attachmentFields.Add("attachmentType", "NewFile");
            attachmentFields.Add("title", title);
            attachmentFields.Add("description", description);
            result = MySFMobileSdkUtil.createJsonFromObject(attachmentFields);

            return result;
        }

        protected JsonObject createJsonForAttachmentAsLink(string url, string urlName) 
        {
            JsonObject result = null;

            Dictionary<string, string> attachmentFields = new Dictionary<string, string>();
            attachmentFields.Add("attachmentType", "Link");
            attachmentFields.Add("url", url);
            attachmentFields.Add("urlName", urlName);
            result = MySFMobileSdkUtil.createJsonFromObject(attachmentFields);

            return result;
        }

        protected JsonObject createJsonForMessageSegmentOfTextType(string text)
        {
            JsonObject result = null;

            Dictionary<string, string> typeSegmentFields = new Dictionary<string, string>();
            typeSegmentFields.Add("type", "Text");
            typeSegmentFields.Add("text", text);
            result = MySFMobileSdkUtil.createJsonFromObject(typeSegmentFields);

            return result;
        }

        protected JsonObject createJsonForMessageSegmentOfLink(string url)
        {
            JsonObject result = null;

            Dictionary<string, string> typeSegmentFields = new Dictionary<string, string>();
            typeSegmentFields.Add("type", "Link");
            typeSegmentFields.Add("url", url);
            result = MySFMobileSdkUtil.createJsonFromObject(typeSegmentFields);

            return result;
        }

        #endregion
    }
}
