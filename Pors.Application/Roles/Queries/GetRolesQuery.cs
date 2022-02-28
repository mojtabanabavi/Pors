using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Roles.Queries
{
    #region query

    public class GetRolesQuery : IRequest<PagingResult<GetRolesQueryResponse>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
    }

    public class GetRolesQueryResponse : IMapFrom<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, PagingResult<GetRolesQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetRolesQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetRolesQueryResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Role> query = _dbContext.Roles;

            if (request.SortColumn.HasValue() && request.SortColumnDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortColumnDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Name.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetRolesQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
