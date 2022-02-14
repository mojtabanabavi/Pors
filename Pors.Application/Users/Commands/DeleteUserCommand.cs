using System;
using MediatR;
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

    public class DeleteUserCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validation

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteUserCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeUserExist).WithMessage("کاربری مطابق شناسه دریافتی یافت نشد");
        }

        public async Task<bool> BeUserExist(int userId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteUserCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FindAsync(request.Id);

            _dbContext.Users.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Result.Success();
        }
    }

    #endregion;
}
