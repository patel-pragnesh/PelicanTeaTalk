using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Models.Users
{
    public class UserInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string slug { get; set; }
        public Avatar_Urls avatar_urls { get; set; }
        public object[] meta { get; set; }
        public _Links _links { get; set; }
        public class Avatar_Urls
        {
            [JsonProperty("24")]
            public string Avatar24 { get; set; }
            [JsonProperty("48")]
            public string Avatar48 { get; set; }
            [JsonProperty("96")]
            public string Avatar96 { get; set; }
        }

        public class _Links
        {
            public Self[] self { get; set; }
            public Collection[] collection { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Collection
        {
            public string href { get; set; }
        }
    }
}
