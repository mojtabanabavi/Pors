using System;
using MediatR;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Public.Reports.Queries
{
    #region query

    public class GetAnswersChartDataQuery : IRequest<GetAnswersChartDataQueryResponse>
    {
        public int QuestionId { get; set; }
    }

    #endregion;

    #region response

    public class GetAnswersChartDataQueryResponse : ChartJsData
    {
        public GetAnswersChartDataQueryResponse()
        {
        }

        public GetAnswersChartDataQueryResponse(ChartJsData data)
        {
            Labels = data.Labels;
            DataSets = data.DataSets;
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetAnswersChartDataQueryHandler : IRequestHandler<GetAnswersChartDataQuery, GetAnswersChartDataQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetAnswersChartDataQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAnswersChartDataQueryResponse> Handle(GetAnswersChartDataQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.AttemptAnswers
                .AsNoTracking()
                .Where(x => x.Option.QuestionId == request.QuestionId)
                .GroupBy(x => new { x.OptionId, x.Option.Title })
                .Select(x => new
                {
                    Count = x.Count(),
                    Label = x.Key.Title,
                })
                .ToListAsync();

            if (data == null)
            {
                throw new NotFoundException(nameof(ExamQuestion), request.QuestionId);
            }

            var labels = await _dbContext.QuestionOptions
                .Where(x => x.QuestionId == request.QuestionId)
                .Select(x => x.Title)
                .ToListAsync();

            var chartData = new GetAnswersChartDataQueryResponse
            {
                Labels = labels,
                DataSets = new List<ChartJsDataDataset>()
                {
                    new ChartJsDataDataset
                    {
                        Data = Enumerable.Repeat(0, labels.Count).ToList()
                    }
                },
            };

            for (int i = 0; i < data.Count; i++)
            {
                var labelIndex = labels.FindIndex(x => x == data[i].Label);

                if (labelIndex != -1)
                {
                    chartData.DataSets[0].Data[labelIndex] = data[i].Count;
                }
            }

            return new GetAnswersChartDataQueryResponse(chartData);
        }
    }

    #endregion;
}
