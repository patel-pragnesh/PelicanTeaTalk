using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.Helpers
{
    public static class QueryStringHelpers
    {
        public static string GetQueryParam(object data)
        {
            return Uri.EscapeDataString(JsonConvert.SerializeObject(data));
        }
        public static TData GetData<TData>(string queryParam)
        {
            var data = Uri.UnescapeDataString(queryParam);
            return JsonConvert.DeserializeObject<TData>(data);
        }
    }
}
