using System;
using MediatR;
using Loby.Tools;
using System.Text;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
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

    public class UpdateRoleCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateRoleCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Name)
                .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.")
                .MaximumLength(50).WithMessage("عنوان میتواند حداکثر 50 کاراکتر داشته باشد.")
                .Must((x, title) => BeUniqueTitle(x.Id, title).Result).WithMessage("عنوان وارد شده تکراری است");

            When(x => x.Description.HasValue(), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(250).WithMessage("توضیحات میتواند حداکثر 250 کاراکتر داشته باشد.");
            });
        }

        public async Task<bool> BeUniqueTitle(int roleId, string title)
        {
            var role = await _dbContext.Roles.FindAsync(roleId);

            if (role.Name == title)
                return true;

            var result = await _dbContext.Roles.AnyAsync(x => x.Name == title);

            return !result;
        }
    }

    #endregion;

    #region handler

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateRoleCommandHandler(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;
        }

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _dbContext.Roles.FindAsync(request.Id);

                role.Name = request.Name;
                role.Description = request.Description;

                await _dbContext.SaveChangesAsync();

                return Result.Success("نقش با موفقیت ویرایش شد");
            }
            catch
            {
                return Result.Failure("خطایی در ویرایش نقش اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
