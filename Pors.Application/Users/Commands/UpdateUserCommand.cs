﻿using System;
using MediatR;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Users.Commands
{
    #region command

    public class UpdateUserCommand : IRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion;

    #region validation

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateUserCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(25)
                .When(x => x.FirstName.HasValue())
                .WithName("نام");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(25)
                .When(x => x.LastName.HasValue())
                .WithName("نام خانوادگی");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(11)
                .Matches(@"^0?9\d{9}$").WithMessage("'{PropertyName}' معتبر نمی‌باشد.")
                .Must(UniquePhoneNumber).WithMessage("'{PropertyName}' تکراری است.")
                .When(x => x.PhoneNumber.HasValue())
                .WithName("موبایل");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(320)
                .Must(ValidEmail).WithMessage("'{PropertyName}' معتبر نمی‌باشد.")
                .Must(UniqueEmail).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("ایمیل");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 50)
                .When(x => x.Password.HasValue())
                .WithName("رمزعبور");
        }

        private bool ValidEmail(string email)
        {
            return Validator.IsValidEmail(email);
        }

        private bool UniqueEmail(UpdateUserCommand command, string email)
        {
            var entity = _dbContext.Users.Find(command.Id);

            if (entity?.Email == email)
            {
                return true;
            }

            return _dbContext.Users.All(x => x.Email != email);
        }

        private bool UniquePhoneNumber(UpdateUserCommand command, string phoneNumber)
        {
            var entity = _dbContext.Users.Find(command.Id);

            if (entity?.PhoneNumber == phoneNumber)
            {
                return true;
            }

            return _dbContext.Users.All(x => x.PhoneNumber != phoneNumber);
        }
    }

    #endregion;

    #region handler

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateUserCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            entity.Email = request.Email;
            entity.LastName = request.LastName;
            entity.IsActive = request.IsActive;
            entity.FirstName = request.FirstName;
            entity.PhoneNumber = request.PhoneNumber;
            entity.IsEmailConfirmed = request.IsEmailConfirmed;
            entity.IsPhoneNumberConfirmed = request.IsPhoneNumberConfirmed;

            if (request.Password.HasValue())
            {
                entity.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            if (request.ProfilePicture != null)
            {
                await _fileManager.DeleteFileAsync(entity.ProfilePicture);

                entity.ProfilePicture = await _fileManager.CreateFileAsync(request.ProfilePicture);
            }

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
