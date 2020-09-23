using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace WPSTORE.Extensions
{
    public static class QueryExtensions
    {
        public static string GetQueryString(this object obj)
        {
            List<string> queryParams = new List<string>();

            var properties = obj.GetType().GetProperties().Where(x => x.GetValue(obj, null) != null).ToList();
            foreach(PropertyInfo property in properties)
            {
                var jsonAttr = property.GetCustomAttribute<JsonPropertyAttribute>();
                if(jsonAttr != null)
                {
                    var propertyType = property.PropertyType;
                    if (propertyType.IsArray)
                    {
                        var arrayValue = property.GetValue(obj, null);
                        if(arrayValue != null)
                        {
                            Array arr = arrayValue as Array;
                            var length = arr.Length;

                            for(int i =0; i <= length - 1; i ++)
                            {
                                var param = $"{jsonAttr.PropertyName.ToLower()}[{i}]={arr.GetValue(i)}";
                                queryParams.Add(param);
                            }
                        }
                    }
                    else
                    {
                        queryParams.Add($"{jsonAttr.PropertyName.ToLower()}={HttpUtility.UrlEncode(property.GetValue(obj, null).ToString())}");
                    }
                }
                else
                {
                    queryParams.Add($"{property.Name.ToLower()}={HttpUtility.UrlEncode(property.GetValue(obj, null).ToString())}");
                }
            }
            return String.Join("&", queryParams.ToArray());
        }
    }
}
