using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using System.Text;
using MySalesforceMobileSDK;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace MySalesforceMobileSDKDebug
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private MySFOAuthCoordinator _coordinator;

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private MySFOAuthCoordinator createCoordinator() 
        {
            String consumerKey = @"3MVG9I1kFE5Iul2Dg.l1JaE9JS0bLtQaXGQhYc6cHNFp80uFMueesAfGdo0Kbo29rW5Wx9rcBGONhj8o5_BW.";
            String callbackUrl = @"https://login.salesforce.com/services/oauth2/success";
            String scope = @"full refresh_token";

            MySFOAuthCoordinator coordinator = new MySFOAuthCoordinator(consumerKey, callbackUrl, scope);
            coordinator.onCompletedAuthorization += coordinator_onCompletedAuthorization;
            coordinator.onFailedAuthorization += coordinator_onFailedAuthorization;
            coordinator.onCanceledAuthorization += coordinator_onCanceledAuthorization;
            coordinator.onCompletedRefresh += coordinator_onCompletedRefresh;
            coordinator.onFailedRefresh += coordinator_onFailedRefresh;
            coordinator.onRequestFailedRefresh += coordinator_onRequestFailedRefresh;
            coordinator.onCompletedRevokeToken += coordinator_onCompletedRevokeToken;
            coordinator.onRequestFailedRevokeToken += coordinator_onRequestFailedRevokeToken;
            coordinator.onFailedRevokeToken += coordinator_onFailedRevokeToken;

            return coordinator;
        }

        private void coordinator_onCompletedAuthorization(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Completed Authorization");
        }
        private void coordinator_onFailedAuthorization(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Failed Authorization");
        }
        private void coordinator_onCanceledAuthorization(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Canceled Authorization");
        }
        private void coordinator_onCompletedRefresh(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Completed Refresh");
        }
        private void coordinator_onFailedRefresh(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Failed Refresh");
        }
        private void coordinator_onRequestFailedRefresh(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Request Failed Refresh");
        }
        private void coordinator_onCompletedRevokeToken(Object sender, MySFOAuthEventArgs args) 
        {
            showDebugMessage("Completed Revoke Token");
        }
        private void coordinator_onFailedRevokeToken(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Request Failed Revoke Token");
        }
        private void coordinator_onRequestFailedRevokeToken(Object sender, MySFOAuthEventArgs args)
        {
            showDebugMessage("Failed Revoke Token");
        }

        private async void showDebugMessage(String msg) 
        {
            try
            {
                MessageDialog dialog = new MessageDialog(msg, "DEBUG");
                await dialog.ShowAsync();
            }
            catch (Exception ex) {
            };
        }

        private void btnTestOAuthAuthenticate_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                _coordinator = createCoordinator();
            }
            _coordinator.authenticate();
        }

        private void btnTestOAuthRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null) 
            {
                _coordinator = createCoordinator();
            }
            _coordinator.refresh();
        }

        private void btnTestOAuthRevokeToken_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                _coordinator = createCoordinator();
            }
            _coordinator.revoke();
        }

        private void sfapi_onCompletedLoadResponse(Object sender, MySFRestEventArgs e) 
        {
            String jsonstr = e.responseData != null ? e.responseData.Stringify() : "null";
            showDebugMessage("CompletedLoadResponse\n" + jsonstr);
        }

        private void sfapi_onFailedLoadResponse(Object sender, MySFRestEventArgs e)
        {
            String jsonstr = e.responseData != null ? e.responseData.Stringify() : "null";
            showDebugMessage("FailedLoadResponse\n" + jsonstr);
        }

        private void sfapi_onCanceldLoadResponse(Object sender, MySFRestEventArgs e)
        {
            showDebugMessage("CanceledLoadResponse");
        }

        private void sfapi_onTimeoutLoadResponse(Object sender, MySFRestEventArgs e)
        {
            showDebugMessage("TimeoutLoadResponse");
        }

        private void sfapi_onFailedRetry(Object sender, MySFRestEventArgs e)
        {
            showDebugMessage("FailedRetry");
        }

        private MySFRestAPI getApi() 
        {
            MySFRestAPI api = new MySFRestAPI();
            MySFRestAPI.coordinator = _coordinator;
            MySFRestAPI.apiVersion = @"v28.0";
            api.onCanceldLoadResponse += sfapi_onCanceldLoadResponse;
            api.onCompletedLoadResponse += sfapi_onCompletedLoadResponse;
            api.onFailedLoadResponse += sfapi_onFailedLoadResponse;
            api.onFailedRetry += sfapi_onFailedRetry;
            api.onTimeoutLoadResponse += sfapi_onTimeoutLoadResponse;
            return api;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            showDebugMessage("TEST");
        }

        private void btnCreateObject_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            Dictionary<String, String> fields = new Dictionary<string, string>();
            fields.Add("Name", txtAccountName.Text);
            MySFRestRequest request = api.requestForCreateWithObjectType("Account", fields);
            api.send(request);
        }

        private void testGetObject_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForRetrieveWithObjectType("Account", txtObjectIdForGet.Text, new List<String> { "Name", "Id" });
            api.send(request);
        }

        private void testUpdateObject_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            fields.Add("Name", "Update Test");
            fields.Add("NumberOfEmployees", "10000");
            MySFRestRequest request = api.requestForUpdateWithObjectType("Account", txtObjectIdForUpdate.Text, fields);
            api.send(request);
        }

        private void testDeleteObject_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForDeleteWithObjectType("Account", txtObjectIdForDelete.Text);
            api.send(request);
        }

        private void btnTestDescribeGlobal_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForDescribeGlobal();
            api.send(request);
        }

        private void btnTestDescribeObject_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForDescribeWithObjectType("Account");
            api.send(request);
        }

        private void btnTestMetadataObject_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForMetadataWithObjectType("Account");
            api.send(request);
        }

        private void btnTestQuery_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForQuery(txtQuery.Text);
            api.send(request);
        }

        private void btnTestResource_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForResources();
            api.send(request);
        }

        private void btnTestSearch_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            MySFRestRequest request = api.requestForSearch(txtSosl.Text);
            api.send(request);
        }

        private void btnTestUpsert_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFRestAPI api = getApi();
            Dictionary<String, String> fields = new Dictionary<String, String>();
            fields.Add("Website", @"http://www.google.co.jp");
            fields.Add("Industry", "IT");
            MySFRestRequest request = api.requestForUpsertObjectType("Account", "AccountRecNumber__c", "NO-00000001", fields);
            api.send(request);
        }

        private MySFChatterRestAPI getChatterApi()
        {
            MySFChatterRestAPI api = new MySFChatterRestAPI();
            api.apiVersion = @"v28.0";
            MySFChatterRestAPI.coordinator = _coordinator;
            api.onCanceldLoadResponse += sfapi_onCanceldLoadResponse;
            api.onCompletedLoadResponse += sfapi_onCompletedLoadResponse;
            api.onFailedLoadResponse += sfapi_onFailedLoadResponse;
            api.onFailedRetry += sfapi_onFailedRetry;
            api.onTimeoutLoadResponse += sfapi_onTimeoutLoadResponse;
            return api;
        }

        private void btn_TestChatterNewsFeed_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.newsFeed();
        }

        private void btnTestChatterNewsFeedOfUser_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.newsFeed(txtUserIdForNewsFeed.Text);
        }

        private void btnTestUserProfileFeed_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.profileFeed();
        }

        private void btnTestUserPfofileFeedOfUser_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.profileFeed(txtUserIdForProfileFeed.Text);
        }

        private void btnTestFollowingRecordFeed_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.recordFeed();
        }

        private void btnTestRecordFeed_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.recordFeed(txtRcordIdForRecordFeed.Text);
        }

        private void btnTestFeedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.feedItem(txtFeedItemId.Text);
        }

        private void btnTestFeedComments_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.feedComments(txtFeedItemId.Text);
        }

        private void btnTestFeedLikes_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.feedLikes(txtFeedItemId.Text);
        }

        private void btnTestToMeFeed_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.feedToMe();
        }

        private void btnTestToUserFeed_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.feedToUser(txtUserIdForToFeed.Text);
        }

        private void btnTestNewsFeedWithExistingContent_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createNewsFeedAttachingExistingContent(txtExistingContentId.Text, "TEST");
        }

        private void btnTestRecordFeedOfAttachingExistingContent_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createRecordFeedAttachingExistingContent(txtExistingContentId2.Text, "TEST", txtRecordId2.Text);
        }

        private async void btnTestNewsFeedWithNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }

            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null) {
                //showDebugMessage(file.Path);

                Byte[] filebytes = null;
                using (IRandomAccessStream stream = await file.OpenReadAsync()) 
                {
                    using(BinaryReader reader = new BinaryReader(stream.AsStream()))
                    {
                        filebytes = reader.ReadBytes((int)stream.Size);
                    }
                }
                if (filebytes != null) {
                    MySFChatterRestAPI api = getChatterApi();
                    String ext = Path.GetExtension(file.Name);
                    String newfilename = "MobileUpload" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ext;
                    api.createNewsFeedAttachingNewFile(filebytes, newfilename, file.Name, "Test Upload", "This is a test."); 
                }
            }
        }

        private async void btnTestRecordFeedWithNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }

            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                //showDebugMessage(file.Path);

                Byte[] filebytes = null;
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    using (BinaryReader reader = new BinaryReader(stream.AsStream()))
                    {
                        filebytes = reader.ReadBytes((int)stream.Size);
                    }
                }
                if (filebytes != null)
                {
                    MySFChatterRestAPI api = getChatterApi();
                    String ext = Path.GetExtension(file.Name);
                    String newfilename = "MobileUpload" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ext;
                    api.createRecordFeedAttachingNewFile(filebytes, newfilename, file.Name, "Test Upload", "This is a test.", txtRecordId.Text);
                }
            }
        }

        private void btnTestNewsFeedWithLink_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createNewsFeedAttachingLink("http://www.google.co.jp/", "Google", "TEST");
        }

        private void btnTestRecordFeedWithLink_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createRecordFeedAttachingLink("http://www.google.co.jp/", "Google", "TEST", txtRecordId3.Text);
        }

        private void btnTestFeedComment_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createComment("This is a test comment",txtFeedItemId2.Text);
        }

        private void btnTestFeedCommentWithExistingContent_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createCommenAttachingExistingContent(txtContentId3.Text, "This is a test comment", txtFeedItemId3.Text);
        }

        private void btnLikeFeedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }
            MySFChatterRestAPI api = getChatterApi();
            api.createLikeToFeed(txtFeedItemId4.Text);
        }

        private async void btnTestFeedCommentWithNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (_coordinator == null)
            {
                showDebugMessage("先に認証を行ってください");
                return;
            }

            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                //showDebugMessage(file.Path);

                Byte[] filebytes = null;
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    using (BinaryReader reader = new BinaryReader(stream.AsStream()))
                    {
                        filebytes = reader.ReadBytes((int)stream.Size);
                    }
                }
                if (filebytes != null)
                {
                    MySFChatterRestAPI api = getChatterApi();
                    String ext = Path.GetExtension(file.Name);
                    String newfilename = "MobileUpload" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ext;
                    api.createCommenAttachingNewFile(filebytes, newfilename, file.Name, "This is a test upload", "Test comment and upload new file", txtFeedItem5.Text);
                }
            }
        }

        
    }
}
