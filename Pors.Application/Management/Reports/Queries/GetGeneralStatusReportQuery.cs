using System;
using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetGeneralStatusReportQuery : IRequest<GetGeneralStatusReportQueryResponse>
    {
    }

    #endregion;

    #region response

    public class GetGeneralStatusReportQueryResponse
    {
        public StatusReportDto UsersStatus { get; set; }
        public StatusReportDto RolesStatus { get; set; }
        public StatusReportDto ExamsStatus { get; set; }
        public StatusReportDto QuestionsStatus { get; set; }
        public StatusReportDto OptionsStatus { get; set; }
        public StatusReportDto AnswersStatus { get; set; }
        public StatusReportDto FaqsStatus { get; set; }
        public StatusReportDto AttemptsStatus { get; set; }
    }

    public class StatusReportDto
    {
        public int TotalCount { get; set; }
    }

    #endregion;

    #region handler

    public class GetGeneralStatusReportQueryHandler : IRequestHandler<GetGeneralStatusReportQuery, GetGeneralStatusReportQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetGeneralStatusReportQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetGeneralStatusReportQueryResponse> Handle(GetGeneralStatusReportQuery request, CancellationToken cancellationToken)
        {
            var result = new GetGeneralStatusReportQueryResponse
            {
                UsersStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.Users.CountAsync(),
                },
                RolesStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.Roles.CountAsync(),
                },
                ExamsStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.Exams.CountAsync(),
                },
                QuestionsStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.ExamQuestions.CountAsync(),
                },
                OptionsStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.QuestionOptions.CountAsync(),
                },
                AnswersStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.AttemptAnswers.CountAsync(),
                },
                FaqsStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.Faqs.CountAsync(),
                },
                AttemptsStatus = new StatusReportDto
                {
                    TotalCount = await _dbContext.ExamAttempts.CountAsync(),
                },
            };

            return result;
        }
    }

    #endregion;
}
