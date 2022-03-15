using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Roles.Queries
{
    #region query

    public class GetRolesSelectListQuery : IRequest<GetRolesSelectListQueryResponse>
    {
    }

    #endregion;

    #region response

    public class GetRolesSelectListQueryResponse
    {
        public List<SelectListItem> Items { get; set; }

        public GetRolesSelectListQueryResponse(List<SelectListItem> items)
        {
            Items = items ?? new List<SelectListItem>();
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetRolesSelectListQueryHandler : IRequestHandler<GetRolesSelectListQuery, GetRolesSelectListQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetRolesSelectListQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetRolesSelectListQueryResponse> Handle(GetRolesSelectListQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Roles
                .Select(x => new SelectListItem(x.Title, x.Id.ToString()))
                .ToListAsync();

            return new GetRolesSelectListQueryResponse(result);
        }
    }

    #endregion;
}
