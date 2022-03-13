using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Answers.Queries
{
    #region query

    public class GetAnswerQuery : IRequest<GetAnswerQueryResponse>
    {
        public int Id { get; set; }

        public GetAnswerQuery()
        {
        }

        public GetAnswerQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetAnswerQueryResponse : IMapFrom<AttemptAnswer>
    {
        public int Id { get; set; }
        public string AttemptId { get; set; }
        public int OptionId { get; set; }
        public bool IsCorrect { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetAnswerQueryHandler : IRequestHandler<GetAnswerQuery, GetAnswerQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetAnswerQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAnswerQueryResponse> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.AttemptAnswers.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(AttemptAnswer), request.Id);
            }

            var result = _mapper.Map<GetAnswerQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}
