using System;
using MediatR;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Questions.Queries
{
    #region query

    public class GetQuestionsSelectListQuery : IRequest<GetQuestionsSelectListQueryResponse>
    {
        public int ExamId { get; set; }

        public GetQuestionsSelectListQuery()
        {
        }

        public GetQuestionsSelectListQuery(int examId)
        {
            ExamId = examId;
        }
    }

    #endregion;

    #region response

    public class GetQuestionsSelectListQueryResponse
    {
        public List<SelectListItem> Items { get; set; }

        public GetQuestionsSelectListQueryResponse(List<SelectListItem> items)
        {
            Items = items ?? new List<SelectListItem>();
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetQuestionsSelectListQueryHandler : IRequestHandler<GetQuestionsSelectListQuery, GetQuestionsSelectListQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetQuestionsSelectListQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetQuestionsSelectListQueryResponse> Handle(GetQuestionsSelectListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ExamQuestion> query = _dbContext.ExamQuestions;

            if (request.ExamId != default(int))
            {
                query = query.Where(x => x.ExamId == request.ExamId);
            }

            var result = await query
                .Select(x => new SelectListItem(x.Title, x.Id.ToString()))
                .ToListAsync();

            return new GetQuestionsSelectListQueryResponse(result);
        }
    }

    #endregion;
}
