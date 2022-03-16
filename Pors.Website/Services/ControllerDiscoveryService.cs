using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pors.Application.Common.Models;
using System.Runtime.CompilerServices;
using Pors.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Pors.Website.Services
{
    public class ControllerDiscoveryService : IControllerDiscoveryService
    {
        private List<ControllerInfo> _controllers;
        private List<ControllerInfo> _securedControllers;
        private IActionDescriptorCollectionProvider _discoveryProvider;

        public ControllerDiscoveryService(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _discoveryProvider = actionDescriptorCollectionProvider;
        }

        public List<ControllerInfo> DiscoverControllers()
        {
            if (_controllers != null)
            {
                return _controllers;
            }

            _controllers = new();
            var lastControllerName = string.Empty;
            ControllerInfo currentController = null;
            var actionDescriptors = _discoveryProvider.ActionDescriptors.Items;

            foreach (var actionDescriptor in actionDescriptors)
            {
                if (!(actionDescriptor is ControllerActionDescriptor descriptor))
                {
                    continue;
                }

                var actionMethodInfo = descriptor.MethodInfo;
                var controllerTypeInfo = descriptor.ControllerTypeInfo;

                if (lastControllerName != descriptor.ControllerName)
                {
                    currentController = new ControllerInfo
                    {
                        Actions = new List<ActionInfo>(),
                        Name = descriptor.ControllerName.ToLower(),
                        Attributes = GetAttributes(controllerTypeInfo),
                        IsSecured = IsSecured(controllerTypeInfo, actionMethodInfo),
                        AreaName = controllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue.ToLower(),
                        DisplayName = controllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? descriptor.ControllerName,
                    };

                    _controllers.Add(currentController);

                    lastControllerName = descriptor.ControllerName;
                }

                var currentAction = new ActionInfo
                {
                    ControllerId = currentController.Id,
                    Name = descriptor.ActionName.ToLower(),
                    Attributes = GetAttributes(actionMethodInfo),
                    IsSecured = IsSecured(controllerTypeInfo, actionMethodInfo),
                    DisplayName = actionMethodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? descriptor.ActionName,
                };

                // Distinc duplicated actions by id
                if (currentController != null && !currentController.Actions.Any(x => x.Id == currentAction.Id))
                {
                    currentController.Actions.Add(currentAction);
                }
            }

            return _controllers.Where(x => x.Actions.Any()).ToList();
        }

        public List<ControllerInfo> DiscoverSecuredControllers()
        {
            if (_securedControllers != null)
            {
                return _securedControllers;
            }

            if (_controllers == null)
            {
                _controllers = DiscoverControllers();
            }

            _securedControllers = new();

            foreach (var controller in _controllers)
            {
                if (controller.IsSecured)
                {
                    controller.Actions = controller.Actions.Where(x => x.IsSecured).ToList();

                    _securedControllers.Add(controller);
                }
            }

            return _securedControllers.Distinct().ToList();
        }

        private List<Attribute> GetAttributes(MemberInfo actionMemberInfo)
        {
            var attributes = actionMemberInfo
                .GetCustomAttributes(inherit: true)
                .Where(x =>
                {
                    var attributeNamespace = x.GetType().Namespace;

                    var result = attributeNamespace != typeof(CompilerGeneratedAttribute).Namespace &&
                                 attributeNamespace != typeof(DebuggerStepThroughAttribute).Namespace;

                    return result;
                });

            return attributes.Cast<Attribute>().ToList();
        }

        private bool IsSecured(MemberInfo controllerTypeInfo, MemberInfo actionMethodInfo)
        {
            var controllerAllowAnonymousAttribute = controllerTypeInfo.GetCustomAttribute<AllowAnonymousAttribute>(inherit: true);

            if (controllerAllowAnonymousAttribute != null)
            {
                return false;
            }

            var actionAllowAnonymousAttribute = actionMethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(inherit: true);

            if (actionAllowAnonymousAttribute != null)
            {
                return false;
            }

            var controllerAuthorizeAttribute = controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(inherit: true);

            if (controllerAuthorizeAttribute != null)
            {
                return true;
            }

            var actionAuthorizeAttribute = actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>(inherit: true);

            if (actionAuthorizeAttribute != null)
            {
                return true;
            }

            return false;
        }
    }
}