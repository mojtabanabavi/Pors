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
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Options.Queries
{
    #region query

    public class GetOptionQuery : IRequest<Result<GetOptionQueryResponse>>
    {
        public int Id { get; set; }
    }

    public class GetOptionQueryResponse : IMapFrom<QuestionOption>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetOptionQueryHandler : IRequestHandler<GetOptionQuery, Result<GetOptionQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetOptionQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<GetOptionQueryResponse>> Handle(GetOptionQuery request, CancellationToken cancellationToken)
        {
            var option = await _dbContext.QuestionOptions.FindAsync(request.Id);

            if (option == null)
            {
                return Result<GetOptionQueryResponse>.Failure("گزینه یافت نشد");
            }

            var result = _mapper.Map<GetOptionQueryResponse>(option);

            return Result<GetOptionQueryResponse>.Success(result);
        }
    }

    #endregion;
}
