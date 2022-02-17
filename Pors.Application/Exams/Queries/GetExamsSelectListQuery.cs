using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Exams.Queries
{
    #region query

    public class GetExamsSelectListQuery : IRequest<GetExamsSelectListQueryResponse>
    {
    }

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
