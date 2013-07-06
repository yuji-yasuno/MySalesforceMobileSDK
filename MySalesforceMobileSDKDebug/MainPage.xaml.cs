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
            MySFRestAPI api = MySFRestAPI.getInstance();
            api.apiVersion = @"v28.0";
            api.coordinator = _coordinator;
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

    }
}
