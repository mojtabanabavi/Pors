using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pors.Website.Extensions
{
    public static class HttpResponseExtensions
    {
        public static string GetCookieValue(this IHeaderDictionary headerDictionary, string key)
        {
            string cookieValue = null;

            foreach (var headers in headerDictionary.Values)
            {
                foreach (var header in headers)
                {
                    if (header.StartsWith($"{key}="))
                    {
                        var p1 = header.IndexOf('=');
                        var p2 = header.IndexOf(';');
                        cookieValue = header.Substring(p1 + 1, p2 - p1 - 1);
                    }
                }
            }

            return cookieValue;
        }
    }
}
