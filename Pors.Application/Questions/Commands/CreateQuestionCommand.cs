using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Exceptions;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Questions.Commands
{
    #region command

    public class CreateQuestionCommand : IRequest<int>
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

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(250)
                .Must(UniqueTitle).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("عنوان");
        }

        private bool UniqueTitle(string title)
        {
            return _dbContext.ExamQuestions.All(x => x.Title != title);
        }
    }

    #endregion;

    #region handler

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, int>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public CreateQuestionCommandHandler(ISqlDbContext sqlDbContext, ICurrentUserService currentUser)
        {
            _dbContext = sqlDbContext;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var isExamExist = await _dbContext.Exams.AnyAsync(x => x.Id == request.ExamId);

            if (!isExamExist)
            {
                throw new NotFoundException(nameof(Exam), request.ExamId);
            }

            var entity = new ExamQuestion
            {
                Title = request.Title,
                ExamId = request.ExamId,
                CreatedBy = _currentUser.DisplayName
            };

            _dbContext.ExamQuestions.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }

    #endregion;
}
