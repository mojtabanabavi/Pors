﻿using System;
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

namespace Pors.Application.Management.Profiles.Queries
{
    #region query

    public class GetProfileQuery : IRequest<GetProfileQueryResponse>
    {
    }

    #endregion;

    #region response

    public class GetProfileQueryResponse : IMapFrom<User>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string LastLoginDateTime { get; set; }
        public string RegisterDateTime { get; set; }
        public List<string> Roles { get; set; }
        public string DisplayName
        {
            get
            {
                string fullName = null;

                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                {
                    fullName = $"{FirstName} {LastName}";
                }

                return fullName ?? PhoneNumber ?? Email;
            }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetProfileQueryResponse>()
                .ForMember(x => x.Roles, option => option.MapFrom(y => y.UserRoles.Select(x => x.Role.Title)));
        }
    }

    #endregion;

    #region validator

    #endregion;

    #region handler

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, GetProfileQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISqlDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public GetProfileQueryHandler(ISqlDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<GetProfileQueryResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(_currentUser.UserId);

            var user = await _dbContext.Users
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync();

            var result = _mapper.Map<GetProfileQueryResponse>(user);

            return result;
        }
    }

    #endregion;
}
