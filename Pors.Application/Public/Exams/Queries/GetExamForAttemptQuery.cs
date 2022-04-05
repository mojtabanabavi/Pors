using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Public.Exams.Queries
{
    #region query
    
    public class GetExamForAttemptQuery : IRequest<GetExamForAttemptQueryResponse>
    {
        public string AttemptId { get; set; }

        public GetExamForAttemptQuery()
        {
        }

        public GetExamForAttemptQuery(string attemptId)
        {
            AttemptId = attemptId;
        }
    }

    #endregion;

    #region response

    public class GetExamForAttemptQueryResponse : IMapFrom<Exam>
    {
        public int Id { get; set; }
        public string AttemptId { get; set; }
        public string Title { get; set; }
        public List<ExamQuestionDto> Questions { get; set; }
    }

    public class ExamQuestionDto : IMapFrom<ExamQuestion>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionOptionDto> Options { get; set; }
    }

    public class QuestionOptionDto : IMapFrom<QuestionOption>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamForAttemptQueryHandler : IRequestHandler<GetExamForAttemptQuery, GetExamForAttemptQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamForAttemptQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetExamForAttemptQueryResponse> Handle(GetExamForAttemptQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.ExamAttempts
                .Where(x => x.Id == request.AttemptId)
                .Include(x => x.Exam)
                .ThenInclude(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Select(x => x.Exam)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(ExamAttempt), request.AttemptId);
            }

            var result = new GetExamForAttemptQueryResponse()
            {
                AttemptId = request.AttemptId,
            };

            return _mapper.Map(entity, result);
        }
    }

    #endregion;
}
