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

namespace Pors.Application.Exams.Commands
{
    #region command

    public class UpdateExamCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateExamCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title)
                .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.")
                .MaximumLength(50).WithMessage("عنوان میتواند حداکثر 50 کاراکتر داشته باشد.");

            RuleFor(x => x.ShortDescription)
                .NotNull().WithMessage("وارد کردن توضیح کوتاه الزامی است.")
                .NotEmpty().WithMessage("وارد کردن توضیح کوتاه الزامی است.")
                .MaximumLength(50).WithMessage("توضیح کوتاه میتواند حداکثر 150 کاراکتر داشته باشد.");
        }

        public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Exams.AnyAsync(x => x.Title == title, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateExamCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exam = await _dbContext.Exams.FindAsync(request.Id);

                exam.Title = request.Title;
                exam.ShortDescription = request.ShortDescription;
                exam.LongDescription = request.LongDescription;

                if (request.Image != null)
                {
                    await _fileManager.DeleteFileAsync(exam.Image);

                    exam.Image = await _fileManager.CreateFileAsync(request.Image);
                }

                await _dbContext.SaveChangesAsync();

                return Result.Success("آزمون با موفقیت ویرایش شد");
            }
            catch
            {
                return Result.Failure("خطایی در ویرایش آزمون اتفاق انفاد.");
            }
        }
    }

    #endregion;
}
