﻿using System;
using MediatR;
using AutoMapper;
using System.Linq;
using Loby.Extensions;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.Exams.Queries
{
    #region query

    public class GetExamsQuery : IRequest<PagingResult<GetExamsQueryResponse>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetExamsQuery(int page = 1, int pageSize = 10)
        {
            Page = page;
            PageSize = pageSize;
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
        public string CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exam, GetExamsQueryResponse>()
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
            IQueryable<Exam> query = _dbContext.Exams;

            var result = await query
                .ProjectTo<GetExamsQueryResponse>(_mapper.ConfigurationProvider)
                .ApplyPagingAsync(request.Page, request.PageSize);

            return result;
        }
    }

    #endregion;
}
