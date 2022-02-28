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

    public class UpdateQuestionCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateQuestionCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeQuestionExist).WithMessage("سوال درخواست شده یافت نشد.");

            RuleFor(x => x.Title)
                .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.")
                .MaximumLength(250).WithMessage("عنوان میتواند حداکثر 250 کاراکتر داشته باشد.")
                .Must((x, title) => BeUniqueTitle(x.Id, title).Result).WithMessage("عنوان وارد شده تکراری است");
        }

        public async Task<bool> BeQuestionExist(int questionId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.ExamQuestions.AnyAsync(x => x.Id == questionId, cancellationToken);

            return result;
        }

        public async Task<bool> BeUniqueTitle(int questionId, string title)
        {
            var question = await _dbContext.ExamQuestions.FindAsync(questionId);

            if (question.Title == title)
                return true;

            var result = await _dbContext.ExamQuestions
                .AnyAsync(x => x.ExamId == question.ExamId && x.Title == title);

            return !result;
        }
    }

    #endregion;

    #region handler

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public UpdateQuestionCommandHandler(ISqlDbContext sqlDbContext, ICurrentUserService currentUser)
        {
            _dbContext = sqlDbContext;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _dbContext.ExamQuestions.FindAsync(request.Id);

                question.Title = request.Title;

                await _dbContext.SaveChangesAsync();

                return Result.Success("سوال با موفقیت ویرایش شد");
            }
            catch
            {
                return Result.Failure("خطایی در ویرایش سوال اتفاق انفاد.");
            }
        }
    }

    #endregion;
}
