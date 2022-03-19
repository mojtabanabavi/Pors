using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetQuestionAnswersAccuracyChartDataQuery : IRequest<GetQuestionAnswersAccuracyChartDataQueryResponse>
    {
        public int QuestionId { get; set; }
    }

    #endregion;

    #region response

    public class GetQuestionAnswersAccuracyChartDataQueryResponse
    {
        public List<int> DataSet { get; set; }
        public List<string> Labels { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetQuestionAnswersAccuracyChartDataQueryHandler : IRequestHandler<GetQuestionAnswersAccuracyChartDataQuery, GetQuestionAnswersAccuracyChartDataQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetQuestionAnswersAccuracyChartDataQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetQuestionAnswersAccuracyChartDataQueryResponse> Handle(GetQuestionAnswersAccuracyChartDataQuery request, CancellationToken cancellationToken)
        {
            var report = await _dbContext.AttemptAnswers
                .Where(x => x.Option.QuestionId == request.QuestionId)
                .GroupBy(x => new { x.OptionId, x.Option.Title })
                .Select(x => new
                {
                    Label = x.Key.Title,
                    Count = x.Where(x => x.IsCorrect).Count(),
                })
                .ToListAsync();

            var result = new GetQuestionAnswersAccuracyChartDataQueryResponse
            {
                Labels = report.Select(x => x.Label).ToList(),
                DataSet = report.Select(x => x.Count).ToList(),
            };

            return result;
        }
    }

    #endregion;
}
