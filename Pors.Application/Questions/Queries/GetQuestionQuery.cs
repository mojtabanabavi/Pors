using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Questions.Queries
{
    #region query

    public class GetQuestionQuery : IRequest<GetQuestionQueryResponse>
    {
        public int Id { get; set; }

        public GetQuestionQuery()
        {
        }

        public GetQuestionQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetQuestionQueryResponse : IMapFrom<ExamQuestion>
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, GetQuestionQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetQuestionQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetQuestionQueryResponse> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.ExamQuestions.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ExamQuestion), request.Id);
            }

            var result = _mapper.Map<GetQuestionQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}