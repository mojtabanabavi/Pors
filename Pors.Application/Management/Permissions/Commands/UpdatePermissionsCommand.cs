using System;
using MediatR;
using System.Linq;
using Loby.Extensions;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Permissions.Commands
{
    #region command

    public class UpdatePermissionsCommand : IRequest
    {
        public int RoleId { get; set; }
        public List<string> PermissionIds { get; set; }
    }

    #endregion;

    #region handler

    public class UpdatePermissionsCommandHandler : IRequestHandler<UpdatePermissionsCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdatePermissionsCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles
                .Where(x => x.Id == request.RoleId)
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Role), request.RoleId);
            }

            entity.Permissions = CreateRolePermissions(request.PermissionIds);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }

        public List<RolePermission> CreateRolePermissions(List<string> permissionIds)
        {
            var rolePermissions = new List<RolePermission>();

            if (permissionIds.IsNullOrEmpty())
            {
                return rolePermissions;
            }

            foreach (var permissionId in permissionIds)
            {
                var splitedPermissionId = permissionId.Split(":");

                if (splitedPermissionId.Length == 3)
                {
                    var action = splitedPermissionId[2];
                    var controller = splitedPermissionId[1];

                    rolePermissions.Add(new RolePermission(controller, action));
                }
            }

            return rolePermissions;
        }
    }

    #endregion;
}
