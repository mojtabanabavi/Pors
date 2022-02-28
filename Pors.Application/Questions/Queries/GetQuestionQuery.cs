using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Questions.Queries
{
    #region query

    public class GetQuestionQuery : IRequest<Result<GetQuestionQueryResponse>>
    {
        public int Id { get; set; }
    }

    public class GetQuestionQueryResponse : IMapFrom<ExamQuestion>
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, Result<GetQuestionQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetQuestionQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<GetQuestionQueryResponse>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            var question = await _dbContext.ExamQuestions.FindAsync(request.Id);

            if (question == null)
            {
                return Result<GetQuestionQueryResponse>.Failure("سوال یافت نشد");
            }

            var result = _mapper.Map<GetQuestionQueryResponse>(question);

            return Result<GetQuestionQueryResponse>.Success(result);
        }
    }

    #endregion;
}
