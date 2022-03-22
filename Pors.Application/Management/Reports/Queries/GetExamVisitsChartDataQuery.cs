using System;
using MediatR;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Globalization;
using System.Threading.Tasks;
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

    public class GetExamVisitsChartDataQueryResponse : ChartData
    {
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

            var report = new ChartData()
            {
                Labels = Enumerable.Repeat("", 12).ToList(),
                DataSet = Enumerable.Repeat(0, 12).ToList(),
            };

            for (int i = 1; i <= 12; i++)
            {
                var monthIndex = data.FindIndex(x => x.Month == i);

                report.Labels[i - 1] = persianCultureDateTime.GetMonthName(i);

                if (monthIndex != -1)
                {
                    report.DataSet[monthIndex] = data[monthIndex].Count;
                }
            }

            var result = new GetExamVisitsChartDataQueryResponse
            {
                Labels = report.Labels,
                DataSet = report.DataSet
            };

            return result;
        }
    }

    #endregion;
}
