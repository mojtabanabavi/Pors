using System;
using MediatR;
using System.Text;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Enums;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Identity.Commands
{
    #region command

    public class SendForgetPasswordTokenCommand : IRequest<Result>
    {
        public string Email { get; set; }
    }

    #endregion;

    #region validation

    public class SendForgetPasswordTokenCommandValidator : AbstractValidator<SendForgetPasswordTokenCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public SendForgetPasswordTokenCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Email)
                .NotNull().WithMessage("وارد کردن ایمیل الزامی است.")
                .NotEmpty().WithMessage("وارد کردن ایمیل الزامی است.")
                .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("ایمیل وارد شده معتبر نمی باشد.")
                .MaximumLength(320).WithMessage("ایمیل نباید بیش از 320 کاراکتر داشته باشد.")
                .MustAsync(BeEmailExist).WithMessage("کاربری با ایمیل وارد شده یافت نشد.");
        }

        public async Task<bool> BeEmailExist(string email, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class SendForgetPasswordTokenCommandHandler : IRequestHandler<SendForgetPasswordTokenCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ITokenBuilderService _tokenBuilder;
        private readonly INotificationService _notificationService;

        public SendForgetPasswordTokenCommandHandler(ISqlDbContext dbContext, ITokenBuilderService tokenBuilder, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _tokenBuilder = tokenBuilder;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(SendForgetPasswordTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = await _dbContext.Users
                .Where(x => x.Email == request.Email)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

                var token = await _tokenBuilder
                    .CreateTokenAsync(userId, IdentityTokenType.ResetPassword, IdentityTokenDataType.AlphaNumerical);

                var userToken = new UserToken
                {
                    Value = token,
                    UserId = userId,
                    Type = IdentityTokenType.ResetPassword,
                    ExpireAt = DateTime.UtcNow.AddMinutes(15),
                };

                _dbContext.UserTokens.Add(userToken);

                await _dbContext.SaveChangesAsync();

                // send token
                await _notificationService.SendAsync(request.Email, "بازیابی رمز عبور", token);

                return Result.Success("توکن با موفقیت ارسال گردید.");
            }
            catch
            {
                return Result.Failure("خطایی در ارسال توکن ایجاد شده است، لطفا بعدا تلاش کنید.");
            }
        }
    }

    #endregion;
}
