using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coach.Entities.Notification
{
    public class NotificationModel
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("isAndroiodDevice")]
        public bool IsAndroiodDevice { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("EntityId")]
        public int? EntityTypeId { get; set; }
        [JsonProperty("EntityTypeId")]
        public int? EntityId { get; set; }
    }

    public class GoogleNotification
    {
        public class DataPayload
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            [JsonProperty("body")]
            public string Body { get; set; }
            [JsonProperty("EntityTypeId")]
            public int? EntityTypeId { get; set; }
            [JsonProperty("EntityId")]

            public int? EntityId { get; set; }

        }
        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";
        [JsonProperty("data")]
        public DataPayload Data { get; set; }
        [JsonProperty("notification")]
        public DataPayload Notification { get; set; }
    }
}
