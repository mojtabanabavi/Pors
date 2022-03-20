using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;
using Pors.Domain.Entities;

namespace Pors.Application.Management.Reports.Queries
{
    #region query

    public class GetAnswersChartDataQuery : IRequest<GetAnswersChartDataQueryResponse>
    {
        public int QuestionId { get; set; }
    }

    #endregion;

    #region response

    public class GetAnswersChartDataQueryResponse : ChartData
    {
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
            var report = await _dbContext.AttemptAnswers
                .Where(x => x.Option.QuestionId == request.QuestionId)
                .GroupBy(x => new { x.OptionId, x.Option.Title })
                .Select(x => new
                {
                    Count = x.Count(),
                    Label = x.Key.Title,
                })
                .ToListAsync();

            if (report == null)
            {
                throw new NotFoundException(nameof(ExamQuestion), request.QuestionId);
            }

            var result = new GetAnswersChartDataQueryResponse
            {
                Labels = report.Select(x => x.Label).ToList(),
                DataSet = report.Select(x => x.Count).ToList(),
            };

            return result;
        }
    }

    #endregion;
}
