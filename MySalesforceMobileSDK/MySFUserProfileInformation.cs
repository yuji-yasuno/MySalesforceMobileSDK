using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace MySalesforceMobileSDK
{
    public class MySFUserProfileInformation
    {
        public class Address 
        {
            public string city { get; set; }
            public string country { get; set; }
            public string formattedAddress { get; set; }
            public string state { get; set; }
            public string street { get; set; }
            public string zip { get; set; }
        }
        public class ChatterActivity 
        {
            public int commentCount { get; set; }
            public int commentReceivedCount { get; set; }
            public int likeReceivedCount { get; set; }
            public int postCount { get; set; }
        }
        public class ChatterInfluence 
        {
            public string pecentile { get; set; }
            public int rank { get; set; }
        }
        public class FollowingCounts
        {
            public int people { get; set; }
            public int records { get; set; }
            public int total { get; set; }
        }
        public class Motif 
        {
            public string largeIconUrl { get; set; }
            public string mediumIconUrl { get; set; }
            public string smallIconUrl { get; set; }
        }
        public class PhoneNumber 
        {
            public string phoneNumber { get; set; }
            public string type { get; set; }
        }
        public class Photo 
        {
            public string fullEmailPhotoUrl { get; set; }
            public string largePhotoUrl { get; set; }
            public string photoVersionId { get; set; }
            public string smallPhotoUrl { get; set; }
            public string standardEmailPhotoUrl { get; set; }
            public string url { get; set; }
        }

        public string aboutMe { get; set; }
        public Address address { get; set; }
        public ChatterActivity chatterActivity { get; set; }
        public ChatterInfluence chatterInfluence { get; set; }
        public string companyName { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public int followersCount { get; set; }
        public FollowingCounts followingCounts { get; set; }
        public int groupCount { get; set; }
        public string id { get; set; }
        public Boolean isActive { get; set; }
        public Boolean isInThisCommunity { get; set; }
        public string lastName { get; set; }
        public string managerId { get; set; }
        public string managerName { get; set; }
        public Motif motif { get; set; }
        public string mySubscription { get; set; }
        public string name { get; set; }
        public List<PhoneNumber> phoneNumbers { get; set; }
        public Photo photo { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string userType { get; set; }
        public string username { get; set; }

        private MySFUserProfileInformation() { }
        public static MySFUserProfileInformation createInstance(JsonObject json)
        {
            if (json == null) return null;
            return createNewInstance(json);
        }

        private static MySFUserProfileInformation createNewInstance(JsonObject json) 
        {
            IJsonValue tmpvalue = null;
            Boolean isSuccess;
            MySFUserProfileInformation result = new MySFUserProfileInformation();

            isSuccess = json.TryGetValue("aboutMe", out tmpvalue);
            if(isSuccess) result.aboutMe = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;

            isSuccess = json.TryGetValue("address", out tmpvalue);
            JsonObject addressJson = null;
            if(isSuccess) addressJson = tmpvalue.ValueType == JsonValueType.Object ? tmpvalue.GetObject() : null;
            if (addressJson != null) {
                result.address = new Address();
                isSuccess = addressJson.TryGetValue("city", out tmpvalue);
                if(isSuccess) result.address.city = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = addressJson.TryGetValue("country", out tmpvalue);
                if(isSuccess) result.address.country = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = addressJson.TryGetValue("formattedAddress", out tmpvalue);
                if(isSuccess) result.address.formattedAddress = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = addressJson.TryGetValue("state", out tmpvalue);
                if(isSuccess) result.address.state = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = addressJson.TryGetValue("street", out tmpvalue);
                if(isSuccess) result.address.street = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = addressJson.TryGetValue("zip", out tmpvalue);
                if(isSuccess) result.address.zip = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            }

            isSuccess = json.TryGetValue("chatterActivity", out tmpvalue);
            JsonObject chatterActivityJson = null;
            if(isSuccess) chatterActivityJson = tmpvalue.ValueType == JsonValueType.Object ? tmpvalue.GetObject() : null;
            if (chatterActivityJson != null) 
            {
                result.chatterActivity = new ChatterActivity();
                isSuccess = chatterActivityJson.TryGetValue("commentCount", out tmpvalue);
                if(isSuccess) result.chatterActivity.commentCount = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
                isSuccess = chatterActivityJson.TryGetValue("commentReceivedCount", out tmpvalue);
                if(isSuccess) result.chatterActivity.commentReceivedCount = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
                isSuccess = chatterActivityJson.TryGetValue("likeReceivedCount", out tmpvalue);
                if(isSuccess) result.chatterActivity.likeReceivedCount = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
                isSuccess = chatterActivityJson.TryGetValue("postCount", out tmpvalue);
                if(isSuccess) result.chatterActivity.postCount = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
            }

            isSuccess = json.TryGetValue("chatterInfluence", out tmpvalue);
            JsonObject chatterInfluenceJson = null;
            if(isSuccess) chatterInfluenceJson = tmpvalue.ValueType == JsonValueType.Object ? tmpvalue.GetObject() : null;
            if (chatterInfluenceJson != null) 
            {
                result.chatterInfluence = new ChatterInfluence();
                isSuccess = chatterInfluenceJson.TryGetValue("percentile", out tmpvalue);
                if(isSuccess) result.chatterInfluence.pecentile = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = chatterInfluenceJson.TryGetValue("rank", out tmpvalue);
                if(isSuccess) result.chatterInfluence.rank = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
            }

            isSuccess = json.TryGetValue("companyName", out tmpvalue);
            if(isSuccess) result.companyName = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("email", out tmpvalue);
            if(isSuccess) result.email = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("firstName", out tmpvalue);
            if(isSuccess) result.firstName = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("followersCount", out tmpvalue);
            if(isSuccess) result.followersCount = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;

            isSuccess = json.TryGetValue("followingCounts", out tmpvalue);
            JsonObject followingCountJson = null;
            if(isSuccess) followingCountJson = tmpvalue.ValueType == JsonValueType.Object ? tmpvalue.GetObject() : null;
            if (followingCountJson != null)
            {
                result.followingCounts = new FollowingCounts();
                isSuccess = followingCountJson.TryGetValue("people", out tmpvalue);
                if(isSuccess) result.followingCounts.people = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
                isSuccess = followingCountJson.TryGetValue("records", out tmpvalue);
                if(isSuccess) result.followingCounts.records = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
                isSuccess = followingCountJson.TryGetValue("total", out tmpvalue);
                if(isSuccess) result.followingCounts.total = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
            }

            isSuccess = json.TryGetValue("groupCount", out tmpvalue);
            if(isSuccess) result.groupCount = tmpvalue.ValueType == JsonValueType.Number ? (int)tmpvalue.GetNumber() : 0;
            isSuccess = json.TryGetValue("id", out tmpvalue);
            if(isSuccess) result.id = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("isActive", out tmpvalue);
            if(isSuccess) result.isActive = tmpvalue.ValueType == JsonValueType.Boolean ? tmpvalue.GetBoolean() : false;
            isSuccess = json.TryGetValue("isInThisCommunity", out tmpvalue);
            if(isSuccess) result.isInThisCommunity = tmpvalue.ValueType == JsonValueType.Boolean ? tmpvalue.GetBoolean() : false;
            isSuccess = json.TryGetValue("lastName", out tmpvalue);
            if(isSuccess) result.lastName = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("managerId", out tmpvalue);
            if(isSuccess) result.managerId = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("managerName", out tmpvalue);
            if(isSuccess) result.managerName = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;

            isSuccess = json.TryGetValue("motif", out tmpvalue);
            JsonObject motifJson = null;
            if(isSuccess) motifJson = tmpvalue.ValueType == JsonValueType.Object ? tmpvalue.GetObject() : null;
            if (motifJson != null) 
            {
                result.motif = new Motif();
                isSuccess = motifJson.TryGetValue("largeIconUrl", out tmpvalue);
                if(isSuccess) result.motif.largeIconUrl = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = motifJson.TryGetValue("mediumIconUrl", out tmpvalue);
                if(isSuccess) result.motif.mediumIconUrl = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = motifJson.TryGetValue("smallIconUrl", out tmpvalue);
                if(isSuccess) result.motif.smallIconUrl = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            }
            isSuccess = json.TryGetValue("mySubscription", out tmpvalue);
            if(isSuccess) result.mySubscription = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("name", out tmpvalue);
            if(isSuccess) result.name = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;

            isSuccess = json.TryGetValue("phoneNumbers", out tmpvalue);
            JsonArray phoneJsons = null;
            if(isSuccess) phoneJsons = tmpvalue.ValueType == JsonValueType.Array ? tmpvalue.GetArray() : null;
            if (phoneJsons != null) 
            {
                result.phoneNumbers = new List<PhoneNumber>();
                for (uint ii = 0; ii < phoneJsons.Count; ii++) {
                    JsonObject phoneJson = phoneJsons.GetObjectAt(ii);
                    PhoneNumber phone = new PhoneNumber();
                    isSuccess = phoneJson.TryGetValue("phoneNumber", out tmpvalue);
                    if(isSuccess) phone.phoneNumber = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                    isSuccess = phoneJson.TryGetValue("type", out tmpvalue);
                    if (isSuccess) phone.type = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                    result.phoneNumbers.Add(phone);
                }
            }

            isSuccess = json.TryGetValue("photo", out tmpvalue);
            JsonObject photoJson = null;
            if(isSuccess) photoJson = tmpvalue.ValueType == JsonValueType.Object ? tmpvalue.GetObject() : null;
            if (photoJson != null) {
                result.photo = new Photo();
                isSuccess = photoJson.TryGetValue("fullEmailPhotoUrl", out tmpvalue);
                if(isSuccess) result.photo.fullEmailPhotoUrl = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = photoJson.TryGetValue("largePhotoUrl", out tmpvalue);
                if(isSuccess) result.photo.largePhotoUrl = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = photoJson.TryGetValue("photoVersionId", out tmpvalue);
                if(isSuccess) result.photo.photoVersionId = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = photoJson.TryGetValue("smallPhotoUrl", out tmpvalue);
                if(isSuccess) result.photo.smallPhotoUrl = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
                isSuccess = photoJson.TryGetValue("url", out tmpvalue);
                if(isSuccess) result.photo.url = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            }

            isSuccess = json.TryGetValue("title", out tmpvalue);
            if(isSuccess) result.title = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("type", out tmpvalue);
            if(isSuccess) result.type = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("url", out tmpvalue);
            if(isSuccess) result.url = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("userType", out tmpvalue);
            if(isSuccess) result.userType = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            isSuccess = json.TryGetValue("username", out tmpvalue);
            if(isSuccess) result.username = tmpvalue.ValueType == JsonValueType.String ? tmpvalue.GetString() : null;
            return result;
        }
    }
}
