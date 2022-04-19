using System;
using MediatR;
using AutoMapper;
using System.Threading;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;
using Pors.Application.Common.Exceptions;

namespace Pors.Application.Management.Roles.Queries
{
    #region query

    public class GetRoleQuery : IRequest<GetRoleQueryResponse>
    {
        public int Id { get; set; }

        public GetRoleQuery()
        {
        }

        public GetRoleQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetRoleQueryResponse : IMapFrom<Role>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    #endregion;

    #region handler

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, GetRoleQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetRoleQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetRoleQueryResponse> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Roles.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Role), request.Id);
            }

            var result = _mapper.Map<GetRoleQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}