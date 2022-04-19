using System;
using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Validators;

namespace Pors.Application.Public.Identity.Queries
{
    #region query

    public class LoginUserQuery : IRequest<Result>
    {
        public string ParticipantId { get; set; }
        public string GoogleRecaptchaResponse { get; set; }
    }

    #endregion;

    #region validator

    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(x => x.ParticipantId)
                .NotEmpty()
                .MaximumLength(36)
                .WithName("شناسه");

            RuleFor(X => X.GoogleRecaptchaResponse)
                .SetValidator(new GoogleRecaptchaValidator<LoginUserQuery, string>());
        }
    }

    #endregion;

    #region handler

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public LoginUserQueryHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.ExamAttempts
                .FirstOrDefaultAsync(x => x.ParticipantId == request.ParticipantId);

            if (entity == null)
            {
                return Result.Failure(GenerateCredentialMismatchMessage());
            }

            return Result.Success();
        }

        private string GenerateCredentialMismatchMessage()
        {
            return "شناسه‌ی وارد شده یافت نشد.";
        }
    }

    #endregion;
}