﻿using System;
using MediatR;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetExamVisitsChartDataQuery : IRequest<GetExamVisitsChartDataQueryResponse>
    {
        public int ExamId { get; set; }
    }

    #endregion;

    #region response

    public class GetExamVisitsChartDataQueryResponse : ChartJsData
    {
        public GetExamVisitsChartDataQueryResponse()
        {
        }

        public GetExamVisitsChartDataQueryResponse(ChartJsData data)
        {
            Labels = data.Labels;
            DataSets = data.DataSets;
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamVisitsChartDataQueryHandler : IRequestHandler<GetExamVisitsChartDataQuery, GetExamVisitsChartDataQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetExamVisitsChartDataQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetExamVisitsChartDataQueryResponse> Handle(GetExamVisitsChartDataQuery request, CancellationToken cancellationToken)
        {
            var data = await _dbContext.ExamAttempts
                .Where(x => x.ExamId == request.ExamId)
                .GroupBy(x => x.CreatedAt.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    Count = x.Count(),
                })
                .ToListAsync();

            if (data == null)
            {
                throw new NotFoundException(nameof(Exam), request.ExamId);
            }

            var persianCultureDateTime = new CultureInfo("fa-ir").DateTimeFormat;

            var chartData = new ChartJsData()
            {
                Labels = Enumerable.Repeat("", 12).ToList(),
                DataSets = new List<ChartJsDataDataset>()
                {
                    new ChartJsDataDataset
                    {
                        Data = Enumerable.Repeat(0, 12).ToList()
                    }
                },
            };

            for (int i = 1; i <= 12; i++)
            {
                var monthIndex = data.FindIndex(x => x.Month == i);

                chartData.Labels[i - 1] = persianCultureDateTime.GetMonthName(i);

                if (monthIndex != -1)
                {
                    chartData.DataSets[0].Data[monthIndex] = data[monthIndex].Count;
                }
            }

            var result = new GetExamVisitsChartDataQueryResponse(chartData);

            return result;
        }
    }

    #endregion;
}
