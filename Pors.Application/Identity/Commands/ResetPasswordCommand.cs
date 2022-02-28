using System;
using MediatR;
using Loby.Tools;
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

    public class ResetPasswordCommand : IRequest<Result>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordCommand(string token)
        {
            Token = token;
        }
    }

    #endregion;

    #region validation

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public ResetPasswordCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.NewPassword)
                .NotNull().WithMessage("وارد کردن رمزعبور الزامی است")
                .NotEmpty().WithMessage("وارد کردن رمزعبور الزامی است")
                .Length(8, 50).WithMessage("رمز عبور میتواند حداقل 8 و حداکثر 50 کاراکتر داشته باشد");

            RuleFor(x => x.Token)
                .NotNull().WithMessage("وارد کردن توکن الزامی است")
                .NotEmpty().WithMessage("وارد کردن توکن الزامی است")
                .MaximumLength(250).WithMessage("توکن نباید بیش از 250 کاراکتر داشته باشد")
                .MustAsync(BeTokenExist).WithMessage("توکن وارد شده یافت نشد.")
                .MustAsync(BeTokenValid).WithMessage("توکن وارد شده منقضی شده است.");
        }

        public async Task<bool> BeTokenExist(string token, CancellationToken cancellationToken)
        {
            var result = await _dbContext.UserTokens.AnyAsync(x => x.Value == token, cancellationToken);

            return result;
        }

        public async Task<bool> BeTokenValid(string token, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;

            var result = await _dbContext.UserTokens.AnyAsync(x => x.Value == token && x.ExpireAt > now, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public ResetPasswordCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.UserTokens
                .Where(x => x.Value == request.Token)
                .Select(x => x.User)
                .SingleOrDefaultAsync();

                user.PasswordHash = PasswordHasher.Hash(request.NewPassword);

                await _dbContext.SaveChangesAsync();

                return Result.Success("رمز عبور با موفقیت بازنشانی شد.");
            }
            catch
            {
                return Result.Failure("خطایی در تغییر رمز عبور اتفاق افتاد، لطفا بعدا تلاش کنید.");
            }
        }
    }

    #endregion;
}
