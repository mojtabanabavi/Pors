using System;
using MediatR;
using Loby.Tools;
using System.Linq;
using Loby.Extensions;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Validators;

namespace Pors.Application.Management.Users.Commands
{
    #region command

    public class CreateUserCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
        public List<int> RoleIds { get; set; }
    }

    #endregion;

    #region validation

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateUserCommandValidator(ISqlDbContext dbContext)
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
                .MaximumLength(11)
                .ValidPhoneNumber()
                .Must(UniquePhoneNumber).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("موبایل");

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(320)
                .ValidEmailAddress()
                .Must(UniqueEmail).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("ایمیل");

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 50)
                .WithName("رمزعبور");
        }

        private bool UniqueEmail(string email)
        {
            return _dbContext.Users.All(x => x.Email != email);
        }

        private bool UniquePhoneNumber(string phoneNumber)
        {
            return _dbContext.Users.All(x => x.PhoneNumber != phoneNumber);
        }
    }

    #endregion;

    #region handler

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public CreateUserCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = new User
            {
                Email = request.Email,
                LastName = request.LastName,
                IsActive = request.IsActive,
                FirstName = request.FirstName,
                PhoneNumber = request.PhoneNumber,
                IsEmailConfirmed = request.IsEmailConfirmed,
                PasswordHash = PasswordHasher.Hash(request.Password),
                IsPhoneNumberConfirmed = request.IsPhoneNumberConfirmed,
            };

            if (!request.RoleIds.IsNullOrEmpty())
            {
                entity.UserRoles = request.RoleIds
                    .Select(roleId => new UserRole(roleId)).ToList();
            }

            if (request.ProfilePicture != null)
            {
                entity.ProfilePicture = await _fileManager.CreateFileAsync(request.ProfilePicture);
            }

            _dbContext.Users.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }

    #endregion;
}