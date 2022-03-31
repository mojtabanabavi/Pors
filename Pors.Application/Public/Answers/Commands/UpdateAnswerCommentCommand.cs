﻿using System;
using MediatR;
using System.Threading;
using FluentValidation;
using Pors.Domain.Enums;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Public.Answers.Commands
{
    #region command

    public class UpdateAnswerCommentCommand : IRequest
    {
        public int Id { get; set; }
        public AnswerStatus CorrectStatus { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateAnswerCommentCommandValidator : AbstractValidator<UpdateAnswerCommentCommand>
    {
        public UpdateAnswerCommentCommandValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithName("توضیحات");
        }
    }

    #endregion;

    #region handler

    public class UpdateAnswerCommentCommandCommandHandler : IRequestHandler<UpdateAnswerCommentCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateAnswerCommentCommandCommandHandler(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateAnswerCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = _dbContext.AttemptAnswers.Find(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(AttemptAnswer), request.Id);
            }

            entity.Status = request.CorrectStatus;
            entity.Description = request.Description;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
