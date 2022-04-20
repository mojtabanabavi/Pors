using System;
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
using Pors.Application.Common.Validators;

namespace Pors.Application.Management.Profiles.Commands
{
    #region command

    public class UpdateProfileCommand : IRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateProfileCommandValidator(ISqlDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.FirstName)
                .MaximumLength(25)
                .WithName("نام");

            RuleFor(x => x.LastName)
                .MaximumLength(25)
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
                .Length(8, 50)
                .WithName("رمزعبور جدید");
        }

        private bool UniqueEmail(UpdateProfileCommand command, string email)
        {
            var entity = _dbContext.Users.Find(command.Id);

            if (entity?.Email == email)
            {
                return true;
            }

            return _dbContext.Users.All(x => x.Email != email);
        }

        private bool UniquePhoneNumber(UpdateProfileCommand command, string phoneNumber)
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

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly IFileManagerService _fileManager;

        public UpdateProfileCommandHandler(ISqlDbContext dbContext, IFileManagerService fileManager)
        {
            _dbContext = dbContext;
            _fileManager = fileManager;
        }

        public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            entity.Email = request.Email;
            entity.LastName = request.LastName;
            entity.FirstName = request.FirstName;
            entity.PhoneNumber = request.PhoneNumber;

            if (request.Password.HasValue())
            {
                entity.PasswordHash = PasswordHasher.Hash(request.Password);
            }

            if (request.ProfilePicture != null)
            {
                entity.ProfilePicture = await _fileManager.UpdateFileAsync(request.ProfilePicture, entity.ProfilePicture);
            }

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
