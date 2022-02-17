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

namespace Pors.Application.Exams.Commands
{
    #region command

    public class CreateExamCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
    }

    #endregion;

    #region validator

    public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateExamCommandValidator(ISqlDbContext dbContext)
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

    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileManagerService _fileManager;

        public CreateExamCommandHandler(ISqlDbContext dbContext, ICurrentUserService currentUser, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exam = new Exam
                {
                    Title = request.Title,
                    CreatedBy = _currentUser.DisplayName,
                    LongDescription = request.LongDescription,
                    ShortDescription = request.ShortDescription,
                };

                if(request.Image != null)
                {
                    exam.Image = await _fileManager.CreateFileAsync(request.Image);
                }

                _dbContext.Exams.Add(exam);

                await _dbContext.SaveChangesAsync();

                return Result.Success("آزمون با موفقیت ایجاد شد");
            }
            catch
            {
                return Result.Failure("خطایی در ساخت آزمون اتفاق انفاد.");
            }
        }
    }

    #endregion;
}
