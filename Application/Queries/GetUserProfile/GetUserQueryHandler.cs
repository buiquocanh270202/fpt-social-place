
using Application.DTO.GetUserProfileDTO;
using Application.Queries.GetUserProfile;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserProfile
{
    public class GetUserQueryHandler: IQueryHandler<GetUserQuery, GetUserQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetUserQueryResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if(_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var user = await _context.UserProfiles
                                    .Include(x => x.ContactInfo)
                                    .Include(x => x.UserStatus)
                                    .Include(x => x.AvataPhotos)
                                    .Include(x => x.UserGender)
                                    .Include(x => x.WebAffiliations)
                                    .Include(x => x.UserSettings)
                                    .Include(x => x.Role)
                                    .Include(x => x.UserGender.Gender)
                                    .Include(x => x.UserInterests)
                                    .Include(x => x.UserRelationship)
                                    .FirstOrDefaultAsync(x => x.UserNumber == request.UserNumber);
            if (user == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }
            if(user.IsActive == false)
            {
                throw new ErrorException(StatusCodeEnum.U02_Lock_User);
            }
            var result = _mapper.Map<GetUserQueryResult>(user);
            foreach (var webaff in user.WebAffiliations)
            {
                var mapweb = _mapper.Map<GetUserWebAfflication>(webaff);
                result.WebAffiliationUrl.Add(mapweb);
            }

            foreach (var avata in user.AvataPhotos)
            {
                var mapavata = _mapper.Map<GetUserWebAfflication>(avata);
                result.WebAffiliationUrl.Add(mapavata);
            }
            result.ContactInfo = _mapper.Map<GetUserContactInfo>(user.ContactInfo);
            return Result<GetUserQueryResult>.Success(result);
        }
    }
}
