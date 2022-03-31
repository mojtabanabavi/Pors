using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Pors.Domain.Enums;
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

    public class GetExamResultQuery : IRequest<GetExamResultQueryResponse>
    {
        public string AttemptId { get; set; }

        public GetExamResultQuery()
        {
        }

        public GetExamResultQuery(string attemptId)
        {
            AttemptId = attemptId;
        }
    }

    #endregion;

    #region response

    public class GetExamResultQueryResponse : IMapFrom<ExamAttempt>
    {
        public string ExamTitle { get; set; }
        public string AttemptId { get; set; }
        public List<ExamAnswerResultDto> Answers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExamAttempt, GetExamResultQueryResponse>()
                .ForMember(x => x.AttemptId, option => option.MapFrom(y => y.Id))
                .ForMember(x => x.Answers, option => option.MapFrom(y => y.Answers))
                .ForMember(x => x.ExamTitle, option => option.MapFrom(y => y.Exam.Title));
        }
    }

    public class ExamAnswerResultDto : IMapFrom<AttemptAnswer>
    {
        public int Id { get; set; }
        public string QuestionTitle { get; set; }
        public string AnswerTitle { get; set; }
        public string AnswerImage { get; set; }
        public string AnswerDescription { get; set; }
        public AnswerStatus CommentStatus { get; set; }
        public string CommentDescription { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AttemptAnswer, ExamAnswerResultDto>()
                .ForMember(x => x.CommentStatus, option => option.MapFrom(y => y.Status))
                .ForMember(x => x.AnswerTitle, option => option.MapFrom(y => y.Option.Title))
                .ForMember(x => x.AnswerImage, option => option.MapFrom(y => y.Option.Image))
                .ForMember(x => x.CommentDescription, option => option.MapFrom(y => y.Description))
                .ForMember(x => x.QuestionTitle, option => option.MapFrom(y => y.Option.Question.Title))
                .ForMember(x => x.AnswerDescription, option => option.MapFrom(y => y.Option.Description));
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetExamResultQueryHandler : IRequestHandler<GetExamResultQuery, GetExamResultQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamResultQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetExamResultQueryResponse> Handle(GetExamResultQuery request, CancellationToken cancellationToken)
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

            return _mapper.Map<GetExamResultQueryResponse>(attemptWithAnswer);
        }
    }

    #endregion;
}
