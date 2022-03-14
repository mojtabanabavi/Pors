using System;
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

namespace Pors.Application.Management.Faqs.Queries
{
    #region query

    public class GetFaqsQuery : DataTableQuery, IRequest<PagingResult<GetFaqsQueryResponse>>
    {
        public GetFaqsQuery(DataTableQuery query) : base(query)
        {
        }
    }

    #endregion;

    #region response

    public class GetFaqsQueryResponse : IMapFrom<Faq>
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string CreatedAt { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetFaqsQueryHandler : IRequestHandler<GetFaqsQuery, PagingResult<GetFaqsQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetFaqsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetFaqsQueryResponse>> Handle(GetFaqsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Faq> query = _dbContext.Faqs;

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Question.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetFaqsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
