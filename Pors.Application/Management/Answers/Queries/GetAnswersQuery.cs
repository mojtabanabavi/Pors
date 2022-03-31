using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Answers.Queries
{
    #region query

    public class GetAnswersQuery : DataTableQuery, IRequest<PagingResult<GetAnswersQueryResponse>>
    {
        public int QuestionId { get; set; }

        public GetAnswersQuery(DataTableQuery query, int questionId) : base(query)
        {
            QuestionId = questionId;
        }
    }

    #endregion;

    #region response

    public class GetAnswersQueryResponse : IMapFrom<AttemptAnswer>
    {
        public int Id { get; set; }
        public string AttemptId { get; set; }
        public int OptionId { get; set; }
        public bool IsCorrect { get; set; }
        public bool HasDescription { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AttemptAnswer, GetAnswersQueryResponse>()
                .ForMember(x => x.HasDescription, option => option.MapFrom(y => y.Description != null));
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetAnswersQueryHandler : IRequestHandler<GetAnswersQuery, PagingResult<GetAnswersQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetAnswersQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetAnswersQueryResponse>> Handle(GetAnswersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<AttemptAnswer> query = _dbContext.AttemptAnswers
                .AsNoTracking();

            if (request.QuestionId != default(int))
            {
                query = query.Where(x => x.Option.QuestionId == request.QuestionId);
            }

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.AttemptId.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetAnswersQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
