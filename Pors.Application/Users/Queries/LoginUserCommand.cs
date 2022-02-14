using System;
using MediatR;
using System.Text;
using System.Linq;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Users.Queries
{
    #region query

    public class LoginUserQuery : IRequest<Result<User>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    #endregion;

    #region validation
    #endregion;

    #region handler

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<User>>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IPasswordHashService _passwordHasher;

        public LoginUserQueryHandler(ISqlDbContext dbContext, IPasswordHashService passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<User>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            request.Password = _passwordHasher.Hash(request.Password);

            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == request.Email && x.PasswordHash == request.Password);

            if (entity == null)
            {
                return Result<User>.Failure("کاربری یافت نشد");
            }

            return Result<User>.Success(entity);
        }
    }

    #endregion;
}