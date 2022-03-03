using System;
using MediatR;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Roles.Commands
{
    #region command

    public class UpdateRoleCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateRoleCommandValidator(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50)
                .Must(UniqueTitle).WithMessage("'{PropertyName}' تکراری است.")
                .WithName("عنوان");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .When(x => x.Description.HasValue())
                .WithName("توضیحات");
        }

        private bool UniqueTitle(UpdateRoleCommand command, string title)
        {
            var entity = _dbContext.Roles.Find(command.Id);

            if (entity?.Name == title)
            {
                return true;
            }

            return _dbContext.Roles.All(x => x.Name != title);
        }
    }

    #endregion;

    #region handler

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public UpdateRoleCommandHandler(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;
        }

        public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Role), request.Id);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }

    #endregion;
}
