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

namespace Pors.Application.Management.Exams.Commands
{
    #region command

    public class UpdateExamCommand : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateExamCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(50)
                .Must(UniqueTitle).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("عنوان");

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .MaximumLength(50)
                .WithName("توضیحات");
        }

        private bool UniqueTitle(UpdateExamCommand command, string title)
        {
            var entity = _dbContext.Exams.Find(command.Id);

            if (entity?.Title == title)
            {
                return true;
            }

            return _dbContext.Exams.All(x => x.Title != title);
        }
    }

    #endregion;

    #region handler

    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateExamCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Unit> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Exams.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exam), request.Id);
            }

            entity.Title = request.Title;
            entity.ShortDescription = request.ShortDescription;
            entity.LongDescription = request.LongDescription;

            if (request.Image != null)
            {
                await _fileManager.DeleteFileAsync(entity.Image);

                entity.Image = await _fileManager.CreateFileAsync(request.Image);
            }

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
