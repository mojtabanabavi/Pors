using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Permissions.Queries
{
    #region query

    public class GetRolePermissionsQuery : IRequest<GetRolePermissionsQueryResponse>
    {
        public int Id { get; set; }

        public GetRolePermissionsQuery()
        {
        }

        public GetRolePermissionsQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetRolePermissionsQueryResponse : IMapFrom<Role>
    {
        public int Id { get; set; }
        public List<RolePermissionDto> Permissions { get; set; }
    }

    public class RolePermissionDto : IMapFrom<RolePermission>
    {
        public string Action { get; set; }
        public string Controller { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, GetRolePermissionsQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetRolePermissionsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetRolePermissionsQueryResponse> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles
                .Where(x => x.Id == request.Id)
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync();

            _dbContext.RolePermissions
            .Where(x => x.RoleId == request.Id);

            var result = _mapper.Map<GetRolePermissionsQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}
