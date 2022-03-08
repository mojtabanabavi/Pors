using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pors.Website.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active", char delimeter = ',')
        {
            var currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            var acceptedActions = (actions ?? currentAction).Split(delimeter);
            var acceptedControllers = (controllers ?? currentController).Split(delimeter);

            var isActive = acceptedActions.Contains(currentAction, StringComparer.OrdinalIgnoreCase) &&
                           acceptedControllers.Contains(currentController, StringComparer.OrdinalIgnoreCase);

            return isActive ? cssClass : string.Empty;
        }
    }
}
