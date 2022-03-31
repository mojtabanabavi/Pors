using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Questions.Queries
{
    #region query

    public class GetQuestionsQuery : DataTableQuery, IRequest<PagingResult<GetQuestionsQueryResponse>>
    {
        public int ExamId { get; set; }

        public GetQuestionsQuery(DataTableQuery query, int examId) : base(query)
        {
            ExamId = examId;
        }
    }

    #endregion;

    #region response

    public class GetQuestionsQueryResponse : IMapFrom<ExamQuestion>
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string CreatedAt { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetQuestionsQueryHandler : IRequestHandler<GetQuestionsQuery, PagingResult<GetQuestionsQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetQuestionsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetQuestionsQueryResponse>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ExamQuestion> query = _dbContext.ExamQuestions
                .AsNoTracking();

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
                query = query.Where(x => x.Title.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetQuestionsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}