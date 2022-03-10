using System;
using MediatR;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.ExamVisits.Commands
{
    #region command

    public class CreateExamVisitCommand : IRequest<int>
    {
        public int ExamId { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class CreateExamVisitCommandCommandHandler : IRequestHandler<CreateExamVisitCommand, int>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public CreateExamVisitCommandCommandHandler(ISqlDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(CreateExamVisitCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyVisited = _dbContext.ExamVisits
                .Any(x => x.ExamId == request.ExamId && x.IpAddress == _currentUser.IpAddress);

            if (!isAlreadyVisited)
            {
                var entity = new ExamVisit
                {
                    ExamId = request.ExamId,
                    IpAddress = _currentUser.IpAddress,
                };

                _dbContext.ExamVisits.Add(entity);

                await _dbContext.SaveChangesAsync();

                return entity.Id;
            }

            return 0;
        }
    }

    #endregion;
}
