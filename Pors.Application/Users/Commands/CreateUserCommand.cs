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

namespace Pors.Application.Users.Commands
{
    #region command

    public class CreateUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    #endregion;

    #region validation

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateUserCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotNull().WithMessage("وارد کردن ایمیل الزامی است")
                .NotEmpty().WithMessage("وارد کردن ایمیل الزامی است")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("ایمیل وارد شده معتبر نمی باشد.")
                .MaximumLength(320).WithMessage("ایمیل نباید بیش از 320 کاراکتر داشته باشد")
                .MustAsync(BeUniqueEmail).WithMessage("ایمیل وارد شده تکراری است");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("وارد کردن رمزعبور الزامی است")
                .NotEmpty().WithMessage("وارد کردن رمزعبور الزامی است")
                .Length(8, 50).WithMessage("رمز عبور میتواند حداقل 8 و حداکثر 50 کاراکتر داشته باشد");
        }

        public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IPasswordHashService _passwordHasher;

        public CreateUserCommandHandler(ISqlDbContext dbContext, IPasswordHashService passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password)
            };

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }

    #endregion;
}