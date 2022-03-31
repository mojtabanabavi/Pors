using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Users.Queries
{
    #region query

    public class GetUsersQuery : DataTableQuery, IRequest<PagingResult<GetUsersQueryResponse>>
    {
        public GetUsersQuery(DataTableQuery query) : base(query)
        {
        }
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
            IQueryable<User> query = _dbContext.Users
                .AsNoTracking();

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
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