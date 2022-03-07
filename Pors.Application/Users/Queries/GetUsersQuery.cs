﻿using System;
using MediatR;
using AutoMapper;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Users.Queries
{
    #region query

    public class GetUsersQuery : DataTableQuery, IRequest<PagingResult<GetUsersQueryResponse>>
    {
    }

    #endregion;

    #region response

    public class GetUsersQueryResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string RegisterDateTime { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagingResult<GetUsersQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetUsersQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<User> query = _dbContext.Users;

            if (request.SortColumn.HasValue() && request.SortColumnDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortColumnDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x =>
                    x.FirstName.Contains(request.Search) ||
                    x.LastName.Contains(request.Search) ||
                    x.Email.Contains(request.Search)
                );
            }

            var result = await query
                .ProjectTo<GetUsersQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}