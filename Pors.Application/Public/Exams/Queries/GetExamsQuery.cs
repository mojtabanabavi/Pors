using System;
using MediatR;
using AutoMapper;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using System.Threading;
using Pors.Domain.Enums;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Enums;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.Exams.Queries
{
    #region query

    public class GetExamsQuery : PagingRequest, IRequest<PagingResult<GetExamsQueryResponse>>
    {
        public string Title { get; set; }
        public ExamSortTypes SortType { get; set; }

        public GetExamsQuery() : this(1, 6)
        {
        }

        public GetExamsQuery(int page = 1, int pageSize = 6)
        {
            Page = page;
            PageSize = pageSize;
        }

        public GetExamsQuery(ExamSortTypes sortType, int page = 1, int pageSize = 6) : this(page, pageSize)
        {
            SortType = sortType;
        }
    }

    #endregion;

    #region response

    public class GetExamsQueryResponse : IMapFrom<Exam>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public int VisitCount { get; set; }
        public string CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exam, GetExamsQueryResponse>()
                .ForMember(x => x.VisitCount, option => option.MapFrom(y => y.Attempts.Count()))
                .ForMember(x => x.Status, option => option.MapFrom(y => y.Status.GetDescription()));
        }
    }

    #endregion;

    #region handler

    public class GetExamsQueryHandler : IRequestHandler<GetExamsQuery, PagingResult<GetExamsQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<PagingResult<GetExamsQueryResponse>> Handle(GetExamsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Exam> query = _dbContext.Exams
                .AsNoTracking()
                .Where(x=> x.Status == ExamStatus.Active && 
                           x.Questions.Any() &&
                           x.Questions.Select(x=> x.Options).Any())
                .Include(x => x.Attempts);

            if (request.Title.HasValue())
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }

            if (request.SortType == ExamSortTypes.Newest)
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }
            else if (request.SortType == ExamSortTypes.MostVisited)
            {
                query = query.OrderByDescending(x => x.Attempts.Count);
            }

            var result = await query
                .ProjectTo<GetExamsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
