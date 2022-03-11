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

namespace Pors.Application.Public.ExamAttempts.Queries
{
    #region query

    public class GetExamAttemptAnswersQuery : IRequest<GetExamAttemptAnswersQueryResponse>
    {
        public string AttemptId { get; set; }

        public GetExamAttemptAnswersQuery()
        {
        }

        public GetExamAttemptAnswersQuery(string attemptId)
        {
            AttemptId = attemptId;
        }
    }

    #endregion;

    #region response

    public class GetExamAttemptAnswersQueryResponse : IMapFrom<ExamAttempt>
    {
        public string ExamTitle { get; set; }
        public string AttemptId { get; set; }
        public List<ExamQuestionDto> Questions { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExamAttempt, GetExamAttemptAnswersQueryResponse>()
                .ForMember(x => x.AttemptId, option => option.MapFrom(y => y.Id))
                .ForMember(x => x.Questions, option => option.MapFrom(y => y.Answers))
                .ForMember(x => x.ExamTitle, option => option.MapFrom(y => y.Exam.Title));
        }
    }

    public class ExamQuestionDto : IMapFrom<AttemptAnswer>
    {
        public string Title { get; set; }
        public QuestionOptionDto Answer { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AttemptAnswer, ExamQuestionDto>()
                .ForMember(x => x.Answer, option => option.MapFrom(y => y.Option))
                .ForMember(x => x.Title, option => option.MapFrom(y => y.Question.Title));
        }
    }

    public class QuestionOptionDto : IMapFrom<QuestionOption>
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamAttemptAnswersQueryHandler : IRequestHandler<GetExamAttemptAnswersQuery, GetExamAttemptAnswersQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamAttemptAnswersQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetExamAttemptAnswersQueryResponse> Handle(GetExamAttemptAnswersQuery request, CancellationToken cancellationToken)
        {
            var attemptWithAnswer = await _dbContext.ExamAttempts
                .Where(x => x.Id == request.AttemptId)
                .Include(x => x.Exam)
                .Include(x => x.Answers)
                .ThenInclude(x => x.Option)
                .ThenInclude(x => x.Question)
                .SingleOrDefaultAsync();

            if (attemptWithAnswer == null)
            {
                throw new NotFoundException(nameof(Exam), request.AttemptId);
            }

            return _mapper.Map<GetExamAttemptAnswersQueryResponse>(attemptWithAnswer);
        }
    }

    #endregion;
}
