using System;
using MediatR;
using Loby.Extensions;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Options.Commands
{
    #region command

    public class DeleteOptionCommand : IRequest
    {
        public int Id { get; set; }
    }

    #endregion;

    #region handler

    public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public DeleteOptionCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Unit> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.QuestionOptions.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(QuestionOption), request.Id);
            }

            if (entity.Image.HasValue())
            {
                await _fileManager.DeleteFileAsync(entity.Image);
            }

            _dbContext.QuestionOptions.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
