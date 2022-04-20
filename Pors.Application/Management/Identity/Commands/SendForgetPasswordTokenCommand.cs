using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Enums;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Validators;

namespace Pors.Application.Management.Identity.Commands
{
    #region command

    public class SendForgetPasswordTokenCommand : IRequest
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
                .NotEmpty()
                .MaximumLength(320)
                .ValidEmailAddress()
                .Must(ExistEmail).WithMessage("'{PropertyName}' یافت نشد.")
                .WithName("ایمیل");
        }

        private bool ExistEmail(string email)
        {
            return _dbContext.Users.Any(x => x.Email == email);
        }
    }

    #endregion;

    #region handler

    public class SendForgetPasswordTokenCommandHandler : IRequestHandler<SendForgetPasswordTokenCommand>
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

        public async Task<Unit> Handle(SendForgetPasswordTokenCommand request, CancellationToken cancellationToken)
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

            await _notificationService.SendAsync(request.Email, "بازیابی رمز عبور", token);

            return Unit.Value;
        }
    }

    #endregion;
}
