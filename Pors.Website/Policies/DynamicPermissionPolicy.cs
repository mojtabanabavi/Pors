using System;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Pors.Application.Management.Identity.Queries;

namespace Pors.Website.Policies
{
    public class DynamicPermissionRequirement : IAuthorizationRequirement
    {
    }

    public class DynamicPermissionHandler : AuthorizationHandler<DynamicPermissionRequirement>
    {
        private readonly ISender Mediator;

        public DynamicPermissionHandler(ISender mediator)
        {
            Mediator = mediator;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicPermissionRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext)
            {
                var endpoint = httpContext.GetEndpoint();
                var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

                var area = actionDescriptor.RouteValues["area"];
                var action = actionDescriptor.RouteValues["action"];
                var controller = actionDescriptor.RouteValues["controller"];

                var canAccess = await Mediator.Send(new CanUserAccessQuery(area, controller, action));

                if (canAccess)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
