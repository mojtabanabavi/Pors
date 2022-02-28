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

    public class CreateRoleCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateRoleCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Name)
                .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.")
                .MaximumLength(50).WithMessage("عنوان میتواند حداکثر 50 کاراکتر داشته باشد.")
                .MustAsync(BeUniqueTitle).WithMessage("عنوان وارد شده تکراری است");

            When(x => x.Description.HasValue(), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(250).WithMessage("توضیحات میتواند حداکثر 250 کاراکتر داشته باشد.");
            });
        }

        public async Task<bool> BeUniqueTitle(string roleTitle, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Roles.AnyAsync(x => x.Name == roleTitle);

            return !result;
        }
    }

    #endregion;

    #region handler

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateRoleCommandHandler(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = new Role(request.Name, request.Description);

                _dbContext.Roles.Add(role);

                await _dbContext.SaveChangesAsync();

                return Result.Success("نقش با موفقیت ایجاد شد");
            }
            catch
            {
                return Result.Failure("خطایی در ایجاد نقش اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
