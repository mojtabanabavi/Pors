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

    public class GetQuestionAnswersChartDataQuery : IRequest<GetQuestionAnswersChartDataQueryResponse>
    {
        public int QuestionId { get; set; }
    }

    #endregion;

    #region response

    public class GetQuestionAnswersChartDataQueryResponse
    {
        public List<int> DataSet { get; set; }
        public List<string> Labels { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetQuestionAnswersChartDataQueryHandler : IRequestHandler<GetQuestionAnswersChartDataQuery, GetQuestionAnswersChartDataQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetQuestionAnswersChartDataQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetQuestionAnswersChartDataQueryResponse> Handle(GetQuestionAnswersChartDataQuery request, CancellationToken cancellationToken)
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

            //var reports = await _dbContext.QuestionOptions
            //    .Where(x => x.QuestionId == request.QuestionId)
            //    .GroupBy(o => new { o.Id, o.Title })
            //    .Select(x => new
            //    {
            //        Label = x.Key.Title,
            //        Count = _dbContext.AttemptAnswers.Where(x => x.OptionId == x.Id).Count(),
            //    })
            //    .ToListAsync();

            var result = new GetQuestionAnswersChartDataQueryResponse
            {
                Labels = report.Select(x => x.Label).ToList(),
                DataSet = report.Select(x => x.Count).ToList(),
            };

            return result;
        }
    }

    #endregion;
}
