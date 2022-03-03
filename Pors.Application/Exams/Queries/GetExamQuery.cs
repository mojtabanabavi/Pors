using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Exams.Queries
{
    #region query

    public class GetExamQuery : IRequest<Result<GetExamQueryResponse>>
    {
        public int Id { get; set; }

        public GetExamQuery()
        {
        }

        public GetExamQuery(int id)
        {
            Id = id;
        }
    }

    public class GetExamQueryResponse : IMapFrom<Exam>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamQueryHandler : IRequestHandler<GetExamQuery, Result<GetExamQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<GetExamQueryResponse>> Handle(GetExamQuery request, CancellationToken cancellationToken)
        {
            var exam = await _dbContext.Exams.FindAsync(request.Id);

            if (exam == null)
            {
                return Result<GetExamQueryResponse>.Failure("آزمون یافت نشد");
            }

            var result = _mapper.Map<GetExamQueryResponse>(exam);

            return Result<GetExamQueryResponse>.Success(result);
        }
    }

    #endregion;
}
