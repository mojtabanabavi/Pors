using System;
using MediatR;
using System.Linq;
using Loby.Extensions;
using FluentValidation;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Roles.Commands
{
    #region command

    public class CreateRoleCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateRoleCommandValidator(ISqlDbContext sqlDbContext)
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

        private bool UniqueTitle(string title)
        {
            return _dbContext.Roles.All(x => x.Name != title);
        }
    }

    #endregion;

    #region handler

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
    {
        private readonly ISqlDbContext _dbContext;

        public CreateRoleCommandHandler(ISqlDbContext sqlDbContext)
        {
            _dbContext = sqlDbContext;
        }

        public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = new Role(request.Name, request.Description);

            _dbContext.Roles.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }
    }

    #endregion;
}
