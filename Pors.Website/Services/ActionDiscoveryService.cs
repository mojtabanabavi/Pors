using System.Linq;
using System.Reflection;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Website.Services
{
    public class ActionDiscoveryService : IActionDiscoveryService
    {
        public List<ActionDiscoveryResult> DiscoverActions()
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(x => !((x.Attributes & MethodAttributes.Virtual) != 0 && (x.Attributes & MethodAttributes.NewSlot) == 0))
                .Select(x => new ActionDiscoveryResult
                {
                    Action = x.Name,
                    Controller = x.DeclaringType.Name,
                    ActionDisplayName = x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? x.Name,
                    Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute)).Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault()
                })
                .ToList();
        }
    }
}
