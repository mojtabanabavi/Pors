using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
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

namespace Pors.Application.Questions.Queries
{
    #region query

    public class GetQuestionsQuery : IRequest<PagingResult<GetQuestionsQueryResponse>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
    }

    public class GetQuestionsQueryResponse : IMapFrom<ExamQuestion>
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
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
            IQueryable<ExamQuestion> query = _dbContext.ExamQuestions;

            if (request.SortColumn.HasValue() && request.SortColumnDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortColumnDirection}");
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
