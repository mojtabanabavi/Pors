using System;
using MediatR;
using Loby.Tools;
using System.Linq;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Identity.Commands
{
    #region command

    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public ResetPasswordCommand(string token)
        {
            Token = token;
        }
    }

    #endregion;

    #region validator

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public ResetPasswordCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Length(8, 50)
                .WithName("رمزعبور جدید");

            RuleFor(x => x.Token)
                .NotEmpty()
                .MaximumLength(250)
                .Must(ExistToken).WithMessage("'{PropertyName}' یافت نشد.")
                .Must(ValidToken).WithMessage("'{PropertyName}' منقضی شده است.")
                .WithName("توکن");
        }

        private bool ExistToken(string token)
        {
            return _dbContext.UserTokens.Any(x => x.Value == token);
        }

        private bool ValidToken(string token)
        {
            return _dbContext.UserTokens.Any(x => x.Value == token && x.ExpireAt > DateTime.Now);
        }
    }

    #endregion;

    #region handler

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public ResetPasswordCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.UserTokens
                .Where(x => x.Value == request.Token)
                .Select(x => x.User)
                .SingleOrDefaultAsync();

            user.PasswordHash = PasswordHasher.Hash(request.NewPassword);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
