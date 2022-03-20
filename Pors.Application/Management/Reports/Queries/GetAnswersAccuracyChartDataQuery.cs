using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetAnswersAccuracyChartDataQuery : IRequest<GetAnswersAccuracyChartDataQueryResponse>
    {
        public int QuestionId { get; set; }
    }

    #endregion;

    #region response

    public class GetAnswersAccuracyChartDataQueryResponse : ChartData
    {
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetAnswersAccuracyChartDataQueryHandler : IRequestHandler<GetAnswersAccuracyChartDataQuery, GetAnswersAccuracyChartDataQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetAnswersAccuracyChartDataQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAnswersAccuracyChartDataQueryResponse> Handle(GetAnswersAccuracyChartDataQuery request, CancellationToken cancellationToken)
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

            var result = new GetAnswersAccuracyChartDataQueryResponse
            {
                Labels = report.Select(x => x.Label).ToList(),
                DataSet = report.Select(x => x.Count).ToList(),
            };

            return result;
        }
    }

    #endregion;
}
