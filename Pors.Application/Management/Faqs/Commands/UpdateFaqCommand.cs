using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Faqs.Commands
{
    #region command

    public class UpdateFaqCommand : IRequest
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateFaqCommandValidator : AbstractValidator<UpdateFaqCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateFaqCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Question)
                .NotEmpty()
                .MaximumLength(80)
                .Must(UniqueQuestion).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("سوال");

            RuleFor(x => x.Answer)
                .NotEmpty()
                .MaximumLength(1000)
                .WithName("پاسخ");
        }

        private bool UniqueQuestion(UpdateFaqCommand command, string question)
        {
            var entity = _dbContext.Faqs.Find(command.Id);

            if (entity?.Question == question)
            {
                return true;
            }

            return _dbContext.Faqs.All(x => x.Question != question);
        }
    }

    #endregion;

    #region handler

    public class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateFaqCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Faqs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Faq), request.Id);
            }

            entity.Answer = request.Answer;
            entity.Question = request.Question;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
