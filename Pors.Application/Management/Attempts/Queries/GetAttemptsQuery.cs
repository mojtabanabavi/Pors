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

namespace Pors.Application.Management.Attempts.Queries
{
    #region query

    public class GetAttemptsQuery : DataTableQuery, IRequest<PagingResult<GetAttemptsQueryResponse>>
    {
        public int ExamId { get; set; }

        public GetAttemptsQuery(DataTableQuery query, int examId) : base(query)
        {
            ExamId = examId;
        }
    }

    #endregion;

    #region response

    public class GetAttemptsQueryResponse : IMapFrom<ExamAttempt>
    {
        public string Id { get; set; }
        public int ExamId { get; set; }
        public string IpAddress { get; set; }
        public string CreatedAt { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetAttemptsQueryHandler : IRequestHandler<GetAttemptsQuery, PagingResult<GetAttemptsQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetAttemptsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetAttemptsQueryResponse>> Handle(GetAttemptsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ExamAttempt> query = _dbContext.ExamAttempts;

            if (request.ExamId != default(int))
            {
                query = query.Where(x => x.ExamId == request.ExamId);
            }

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Id.Contains(request.Search) ||
                                         x.IpAddress.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetAttemptsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
