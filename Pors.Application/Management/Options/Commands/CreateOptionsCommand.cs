using System;
using MediatR;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Options.Commands
{
    #region command

    public class CreateOptionsCommand : IRequest<int[]>
    {
        public int Id { get; set; }
        public List<CreateOptionItems> Items { get; set; }

        public CreateOptionsCommand()
        {
        }

        public CreateOptionsCommand(int id)
        {
            Id = id;
        }

        public class CreateOptionItems
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public IFormFile Image { get; set; }
        }
    }

    #endregion;

    #region validator

    public class CreateOptionsCommandValidator : AbstractValidator<CreateOptionsCommand>
    {
        public CreateOptionsCommandValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty()
                .WithName("گزینه‌ها");

            RuleForEach(x => x.Items)
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Title)
                        .NotEmpty()
                        .WithName("‌عنوان");
                });
        }
    }

    #endregion;

    #region handler

    public class CreateOptionsCommandHandler : IRequestHandler<CreateOptionsCommand, int[]>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public CreateOptionsCommandHandler(ISqlDbContext sqlDbContext, IFileManagerService fileManager)
        {
            _dbContext = sqlDbContext;
            _fileManager = fileManager;
        }

        public async Task<int[]> Handle(CreateOptionsCommand request, CancellationToken cancellationToken)
        {
            var isQuestionExist = _dbContext.ExamQuestions.Any(x => x.Id == request.Id);

            if (!isQuestionExist)
            {
                throw new NotFoundException(nameof(ExamQuestion), request.Id);
            }

            var options = new List<QuestionOption>();

            foreach (var requestOption in request.Items)
            {
                var option = new QuestionOption
                {
                    QuestionId = request.Id,
                    Title = requestOption.Title,
                    Description = requestOption.Description,
                };

                if (requestOption.Image != null)
                {
                    option.Image = await _fileManager.CreateFileAsync(requestOption.Image);
                }

                options.Add(option);
            }

            _dbContext.QuestionOptions.AddRange(options);

            await _dbContext.SaveChangesAsync();

            var optionIds = options.Select(x => x.Id).ToArray();

            return optionIds;
        }
    }

    #endregion;
}
