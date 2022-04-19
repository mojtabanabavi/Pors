using System;
using MediatR;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Roles.Commands
{
    #region command

    public class DeleteRoleCommand : IRequest
    {
        public int Id { get; set; }
    }

    #endregion;

    #region handler

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteRoleCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Role), request.Id);
            }

            _dbContext.Roles.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
