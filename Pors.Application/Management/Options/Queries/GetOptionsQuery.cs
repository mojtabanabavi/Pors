using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Options.Queries
{
    #region query

    public class GetOptionsQuery : DataTableQuery, IRequest<PagingResult<GetOptionsQueryResponse>>
    {
        public int QuestionId { get; set; }

        public GetOptionsQuery(DataTableQuery query, int questionId) : base(query)
        {
            QuestionId = questionId;
        }
    }

    #endregion;

    #region response

    public class GetOptionsQueryResponse : IMapFrom<QuestionOption>
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string CreatedAt { get; set; }
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
            IQueryable<QuestionOption> query = _dbContext.QuestionOptions
                .AsNoTracking();

            if (request.QuestionId != default(int))
            {
                query = query.Where(x => x.QuestionId == request.QuestionId);
            }

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Id.ToString() == request.Search ||
                                         x.Title.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetOptionsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyDataTablePagingAsync(request.Skip, request.Take);

            return result;
        }
    }

    #endregion;
}
