using System;
using MediatR;
using Loby.Tools;
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
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Profiles.Commands
{
    #region command

    public class UpdateProfileCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
    }

    #endregion;

    #region validation

    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateProfileCommandValidator(ISqlDbContext dbContext)
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
                    .Matches(@"^0?9\d{9}$").WithMessage("شماره تلفن معتبر نمی باشد")
                    .MaximumLength(11).WithMessage("شماره تلفن نباید بیش از 11 کاراکتر داشته باشد")
                    .Must((x, phoneNumber) => BeUniquePhone(x.Id, phoneNumber).Result).WithMessage("شماره تلفن وارد شده تکراری است");
            });

            RuleFor(x => x.Email)
                .NotNull().WithMessage("وارد کردن ایمیل الزامی است")
                .NotEmpty().WithMessage("وارد کردن ایمیل الزامی است")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("ایمیل وارد شده معتبر نمی باشد.")
                .MaximumLength(320).WithMessage("ایمیل نباید بیش از 320 کاراکتر داشته باشد")
                .Must((x, email) => BeUniqueEmail(x.Id, email).Result).WithMessage("ایمیل وارد شده تکراری است");

            When(x => !string.IsNullOrEmpty(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .NotNull().WithMessage("وارد کردن رمزعبور الزامی است")
                    .NotEmpty().WithMessage("وارد کردن رمزعبور الزامی است")
                    .Length(8, 50).WithMessage("رمز عبور میتواند حداقل 8 و حداکثر 50 کاراکتر داشته باشد");
            });
        }

        public async Task<bool> BeUserExist(int userId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken);

            return result;
        }

        public async Task<bool> BeUniqueEmail(int userId, string email)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user.Email == email)
                return true;

            var result = await _dbContext.Users.AnyAsync(x => x.Email == email);

            return !result;
        }

        public async Task<bool> BeUniquePhone(int userId, string phoneNumber)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user.PhoneNumber == phoneNumber)
                return true;

            var result = await _dbContext.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);

            return !result;
        }
    }

    #endregion;

    #region handler

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateProfileCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(request.Id);

            user.Email = request.Email;
            user.LastName = request.LastName;
            user.FirstName = request.FirstName;
            user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            if (request.ProfilePicture != null)
            {
                await _fileManager.DeleteFileAsync(user.ProfilePicture);

                user.ProfilePicture = await _fileManager.CreateFileAsync(request.ProfilePicture);
            }

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }

    #endregion;
}
