using System;
using MediatR;
using Loby.Tools;
using System.Text;
using System.Linq;
using System.Threading;
using FluentValidation;
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

    public class DeleteOptionCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validator

    public class DeleteOptionCommandValidator : AbstractValidator<DeleteOptionCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteOptionCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeOptionExist).WithMessage("گزینه ای مطابق شناسه دریافتی یافت نشد");
        }

        public async Task<bool> BeOptionExist(int optionId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.QuestionOptions.AnyAsync(x => x.Id == optionId, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class DeleteOptionCommandHandler : IRequestHandler<DeleteOptionCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public DeleteOptionCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(DeleteOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var option = await _dbContext.QuestionOptions.FindAsync(request.Id);

                if (!string.IsNullOrEmpty(option.Image))
                {
                    await _fileManager.DeleteFileAsync(option.Image);
                }

                _dbContext.QuestionOptions.Remove(option);

                await _dbContext.SaveChangesAsync();

                return Result.Success("گزینه با موفقیت حذف گردید.");
            }
            catch
            {
                return Result.Failure("خطایی در حذف گزینه اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
