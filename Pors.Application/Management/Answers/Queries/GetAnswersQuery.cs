﻿using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Enums;
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
        public string ParticipantId { get; set; }

        public GetAnswersQuery(DataTableQuery query, int questionId, string participantId) : base(query)
        {
            QuestionId = questionId;
            ParticipantId = participantId;
        }
    }

    #endregion;

    #region response

    public class GetAnswersQueryResponse : IMapFrom<AttemptAnswer>
    {
        public int Id { get; set; }
        public string AttemptId { get; set; }
        public int OptionId { get; set; }
        public AnswerStatus Status { get; set; }
    }

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

            if (request.ParticipantId.HasValue())
            {
                query = query.Where(x => x.Attempt.ParticipantId == request.ParticipantId);
            }

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Id.ToString() == request.Search ||
                                         x.AttemptId.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetAnswersQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyDataTablePagingAsync(request.Skip, request.Take);

            return result;
        }
    }

    #endregion;
}
