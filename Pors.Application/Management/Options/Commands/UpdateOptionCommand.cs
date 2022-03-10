using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Options.Commands
{
    #region command

    public class UpdateOptionCommand : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateOptionCommandValidator : AbstractValidator<UpdateOptionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateOptionCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100)
                .Must(UniqueTitle).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("عنوان");
        }

        private bool UniqueTitle(UpdateOptionCommand command, string title)
        {
            var entity = _dbContext.QuestionOptions.Find(command.Id);

            if (entity?.Title == title)
            {
                return true;
            }

            return _dbContext.QuestionOptions.All(x => x.Title != title);
        }
    }

    #endregion;

    #region handler

    public class UpdateOptionCommandHandler : IRequestHandler<UpdateOptionCommand>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateOptionCommandHandler(ISqlDbContext sqlDbContext, IFileManagerService fileManager)
        {
            _dbContext = sqlDbContext;
            _fileManager = fileManager;
        }

        public async Task<Unit> Handle(UpdateOptionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.QuestionOptions.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(QuestionOption), request.Id);
            }

            entity.Title = request.Title;
            entity.Description = request.Description;

            if (request.Image != null)
            {
                entity.Image = await _fileManager.UpdateFileAsync(request.Image, entity.Image);
            }

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
