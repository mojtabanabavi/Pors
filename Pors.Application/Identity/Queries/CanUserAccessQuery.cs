using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Identity.Queries
{
    #region query

    public class CanUserAccessQuery : IRequest<bool>
    {
        public string Area { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }

        public CanUserAccessQuery()
        {
        }

        public CanUserAccessQuery(string area, string controller, string action)
        {
            Area = area;
            Action = action;
            Controller = controller;
        }
    }

    #endregion;

    #region handler

    public class CanUserAccessQueryHandler : IRequestHandler<CanUserAccessQuery, bool>
    {
        private ISqlDbContext _dbContext;
        private ICurrentUserService _currentUser;

        public CanUserAccessQueryHandler(ISqlDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUser = currentUserService;
        }

        public async Task<bool> Handle(CanUserAccessQuery request, CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(_currentUser.UserId);

            if (userId == 1)
            {
                return true;
            }

            var canAccess = await _dbContext.UserRoles
                .Where(x => x.UserId == userId)
                .AnyAsync(x => x.Role.Permissions.Any(x =>
                    x.Controller == request.Controller &&
                    x.Action == request.Action));

            return canAccess;
        }
    }

    #endregion;
}
