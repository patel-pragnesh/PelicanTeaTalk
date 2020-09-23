using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPSTORE.Models
{
    public class BaseNotificationResponseModel
    {
        public int respond { get; set; }
        public Paging paging { get; set; }
        public string message { get; set; }
    }
    public class NotificationResponseModel : BaseNotificationResponseModel
    {
        
        [JsonProperty("result")]
        public NotificationItem[] Notifications { get; set; }

        public bool HasData => Notifications != null && Notifications.Any();
    }
    public class Paging
    {
        public int stillmore { get; set; }
        public int perpage { get; set; }
        public int callpage { get; set; }
        public int next { get; set; }
        public int previous { get; set; }
        public int pages { get; set; }
        public string result { get; set; }
        public int page { get; set; }
    }
    public class NotificationItem
    {
        public string id { get; set; }
        public string message { get; set; }
        public string starttime { get; set; }
        public string mobtitle { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string icon { get; set; }
        public bool renotify { get; set; }
        public object[] actions { get; set; }
        public bool post_id { get; set; }
        public bool post_type { get; set; }
        public string direction { get; set; }
        public object[] vibrate { get; set; }
        public int silent { get; set; }
        public string bigimage { get; set; }
        public string badge { get; set; }
        public string sound { get; set; }
        public string requireInteraction { get; set; }
    }
}
