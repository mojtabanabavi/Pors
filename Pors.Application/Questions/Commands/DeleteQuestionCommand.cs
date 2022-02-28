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

    public class DeleteQuestionCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validator

    public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteQuestionCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeQuestionExist).WithMessage("سوالی مطابق شناسه دریافتی یافت نشد");
        }

        public async Task<bool> BeQuestionExist(int questionId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.ExamQuestions.AnyAsync(x => x.Id == questionId, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteQuestionCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _dbContext.ExamQuestions.FindAsync(request.Id);

                _dbContext.ExamQuestions.Remove(question);

                await _dbContext.SaveChangesAsync();

                return Result.Success("سوال با موفقیت حذف گردید.");
            }
            catch
            {
                return Result.Failure("خطایی در حذف سوال اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
