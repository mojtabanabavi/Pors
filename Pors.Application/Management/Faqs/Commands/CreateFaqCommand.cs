using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Faqs.Commands
{
    #region command

    public class CreateFaqCommand : IRequest<int>
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    #endregion;

    #region validator

    public class CreateFaqCommandValidator : AbstractValidator<CreateFaqCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateFaqCommandValidator(ISqlDbContext dbContext)
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

        private bool UniqueQuestion(string question)
        {
            return _dbContext.Faqs.All(x => x.Question != question);
        }
    }

    #endregion;

    #region handler

    public class CreateFaqCommandHandler : IRequestHandler<CreateFaqCommand, int>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateFaqCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateFaqCommand request, CancellationToken cancellationToken)
        {
            var entity = new Faq
            {
                Answer = request.Answer,
                Question = request.Question,
            };

            _dbContext.Faqs.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }

    #endregion;
}
