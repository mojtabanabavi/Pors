using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
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

namespace Pors.Application.Management.Participants.Queries
{
    #region query

    public class GetParticipantsQuery : DataTableQuery, IRequest<PagingResult<GetParticipantsQueryResponse>>
    {
        public int ExamId { get; set; }

        public GetParticipantsQuery(DataTableQuery query, int examId) : base(query)
        {
            ExamId = examId;
        }
    }

    #endregion;

    #region response

    public class GetParticipantsQueryResponse : IMapFrom<ExamAttempt>
    {
        public string ExamTitle { get; set; }
        public string IpAddress { get; set; }
        public string CreatedAt { get; set; }
        public string ParticipantId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExamAttempt, GetParticipantsQueryResponse>()
                .ForMember(x => x.ExamTitle, option => option.MapFrom(y => y.Exam.Title));
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetParticipantsQueryHandler : IRequestHandler<GetParticipantsQuery, PagingResult<GetParticipantsQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetParticipantsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetParticipantsQueryResponse>> Handle(GetParticipantsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ExamAttempt> query = _dbContext.ExamAttempts
                .Include(x => x.Exam)
                .AsNoTracking();

            if (request.ExamId != default(int))
            {
                query = query.Where(x => x.ExamId == request.ExamId);
            }

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Exam.Title.Contains(request.Search) ||
                                         x.ParticipantId.Contains(request.Search) ||
                                         x.IpAddress.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetParticipantsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyDataTablePagingAsync(request.Skip, request.Take);

            return result;
        }
    }

    #endregion;
}
