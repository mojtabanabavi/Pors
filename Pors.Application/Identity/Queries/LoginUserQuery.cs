using System;
using MediatR;
using Loby.Tools;
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
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Identity.Queries
{
    #region query

    public class LoginUserQuery : IRequest<Result<LoginUserQueryResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

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
                string fullName = null;

                if (!string.IsNullOrEmpty(User.FirstName) && !string.IsNullOrEmpty(User.LastName))
                {
                    fullName = $"{User.FirstName} {User.LastName}";
                }

                return fullName ?? User.PhoneNumber ?? User.Email;
            }
        }
    }

    #endregion;

    #region validation

    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("وارد کردن ایمیل الزامی است")
                .NotEmpty().WithMessage("وارد کردن ایمیل الزامی است")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("ایمیل وارد شده معتبر نمی باشد.")
                .MaximumLength(320).WithMessage("ایمیل نباید بیش از 320 کاراکتر داشته باشد");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("وارد کردن رمزعبور الزامی است")
                .NotEmpty().WithMessage("وارد کردن رمزعبور الزامی است")
                .Length(8, 50).WithMessage("رمز عبور میتواند حداقل 8 و حداکثر 50 کاراکتر داشته باشد");
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
            var entity = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == request.Email);

            if (entity == null)
            {
                return Result<LoginUserQueryResponse>.Failure("نام کاربری یا رمز عبور اشتباه می باشد.");
            }

            if (!PasswordHasher.Verify(request.Password, entity.PasswordHash))
            {
                return Result<LoginUserQueryResponse>.Failure("نام کاربری یا رمز عبور اشتباه می باشد.");
            }

            return Result<LoginUserQueryResponse>.Success(new LoginUserQueryResponse(entity));
        }
    }

    #endregion;
}