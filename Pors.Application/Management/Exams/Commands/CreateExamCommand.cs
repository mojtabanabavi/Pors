using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Management.Exams.Commands
{
    #region command

    public class CreateExamCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IFormFile Image { get; set; }
    }

    #endregion;

    #region validator

    public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateExamCommandValidator(ISqlDbContext dbContext)
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

        private bool UniqueTitle(string title)
        {
            return _dbContext.Exams.All(x => x.Title != title);
        }
    }

    #endregion;

    #region handler

    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, int>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileManagerService _fileManager;

        public CreateExamCommandHandler(ISqlDbContext dbContext, ICurrentUserService currentUser, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _fileManager = fileManager;
        }

        public async Task<int> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var entity = new Exam
            {
                Title = request.Title,
                CreatedBy = _currentUser.DisplayName,
                LongDescription = request.LongDescription,
                ShortDescription = request.ShortDescription,
            };

            if (request.Image != null)
            {
                entity.Image = await _fileManager.CreateFileAsync(request.Image);
            }

            _dbContext.Exams.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }

    #endregion;
}
