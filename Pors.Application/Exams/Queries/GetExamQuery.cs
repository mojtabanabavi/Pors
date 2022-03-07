using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Exams.Queries
{
    #region query

    public class GetExamQuery : IRequest<GetExamQueryResponse>
    {
        public int Id { get; set; }

        public GetExamQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetExamQueryResponse : IMapFrom<Exam>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
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
            var entity = await _dbContext.Exams.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exam), request.Id);
            }

            var result = _mapper.Map<GetExamQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}
