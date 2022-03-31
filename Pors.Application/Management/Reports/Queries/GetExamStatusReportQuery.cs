using System;
using MediatR;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetExamStatusReportQuery : IRequest<GetExamStatusReportQueryResponse>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region response

    public class GetExamStatusReportQueryResponse
    {
        public int ExamId { get; set; }
        public int TotalOptionsCount { get; set; }
        public int TotalAnswersCount { get; set; }
        public int TotalAttemptsCount { get; set; }
        public int TotalQuestionsCount { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamStatusReportQueryHandler : IRequestHandler<GetExamStatusReportQuery, GetExamStatusReportQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetExamStatusReportQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetExamStatusReportQueryResponse> Handle(GetExamStatusReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _dbContext.Exams
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Attempts)
                .ThenInclude(x => x.Answers)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Select(x => new
                {
                    TotalAttemptsCount = x.Attempts.Count(),
                    TotalQuestionsCount = x.Questions.Count(),
                    TotalAnswersCount = x.Attempts.Select(x => x.Answers).Count(),
                    TotalOptionsCount = x.Questions.Select(x => x.Options).Count(),
                })
                .SingleOrDefaultAsync();

            if (report == null)
            {
                throw new NotFoundException(nameof(Exam), request.Id);
            }

            var result = new GetExamStatusReportQueryResponse
            {
                ExamId = request.Id,
                TotalAnswersCount = report.TotalAnswersCount,
                TotalOptionsCount = report.TotalOptionsCount,
                TotalAttemptsCount = report.TotalAttemptsCount,
                TotalQuestionsCount = report.TotalQuestionsCount,
            };

            return result;
        }
    }

    #endregion;
}
