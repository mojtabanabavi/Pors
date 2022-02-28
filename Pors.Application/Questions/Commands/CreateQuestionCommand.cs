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

namespace Pors.Application.Questions.Commands
{
    #region command

    public class CreateQuestionCommand : IRequest<Result>
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
    }

    #endregion;

    #region validator

    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateQuestionCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.ExamId)
                .MustAsync(BeExamExist).WithMessage("آزمون درخواست شده یافت نشد.");

            RuleFor(x => x.Title)
                .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.")
                .MaximumLength(250).WithMessage("عنوان میتواند حداکثر 250 کاراکتر داشته باشد.")
                .Must((x, title) => BeUniqueTitle(x.ExamId, title).Result).WithMessage("عنوان وارد شده تکراری است");
        }

        public async Task<bool> BeExamExist(int examId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Exams.AnyAsync(x => x.Id == examId, cancellationToken);

            return result;
        }

        public async Task<bool> BeUniqueTitle(int examId, string title)
        {
            var result = await _dbContext.ExamQuestions.AnyAsync(x => x.ExamId == examId && x.Title == title);

            return !result;
        }
    }

    #endregion;

    #region handler

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public CreateQuestionCommandHandler(ISqlDbContext sqlDbContext, ICurrentUserService currentUser)
        {
            _dbContext = sqlDbContext;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = new ExamQuestion
                {
                    Title = request.Title,
                    ExamId = request.ExamId,
                    CreatedBy = _currentUser.DisplayName
                };

                _dbContext.ExamQuestions.Add(question);

                await _dbContext.SaveChangesAsync();

                return Result.Success("سوال با موفقیت ایجاد شد");
            }
            catch
            {
                return Result.Failure("خطایی در ایجاد سوال اتفاق انفاد.");
            }
        }
    }

    #endregion;
}
