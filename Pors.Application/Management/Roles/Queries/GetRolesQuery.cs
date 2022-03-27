﻿using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Roles.Queries
{
    #region query

    public class GetRolesQuery : DataTableQuery, IRequest<PagingResult<GetRolesQueryResponse>>
    {
        public GetRolesQuery(DataTableQuery query) : base(query)
        {
        }
    }

    #endregion;

    #region response

    public class GetRolesQueryResponse : IMapFrom<Role>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
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

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Title.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetRolesQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}