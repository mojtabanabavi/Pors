using System;
using MediatR;
using Loby.Tools;
using System.Text;
using System.Linq;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion;

    #region validation

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateUserCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(25).WithMessage("نام نباید بیش از 25 کاراکتر داشته باشد");
            });

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(25).WithMessage("نام خانوادگی نباید بیش از 25 کاراکتر داشته باشد");
            });

            When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Matches(@"^0?9\d{9}$").WithMessage("شماره تلفن معتبر نمی‌باشد")
                    .MaximumLength(15).WithMessage("شماره تلفن نباید بیش از 15 کاراکتر داشته باشد");
            });

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

            return !result;
        }
    }

    #endregion;

    #region handler

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public CreateUserCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = new User
                {
                    Email = request.Email,
                    LastName = request.LastName,
                    IsActive = request.IsActive,
                    FirstName = request.FirstName,
                    PhoneNumber = request.PhoneNumber,
                    IsEmailConfirmed = request.IsEmailConfirmed,
                    PasswordHash = PasswordHasher.Hash(request.Password),
                    IsPhoneNumberConfirmed = request.IsPhoneNumberConfirmed,
                };

                if (request.ProfilePicture != null)
                {
                    user.ProfilePicture = await _fileManager.CreateFileAsync(request.ProfilePicture);
                }

                _dbContext.Users.Add(user);

                await _dbContext.SaveChangesAsync();

                return Result.Success("کاربر با موفقیت ایجاد شد");
            }
            catch
            {
                return Result.Failure("خطایی در ساخت کاربر اتفاق انفاد.");
            }
        }
    }

    #endregion;
}