using System;
using MediatR;
using AutoMapper;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetExamReportQuery : IRequest<GetExamReportQueryResponse>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region response

    public class GetExamReportQueryResponse : IMapFrom<Exam>
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

    public class GetExamReportQueryHandler : IRequestHandler<GetExamReportQuery, GetExamReportQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamReportQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetExamReportQueryResponse> Handle(GetExamReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _dbContext.Exams
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

            var result = new GetExamReportQueryResponse
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
