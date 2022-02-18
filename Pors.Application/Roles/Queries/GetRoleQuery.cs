using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Roles.Queries
{
    #region query

    public class GetRoleQuery : IRequest<Result<GetRoleQueryResponse>>
    {
        public int Id { get; set; }
    }

    public class GetRoleQueryResponse : IMapFrom<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, Result<GetRoleQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetRoleQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<GetRoleQueryResponse>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.FindAsync(request.Id);

            if (role == null)
            {
                return Result<GetRoleQueryResponse>.Failure("نقش یافت نشد.");
            }

            var result = _mapper.Map<GetRoleQueryResponse>(role);

            return Result<GetRoleQueryResponse>.Success(result);
        }
    }

    #endregion;
}
