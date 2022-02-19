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

namespace Pors.Application.Users.Queries
{
    #region query

    public class GetUserQuery : IRequest<Result<GetUserQueryResponse>>
    {
        public int Id { get; set; }
    }

    public class GetUserQueryResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string LastLoginDateTime { get; set; }
        public string RegisterDateTime { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<GetUserQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetUserQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Result<GetUserQueryResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(request.Id);

            if (user == null)
            {
                return Result<GetUserQueryResponse>.Failure("کاربر یافت نشد.");
            }

            var result = _mapper.Map<GetUserQueryResponse>(user);

            return Result<GetUserQueryResponse>.Success(result);
        }
    }

    #endregion;
}
