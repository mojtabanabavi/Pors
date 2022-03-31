using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Pors.Domain.Enums;
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

    public class GetAnswersAccuracyChartDataQueryResponse : ChartJsData
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
            var data = await _dbContext.AttemptAnswers
                .Where(x => x.Option.QuestionId == request.QuestionId)
                .GroupBy(x => new { x.OptionId, x.Option.Title })
                .Select(x => new
                {
                    Label = x.Key.Title,
                    WrongCount = x.Where(x => x.Status == AnswerStatus.Wrong).Count(),
                    CorrectCount = x.Where(x => x.Status == AnswerStatus.Correct).Count(),
                    UnknownCount = x.Where(x => x.Status == AnswerStatus.Unknown).Count(),
                })
                .ToListAsync();

            var correctCountDataSet = new ChartJsDataDataset()
            {
                Stack = "accuracy",
                Label = "صحیح",
            };

            var wrongCountDataSet = new ChartJsDataDataset()
            {
                Stack = "accuracy",
                Label = "غلط",
            };

            var unknownCountDataSet = new ChartJsDataDataset()
            {
                Stack = "accuracy",
                Label = "نامشخص",
            };

            foreach (var group in data.GroupBy(x=> x.Label))
            {
                wrongCountDataSet.Data.Add(group.First().WrongCount);
                correctCountDataSet.Data.Add(group.First().CorrectCount);
                unknownCountDataSet.Data.Add(group.First().UnknownCount);
            }

            var result = new GetAnswersAccuracyChartDataQueryResponse
            {
                DataSets = new List<ChartJsDataDataset>
                {
                    correctCountDataSet,
                    wrongCountDataSet,
                    unknownCountDataSet
                },
                Labels = data.Select(x => x.Label).ToList(),
            };

            return result;
        }
    }

    #endregion;
}
