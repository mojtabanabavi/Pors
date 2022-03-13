using System;
using MediatR;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.Exams.Commands
{
    #region command

    public class CreateExamAttemptCommand : IRequest<string>
    {
        public int ExamId { get; set; }

        public CreateExamAttemptCommand(int examId)
        {
            ExamId = examId;
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class CreateExamAttemptCommandCommandHandler : IRequestHandler<CreateExamAttemptCommand, string>
    {
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public CreateExamAttemptCommandCommandHandler(ISqlDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<string> Handle(CreateExamAttemptCommand request, CancellationToken cancellationToken)
        {
            //var isAlreadyVisited = _dbContext.ExamAttempts
            //    .Any(x => x.ExamId == request.ExamId && x.IpAddress == _currentUser.IpAddress);

            //if (!isAlreadyVisited)
            //{
            var entity = new ExamAttempt
            {
                ExamId = request.ExamId,
                IpAddress = _currentUser.IpAddress,
            };

            _dbContext.ExamAttempts.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity.Id;
            //}

            //return string.Empty;
        }
    }

    #endregion;k
}
