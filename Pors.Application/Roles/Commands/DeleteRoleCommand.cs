using System;
using MediatR;
using Loby.Tools;
using System.Text;
using System.Linq;
using FluentValidation;
using Pors.Domain.Enums;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Roles.Commands
{
    #region command

    public class DeleteRoleCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validator

    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteRoleCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeRoleExist).WithMessage("نقشی مطابق شناسه دریافتی یافت نشد");
        }

        public async Task<bool> BeRoleExist(int roleId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Roles.AnyAsync(x => x.Id == roleId, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteRoleCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _dbContext.Roles.FindAsync(request.Id);

                _dbContext.Roles.Remove(role);

                await _dbContext.SaveChangesAsync();

                return Result.Success("نقش با موفقیت حذف گردید.");
            }
            catch
            {
                return Result.Failure("خطایی در حذف نقش اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
