using System;
using MediatR;
using Loby.Extensions;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Exams.Commands
{
    #region command

    public class DeleteExamCommand : IRequest
    {
        public int Id { get; set; }
    }

    #endregion;

    #region handler

    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public DeleteExamCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Unit> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Exams.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exam), request.Id);
            }

            if (entity.Image.HasValue())
            {
                await _fileManager.DeleteFileAsync(entity.Image);
            }

            _dbContext.Exams.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
