using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Questions.Commands
{
    #region command

    public class UpdateQuestionCommand : IRequest
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

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(250)
                .Must(UniqueTitle).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("عنوان");
        }

        private bool UniqueTitle(UpdateQuestionCommand command, string title)
        {
            var entity = _dbContext.ExamQuestions.Find(command.Id);

            if (entity?.Title == title)
            {
                return true;
            }

            return _dbContext.ExamQuestions.All(x => x.Title != title);
        }
    }

    #endregion;

    #region handler

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateQuestionCommandHandler(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;
        }

        public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.ExamQuestions.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ExamQuestion), request.Id);
            }

            entity.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
