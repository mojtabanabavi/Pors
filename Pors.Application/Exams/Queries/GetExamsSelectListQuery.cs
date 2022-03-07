using System;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Exams.Queries
{
    #region query

    public class GetExamsSelectListQuery : IRequest<GetExamsSelectListQueryResponse>
    {
    }

    #endregion;

    #region response

    public class GetExamsSelectListQueryResponse
    {
        public List<SelectListItem> Items { get; set; }

        public GetExamsSelectListQueryResponse(List<SelectListItem> items)
        {
            Items = items ?? new List<SelectListItem>();
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamsSelectListQueryHandler : IRequestHandler<GetExamsSelectListQuery, GetExamsSelectListQueryResponse>
    {
        private readonly ISqlDbContext _dbContext;

        public GetExamsSelectListQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetExamsSelectListQueryResponse> Handle(GetExamsSelectListQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Exams
                .Select(x => new SelectListItem(x.Id.ToString(), x.Title))
                .ToListAsync();

            return new GetExamsSelectListQueryResponse(result);
        }
    }

    #endregion;
}
