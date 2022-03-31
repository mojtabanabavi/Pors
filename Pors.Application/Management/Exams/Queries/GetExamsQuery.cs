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

namespace Pors.Application.Management.Exams.Queries
{
    #region query

    public class GetExamsQuery : DataTableQuery, IRequest<PagingResult<GetExamsQueryResponse>>
    {
        public GetExamsQuery(DataTableQuery query) : base(query)
        {
        }
    }

    #endregion;

    #region response

    public class GetExamsQueryResponse : IMapFrom<Exam>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exam, GetExamsQueryResponse>()
                .ForMember(x => x.CreatedBy, option => option.MapFrom(y => y.User.Email))
                .ForMember(x => x.Status, option => option.MapFrom(y => y.Status.GetDescription()));
        }
    }

    #endregion;

    #region validator

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
                .Include(x=> x.User);

            if (request.SortColumn.HasValue() && request.SortDirection.HasValue())
            {
                query = query.OrderBy($"{request.SortColumn} {request.SortDirection}");
            }

            if (request.Search.HasValue())
            {
                query = query.Where(x => x.Title.Contains(request.Search));
            }

            var result = await query
                .ProjectTo<GetExamsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
