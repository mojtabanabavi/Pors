using System;
using MediatR;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Faqs.Commands
{
    #region command

    public class DeleteFaqCommand : IRequest
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteFaqCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteFaqCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Faqs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Faq), request.Id);
            }

            _dbContext.Faqs.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
