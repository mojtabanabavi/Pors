using System;
using MediatR;
using Loby.Tools;
using AutoMapper;
using System.Text;
using System.Linq;
using System.Threading;
using FluentValidation;
using Pors.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Pors.Application.Common.Models;
using Pors.Application.Common.Mappings;
using Pors.Application.Common.Interfaces;

namespace Pors.Application.Profiles.Queries
{
    #region query

    public class GetProfileQuery : IRequest<GetProfileQueryResponse>
    {
    }

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

        public GetProfileQueryHandler(ISqlDbContext dbContext,IMapper mapper, ICurrentUserService currentUser)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<GetProfileQueryResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(_currentUser.UserId);

            var user = await _dbContext.Users.FindAsync(userId);

            var result = _mapper.Map<GetProfileQueryResponse>(user);

            return result;
        }
    }

    #endregion;
}
