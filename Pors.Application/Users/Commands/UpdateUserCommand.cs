using System;
using MediatR;
using Loby.Tools;
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

    public class UpdateUserCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    #endregion;

    #region validation

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateUserCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeUserExist).WithMessage("کاربری مطابق شناسه دریافتی یافت نشد");

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
    }

    #endregion;

    #region handler

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateUserCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FindAsync(request.Id);

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;

            if (!string.IsNullOrEmpty(request.Password))
            {
                entity.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }

    #endregion;
}
