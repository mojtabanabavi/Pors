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
            var items = Enum.GetValues(typeof(T)).Cast<Enum>()
                  .Select(e => new SelectListItem
                  {
                      Value = e.ToString(),
                      Text = e.GetDescription(),
                  }).ToList();

            return new SelectList(items, "Value", "Text");
        }
    }
}