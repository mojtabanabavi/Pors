using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Public.AttemptAnswers.Commands
{
    #region command

    public class SaveAttemptAnswersCommand : IRequest<string>
    {
        public string AttemptId { get; set; }
        public List<AttemptAnswerItem> Answers { get; set; }
    }

    public class AttemptAnswerItem
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class SaveAttemptAnswersCommandCommandHandler : IRequestHandler<SaveAttemptAnswersCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public SaveAttemptAnswersCommandCommandHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<string> Handle(SaveAttemptAnswersCommand request, CancellationToken cancellationToken)
        {
            var answers = new List<AttemptAnswer>();

            foreach (var answer in request.Answers)
            {
                answers.Add(new AttemptAnswer
                {
                    OptionId = answer.OptionId,
                    QuestionId = answer.QuestionId,
                    AttemptId = request.AttemptId,
                });
            }

            _dbContext.AttemptAnswers.AddRange(answers);

            await _dbContext.SaveChangesAsync();

            return request.AttemptId;
        }
    }

    #endregion;
}
