using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using Loby.Extensions;
using System.Threading;
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

namespace Pors.Application.Options.Queries
{
    #region query

    public class GetOptionsQuery : IRequest<PagingResult<GetOptionsQueryResponse>>
    {
        public int Id { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; }
    }

    public class GetOptionsQueryResponse : IMapFrom<QuestionOption>
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetOptionsQueryHandler : IRequestHandler<GetOptionsQuery, PagingResult<GetOptionsQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetOptionsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetOptionsQueryResponse>> Handle(GetOptionsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<QuestionOption> query = _dbContext.QuestionOptions;

            if (request.Id != default(int))
            {
                query = query.Where(x => x.QuestionId == request.Id);
            }

            if (request.SortColumn.HasValue() && request.SortColumnDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortColumnDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Title.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetOptionsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
