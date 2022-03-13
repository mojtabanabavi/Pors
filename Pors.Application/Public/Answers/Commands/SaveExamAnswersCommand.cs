using System;
using MediatR;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.Answers.Commands
{
    #region command

    public class SaveExamAnswersCommand : IRequest<string>
    {
        public string AttemptId { get; set; }
        public List<AttemptAnswerItem> Answers { get; set; }
    }

    public class AttemptAnswerItem
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
    }

    #endregion;

    #region validator

    public class SaveExamAnswersCommandValidator : AbstractValidator<SaveExamAnswersCommand>
    {
        public SaveExamAnswersCommandValidator()
        {
            RuleFor(x => x.Answers)
                .NotNull()
                .NotEmpty()
                .WithName("گزینه‌ها");

            RuleForEach(x => x.Answers)
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.OptionId)
                        .NotEmpty()
                        .WithName("‌شناسه‌ی پاسخ");

                    x.RuleFor(x => x.QuestionId)
                        .NotEmpty()
                        .WithName("‌شناسه‌ی سوال");
                });
        }
    }

    #endregion;

    #region handler

    public class SaveAttemptAnswersCommandCommandHandler : IRequestHandler<SaveExamAnswersCommand, string>
    {
        private readonly ISqlDbContext _dbContext;

        public SaveAttemptAnswersCommandCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Handle(SaveExamAnswersCommand request, CancellationToken cancellationToken)
        {
            var answers = new List<AttemptAnswer>();

            foreach (var answer in request.Answers)
            {
                answers.Add(new AttemptAnswer
                {
                    OptionId = answer.OptionId,
                    AttemptId = request.AttemptId,
                });
            }

            _dbContext.AttemptAnswers.AddRange(answers);

            await _dbContext.SaveChangesAsync();

            return request.AttemptId;
        }
    }

    #endregion;
}
