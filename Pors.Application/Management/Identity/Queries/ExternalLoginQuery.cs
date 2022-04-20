using System;
using MediatR;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Identity.Queries
{
    #region query

    public class ExternalLoginQuery : IRequest<Result<ExternalLoginQueryResponse>>
    {
        public string Email { get; set; }

        public ExternalLoginQuery()
        {
        }

        public ExternalLoginQuery(string email)
        {
            Email = email;
        }
    }

    #endregion;

    #region response

    public class ExternalLoginQueryResponse
    {
        public ExternalLoginQueryResponse(User user)
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

    #region handler

    public class ExternalLoginQueryHandler : IRequestHandler<ExternalLoginQuery, Result<ExternalLoginQueryResponse>>
    {
        private const bool RequireConfirmedEmail = false;
        private const bool RequireConfirmedPhoneNumber = false;

        private readonly ISqlDbContext _dbContext;

        public ExternalLoginQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ExternalLoginQueryResponse>> Handle(ExternalLoginQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == request.Email);

            if (entity == null)
            {
                return Result<ExternalLoginQueryResponse>.Failure(GenerateCredentialMismatchMessage());
            }

            if (!entity.IsActive)
            {
                return Result<ExternalLoginQueryResponse>.Failure(GenerateAccountIsInActiveMessage());
            }

            if (RequireConfirmedEmail && entity.IsEmailConfirmed)
            {
                return Result<ExternalLoginQueryResponse>.Failure(GenerateRequireConfirmedEmailMessage());
            }

            if (RequireConfirmedPhoneNumber && entity.IsPhoneNumberConfirmed)
            {
                return Result<ExternalLoginQueryResponse>.Failure(GenerateRequireConfirmedPhoneNumberMessage());
            }

            await UpdateLastLoginDateTimeAsync(entity);

            return Result<ExternalLoginQueryResponse>.Success(new ExternalLoginQueryResponse(entity));
        }

        private string GenerateCredentialMismatchMessage()
        {
            return "کاربری مطابق ایمیل وارد شده یافت نشد.";
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

        private async Task UpdateLastLoginDateTimeAsync(User user)
        {
            user.LastLoginDateTime = DateTime.Now;

            await _dbContext.SaveChangesAsync();
        }
    }

    #endregion;
}
