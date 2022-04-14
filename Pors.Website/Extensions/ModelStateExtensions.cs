using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pors.Website.Extensions
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary AddErrors(this ModelStateDictionary modelState, IEnumerable<string> errors)
        {
            foreach (var error in errors)
                modelState.AddModelError(string.Empty, error);

            return modelState;
        }

        public static IEnumerable<string> GetErrorMessages(this ModelStateDictionary modelState)
        {
            return modelState.SelectMany(x => x.Value.Errors.Select(x => x.ErrorMessage));
        }
    }
}
