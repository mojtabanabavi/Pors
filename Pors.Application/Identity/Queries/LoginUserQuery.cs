using System;
using MediatR;
using Loby.Tools;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Identity.Queries
{
    #region query

    public class LoginUserQuery : IRequest<Result<LoginUserQueryResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    #endregion;

    #region response

    public class LoginUserQueryResponse
    {
        public LoginUserQueryResponse(User user)
        {
            User = user;
        }

        public User User { get; set; }
        public string DisplayName
        {
            get
            {
                string fullName = string.Empty;

                if (!string.IsNullOrEmpty(User.FirstName) && !string.IsNullOrEmpty(User.LastName))
                {
                    fullName = $"{User.FirstName} {User.LastName}";
                }

                return fullName ?? User.Email ?? User.PhoneNumber;
            }
        }
    }

    #endregion;

    #region validator

    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        private readonly ISqlDbContext _dbContext;

        public LoginUserQueryValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(320)
                .Must(ValidEmail).WithMessage("'{PropertyName}' معتبر نمی‌باشد.")
                .Must(ExistEmail).WithMessage("'{PropertyName}' یافت نشد.")
                .WithName("ایمیل");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 50)
                .WithName("رمزعبور");
        }

        private bool ValidEmail(string email)
        {
            return Validator.IsValidEmail(email);
        }

        private bool ExistEmail(string email)
        {
            return _dbContext.Users.Any(x => x.Email == email);
        }
    }

    #endregion;

    #region handler

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<LoginUserQueryResponse>>
    {
        private readonly ISqlDbContext _dbContext;

        public LoginUserQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<LoginUserQueryResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == request.Email);

            if (entity == null)
            {
                return Result<LoginUserQueryResponse>.Failure("ایمیل یا رمزعبور صحیح نیست.");
            }

            if (!PasswordHasher.Verify(request.Password, entity.PasswordHash))
            {
                return Result<LoginUserQueryResponse>.Failure("ایمیل یا رمزعبور صحیح نیست.");
            }

            return Result<LoginUserQueryResponse>.Success(new LoginUserQueryResponse(entity));
        }
    }

    #endregion;
}