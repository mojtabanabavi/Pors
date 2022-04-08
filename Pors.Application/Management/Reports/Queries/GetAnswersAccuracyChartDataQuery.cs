using System;
using MediatR;
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
                .AsNoTracking()
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

            var labels = await _dbContext.QuestionOptions
                .Where(x => x.QuestionId == request.QuestionId)
                .Select(x => x.Title)
                .ToListAsync();

            var correctCountDataSet = new ChartJsDataDataset()
            {
                Label = "صحیح",
                Stack = "accuracy",
                Data = Enumerable.Repeat(0, labels.Count).ToList(),
            };

            var wrongCountDataSet = new ChartJsDataDataset()
            {
                Label = "غلط",
                Stack = "accuracy",
                Data = Enumerable.Repeat(0, labels.Count).ToList(),
            };

            var unknownCountDataSet = new ChartJsDataDataset()
            {
                Label = "نامشخص",
                Stack = "accuracy",
                Data = Enumerable.Repeat(0, labels.Count).ToList(),
            };

            for (int i = 0; i < data.Count; i++)
            {
                var labelIndex = labels.FindIndex(x => x == data[i].Label);

                if (labelIndex != -1)
                {
                    wrongCountDataSet.Data[labelIndex] = data[i].WrongCount;
                    correctCountDataSet.Data[labelIndex] = data[i].CorrectCount;
                    unknownCountDataSet.Data[labelIndex] = data[i].UnknownCount;
                }
            }

            var result = new GetAnswersAccuracyChartDataQueryResponse
            {
                DataSets = new List<ChartJsDataDataset>
                {
                    correctCountDataSet,
                    wrongCountDataSet,
                    unknownCountDataSet
                },
                Labels = labels,
            };

            return result;
        }
    }

    #endregion;
}
