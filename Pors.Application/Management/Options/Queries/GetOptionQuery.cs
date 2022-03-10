using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Options.Queries
{
    #region query

    public class GetOptionQuery : IRequest<GetOptionQueryResponse>
    {
        public int Id { get; set; }

        public GetOptionQuery()
        {
        }

        public GetOptionQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetOptionQueryResponse : IMapFrom<QuestionOption>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetOptionQueryHandler : IRequestHandler<GetOptionQuery, GetOptionQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetOptionQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetOptionQueryResponse> Handle(GetOptionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.QuestionOptions.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(QuestionOption), request.Id);
            }

            var result = _mapper.Map<GetOptionQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}
