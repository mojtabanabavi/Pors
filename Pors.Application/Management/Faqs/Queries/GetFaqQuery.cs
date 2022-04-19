using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Faqs.Queries
{
    #region query

    public class GetFaqQuery : IRequest<GetFaqQueryResponse>
    {
        public int Id { get; set; }

        public GetFaqQuery()
        {
        }

        public GetFaqQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetFaqQueryResponse : IMapFrom<Faq>
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string CreatedAt { get; set; }
    }

    #endregion;

    #region handler

    public class GetFaqQueryHandler : IRequestHandler<GetFaqQuery, GetFaqQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetFaqQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetFaqQueryResponse> Handle(GetFaqQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Faqs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Faq), request.Id);
            }

            var result = _mapper.Map<GetFaqQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}
