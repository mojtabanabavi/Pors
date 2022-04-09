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

namespace Pors.Application.Management.Identity.Queries
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
        private const int DefaultLockoutMinutes = 3;
        private const int MaxFailedAccessAttempts = 5;
        private const bool RequireConfirmedEmail = false;
        private const bool RequireConfirmedPhoneNumber = false;

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
                return Result<LoginUserQueryResponse>.Failure(GenerateCredentialMismatchMessage());
            }

            if (!entity.IsActive)
            {
                return Result<LoginUserQueryResponse>.Failure(GenerateAccountIsInActiveMessage());
            }

            if (IsAccountLocked(entity))
            {
                return Result<LoginUserQueryResponse>.Failure(GenerateAccountIsLockedMessage(entity));
            }

            if (RequireConfirmedEmail && entity.IsEmailConfirmed)
            {
                return Result<LoginUserQueryResponse>.Failure(GenerateRequireConfirmedEmailMessage());
            }

            if (RequireConfirmedPhoneNumber && entity.IsPhoneNumberConfirmed)
            {
                return Result<LoginUserQueryResponse>.Failure(GenerateRequireConfirmedPhoneNumberMessage());
            }

            // mismatch password
            if (!PasswordHasher.Verify(request.Password, entity.PasswordHash))
            {
                await IncreaseFailedAccessAttemptsCountAsync(entity);

                if (entity.AccessFailedCount >= MaxFailedAccessAttempts)
                {
                    await LockAccountAsync(entity);

                    return Result<LoginUserQueryResponse>.Failure(GenerateAccountIsLockedMessage(entity));
                }

                return Result<LoginUserQueryResponse>.Failure(GenerateCredentialMismatchMessage());
            }

            if (entity.AccessFailedCount != 0)
            {
                await UnLockAccountAsync(entity);
            }

            await UpdateLastLoginDateTimeAsync(entity);

            return Result<LoginUserQueryResponse>.Success(new LoginUserQueryResponse(entity));
        }


        private string GenerateAccountIsLockedMessage(User user)
        {
            var remainingLockoutMinutes = (DateTime.Now - user.LockoutEndAt.Value).Minutes;

            var message = $"بدلیل {MaxFailedAccessAttempts} تلاش ناموفق، حساب شما تا {remainingLockoutMinutes} دقیقه‌ی آینده قفل می‌باشد.";

            return message;
        }

        private string GenerateCredentialMismatchMessage()
        {
            return "ایمیل یا رمزعبور صحیح نیست.";
        }

        private string GenerateAccountIsInActiveMessage()
        {
            return "متاسفانه حساب شما غیرفعال شده است. لطفا با پشتیبانی تماس بگیرید.";
        }

        private string GenerateRequireConfirmedEmailMessage()
        {
            return "برای ورود به حساب لازم است ایمیل شما تایید شده باشد.";
        }

        private string GenerateRequireConfirmedPhoneNumberMessage()
        {
            return "برای ورود به حساب لازم است شماره تلفن شما تایید شده باشد.";
        }


        private async Task IncreaseFailedAccessAttemptsCountAsync(User user)
        {
            user.AccessFailedCount += 1;

            await _dbContext.SaveChangesAsync();
        }

        private async Task LockAccountAsync(User user)
        {
            user.AccessFailedCount = 0;
            user.LockoutEndAt = DateTime.Now.AddMinutes(DefaultLockoutMinutes);

            await _dbContext.SaveChangesAsync();
        }

        private async Task UnLockAccountAsync(User user)
        {
            user.LockoutEndAt = null;
            user.AccessFailedCount = 0;

            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateLastLoginDateTimeAsync(User user)
        {
            user.LastLoginDateTime = DateTime.Now;

            await _dbContext.SaveChangesAsync();
        }


        private bool IsAccountLocked(User user)
        {
            return user.LockoutEndAt.HasValue && user.LockoutEndAt.Value > DateTime.Now;
        }
    }

    #endregion;
}