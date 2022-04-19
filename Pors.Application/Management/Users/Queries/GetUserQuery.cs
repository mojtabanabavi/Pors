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

namespace Pors.Application.Management.Users.Queries
{
    #region query

    public class GetUserQuery : IRequest<GetUserQueryResponse>
    {
        public int Id { get; set; }

        public GetUserQuery()
        {
        }

        public GetUserQuery(int id)
        {
            Id = id;
        }
    }

    #endregion;

    #region response

    public class GetUserQueryResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string LastLoginDateTime { get; set; }
        public string RegisterDateTime { get; set; }
        public List<int> RoleIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserQueryResponse>()
                .ForMember(x => x.RoleIds, option => option.MapFrom(y => y.UserRoles.Select(x => x.RoleId)));
        }
    }

    #endregion;

    #region handler

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;

        public GetUserQueryHandler(ISqlDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Users
                .Include(x => x.UserRoles)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            var result = _mapper.Map<GetUserQueryResponse>(entity);

            return result;
        }
    }

    #endregion;
}