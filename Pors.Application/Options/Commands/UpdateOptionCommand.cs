using System;
using MediatR;
using Loby.Tools;
using System.Text;
using System.Linq;
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

namespace Pors.Application.Options.Commands
{
    #region command

    public class UpdateOptionCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateOptionCommandValidator : AbstractValidator<UpdateOptionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateOptionCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeOptionExist).WithMessage("گزینه درخواست شده یافت نشد.");

            RuleFor(x => x.Title)
                .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.")
                .MaximumLength(100).WithMessage("عنوان میتواند حداکثر 100 کاراکتر داشته باشد.")
                .Must((x, title) => BeUniqueTitle(x.Id, title).Result).WithMessage("عنوان وارد شده تکراری است");
        }

        public async Task<bool> BeOptionExist(int optionId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.QuestionOptions.AnyAsync(x => x.Id == optionId, cancellationToken);

            return result;
        }

        public async Task<bool> BeUniqueTitle(int optionId, string title)
        {
            var option = await _dbContext.QuestionOptions.FindAsync(optionId);

            if (option.Title == title)
                return true;

            var result = await _dbContext.QuestionOptions
                .AnyAsync(x => x.QuestionId == option.QuestionId && x.Title == title);

            return !result;
        }
    }

    #endregion;

    #region handler

    public class UpdateOptionCommandHandler : IRequestHandler<UpdateOptionCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateOptionCommandHandler(ISqlDbContext sqlDbContext, IFileManagerService fileManager)
        {
            _dbContext = sqlDbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var option = await _dbContext.QuestionOptions.FindAsync(request.Id);

                option.Title = request.Title;

                if (request.Image != null)
                {
                    await _fileManager.DeleteFileAsync(option.Image);

                    option.Image = await _fileManager.CreateFileAsync(request.Image);
                }

                await _dbContext.SaveChangesAsync();

                return Result.Success("گزینه با موفقیت ویرایش شد");
            }
            catch
            {
                return Result.Failure("خطایی در ویرایش گزینه اتفاق انفاد.");
            }
        }
    }

    #endregion;
}
