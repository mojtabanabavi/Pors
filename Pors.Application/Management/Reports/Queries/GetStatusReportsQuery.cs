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

    public class GetStatusReportsQuery : IRequest<GetStatusReportsQueryResponse>
    {
    }

    #endregion;

    #region response

    public class GetStatusReportsQueryResponse
    {
        public StatusReportDto UsersStatus { get; set; }
        public StatusReportDto RolesStatus { get; set; }
        public StatusReportDto ExamsStatus { get; set; }
        public StatusReportDto QuestionsStatus { get; set; }
        public StatusReportDto OptionsStatus { get; set; }
        public StatusReportDto AnswersStatus { get; set; }
    }

    public class StatusReportDto
    {
        public int TotalCount { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetStatusReportsQueryHandler : IRequestHandler<GetStatusReportsQuery, GetStatusReportsQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetStatusReportsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetStatusReportsQueryResponse> Handle(GetStatusReportsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetStatusReportsQueryResponse
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
            };

            return result;
        }
    }

    #endregion;
}
