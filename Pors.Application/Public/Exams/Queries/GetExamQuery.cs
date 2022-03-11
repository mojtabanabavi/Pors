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

    public class GetExamQuery : IRequest<GetExamQueryResponse>
    {
        public int Id { get; set; }
        public string AttemptId { get; set; }

        public GetExamQuery()
        {
        }

        public GetExamQuery(int id, string attemptId)
        {
            Id = id;
            AttemptId = attemptId;
        }
    }

    #endregion;

    #region response

    public class GetExamQueryResponse : IMapFrom<Exam>
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

    public class GetExamQueryHandler : IRequestHandler<GetExamQuery, GetExamQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetExamQueryResponse> Handle(GetExamQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Exams
                .Where(x => x.Id == request.Id)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exam), request.Id);
            }

            var result = new GetExamQueryResponse()
            {
                AttemptId = request.AttemptId,
            };

            return _mapper.Map(entity, result);
        }
    }

    #endregion;
}
