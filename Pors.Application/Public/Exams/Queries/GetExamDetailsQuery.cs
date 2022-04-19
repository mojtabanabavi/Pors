using System;
using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Public.Exams.Queries
{
    #region query

    public class GetExamDetailsQuery : IRequest<GetExamDetailsQueryResponse>
    {
        public int Id { get; set; }
    }

    #endregion;

    #region response

    public class GetExamDetailsQueryResponse : IMapFrom<Exam>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Image { get; set; }
        public string CreatedAt { get; set; }
        public int AttemptsCount { get; set; }
        public int QuestionsCount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Exam, GetExamDetailsQueryResponse>()
                .ForMember(x => x.AttemptsCount, option => option.MapFrom(y => y.Attempts.Count()))
                .ForMember(x => x.QuestionsCount, option => option.MapFrom(y => y.Questions.Count()));
        }
    }

    #endregion;

    #region handler

    public class GetExamDetailsQueryHandler : IRequestHandler<GetExamDetailsQuery, GetExamDetailsQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetExamDetailsQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetExamDetailsQueryResponse> Handle(GetExamDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Exams
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Attempts)
                .Include(x => x.Questions)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Exam), request.Id);
            }

            var result = _mapper.Map<GetExamDetailsQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}
