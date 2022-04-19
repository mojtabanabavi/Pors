using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pors.Application.Common.Models
{
    public class SelectListItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public SelectListItem()
        {
        }

        public SelectListItem(string text, object value)
        {
            Text = text;
            Value = value;
        }
    }
}
