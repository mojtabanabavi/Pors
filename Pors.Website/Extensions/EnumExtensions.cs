using System;
using System.Linq;
using Loby.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pors.Website.Extensions
{
    public static class EnumExtensions
    {
        public static SelectList ToSelectList<T>()
        {
            return ToSelectList<T>(null);
        }

        public static SelectList ToSelectList<T>(object selectedValue)
        {
            var items = Enum.GetValues(typeof(T)).Cast<Enum>()
                  .Select(e => new SelectListItem
                  {
                      Text = e.GetDescription(),
                      Value = (Convert.ToInt32(e)).ToString(),
                  }).ToList();

            return new SelectList(items, "Value", "Text", selectedValue);
        }
    }
}