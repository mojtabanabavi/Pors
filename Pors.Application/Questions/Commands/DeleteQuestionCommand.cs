using System;
using MediatR;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Questions.Commands
{
    #region command

    public class DeleteQuestionCommand : IRequest
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteQuestionCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.ExamQuestions.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ExamQuestion), request.Id);
            }

            _dbContext.ExamQuestions.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
