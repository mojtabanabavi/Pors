using System;
using System.Linq;
using Loby.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
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
                      Text = e.ToString(),
                      Value = e.GetDescription(),
                  }).ToList();

            return new SelectList(items, "Text", "Value");
        }
    }
}
