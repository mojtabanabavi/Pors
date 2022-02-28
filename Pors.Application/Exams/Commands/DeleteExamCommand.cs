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

namespace Pors.Application.Exams.Commands
{
    #region command

    public class DeleteExamCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region validator

    public class DeleteExamCommandValidator : AbstractValidator<DeleteExamCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public DeleteExamCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Id)
                .MustAsync(BeExamExist).WithMessage("آزمونی مطابق شناسه دریافتی یافت نشد");
        }

        public async Task<bool> BeExamExist(int examId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Exams.AnyAsync(x => x.Id == examId, cancellationToken);

            return result;
        }
    }

    #endregion;

    #region handler

    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, Result>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public DeleteExamCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Result> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exam = await _dbContext.Exams.FindAsync(request.Id);

                await _fileManager.DeleteFileAsync(exam.Image);

                _dbContext.Exams.Remove(exam);

                await _dbContext.SaveChangesAsync();

                return Result.Success("آزمون با موفقیت حذف گردید.");
            }
            catch
            {
                return Result.Failure("خطایی در حذف آزمون اتفاق افتاد.");
            }
        }
    }

    #endregion;
}
