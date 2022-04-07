using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.Faqs.Queries
{
    #region query

    public class GetFaqsQuery : PagingRequest, IRequest<PagingResult<GetFaqsQueryResponse>>
    {
        public GetFaqsQuery(int page = 1, int pageSize = 10)
        {
            Page = page;
            PageSize = pageSize;
        }
    }

    #endregion;

    #region response

    public class GetFaqsQueryResponse : IMapFrom<Faq>
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
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
            IQueryable<Faq> query = _dbContext.Faqs
                .OrderByDescending(x=> x.CreatedAt)
                .AsNoTracking();

            var result = await query
                .ProjectTo<GetFaqsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
