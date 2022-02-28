using System;
using MediatR;
using Loby.Tools;
using System.Text;
using System.Linq;
using FluentValidation;
using System.Threading;
using Pors.Domain.Enums;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Options.Commands
{
    #region command

    public class CreateOptionCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public List<CreateOptionItems> Items { get; set; }

        public class CreateOptionItems
        {
            public string Title { get; set; }
            public IFormFile Image { get; set; }
        }
    }

    #endregion;

    #region validator

    public class CreateOptionCommandValidator : AbstractValidator<CreateOptionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateOptionCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeQuestionExist).WithMessage("سوال درخواست شده یافت نشد.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("وارد کردن حداقل یک گزینه الزامی است.")
                .NotEmpty().WithMessage("وارد کردن حداقل یک گزینه الزامی است.");

            RuleForEach(x => x.Items)
                .ChildRules(x =>
                {
                    x.RuleFor(x=> x.Title)
                        .NotNull().WithMessage("وارد کردن عنوان الزامی است.")
                        .NotEmpty().WithMessage("وارد کردن عنوان الزامی است.");
                });
        }

        public async Task<bool> BeQuestionExist(int QuestionId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.ExamQuestions.AnyAsync(x => x.Id == QuestionId, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class CreateOptionCommandHandler : IRequestHandler<CreateOptionCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public CreateOptionCommandHandler(ISqlDbContext sqlDbContext, IFileManagerService fileManager)
        {
            _dbContext = sqlDbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(CreateOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var options = new List<QuestionOption>();

                foreach (var requestOption in request.Items)
                {
                    var option = new QuestionOption
                    {
                        QuestionId = request.Id,
                        Title = requestOption.Title,
                    };

                    if (requestOption.Image != null)
                    {
                        option.Image = await _fileManager.CreateFileAsync(requestOption.Image);
                    }

                    options.Add(option);
                }

                _dbContext.QuestionOptions.AddRange(options);

                await _dbContext.SaveChangesAsync();

                return Result.Success("گزینه ها با موفقیت ایجاد شدند.");
            }
            catch
            {
                return Result.Failure("خطایی در ایجاد گزینه ها اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
