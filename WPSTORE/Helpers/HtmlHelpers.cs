using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WPSTORE.Helpers
{
    public static class HtmlHelpers
    {
        public static string RemoveHtmlTag(this string input)
        {
            if(!string.IsNullOrEmpty(input))
                return Regex.Replace(input, @"<.*?>", string.Empty);
            return input;
        }
    }
}
