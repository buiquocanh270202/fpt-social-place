using Application.DTO.GetUserProfileDTO;
using Application.Queries.GetUserProfile;
using Application.Queries.GetWebAffilication;
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

namespace Application.Queries.GetUserByUserId
{
    public class GetUserByUserIdHandler : IQueryHandler<GetUserByUserIdQuery, GetUserByUserIdResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserByUserIdHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetUserByUserIdResult>> Handle(GetUserByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var getuser = await _context.UserProfiles
                                .Include(x => x.ContactInfo)
                                .Include(x => x.UserStatus)
                                .Include(x => x.AvataPhotos)
                                .Include(x => x.UserGender)
                                .Include(x => x.WebAffiliations)
                                .Include(x => x.UserSettings)
                                .Include(x => x.Role)
                                .Include(x=>x.WorkPlaces)
                                .Include(x => x.UserInterests)
                                .Include(x => x.UserRelationship)
                                .FirstOrDefaultAsync(x => x.UserId == request.UserId);
            var getgender = await _context.Genders.ToListAsync();
            var getinteres = await _context.Interests.ToListAsync();
            var getrelationship = await _context.Relationships.ToListAsync();

            if(getuser == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            if (getuser.IsActive == false)
            {
                throw new ErrorException(StatusCodeEnum.U02_Lock_User);
            }
            var result = _mapper.Map<GetUserByUserIdResult>(getuser);
            result.UserGender.GenderName = getgender.FirstOrDefault(x=>x.GenderId == result.UserGender.GenderId).GenderName;
            if(result.UserRelationship != null)
            {
                result.UserRelationship.RelationshipName = getrelationship.FirstOrDefault(x => x.RelationshipId == result.UserRelationship.RelationshipId).RelationshipName;
            }
            if (result.UserInterests != null || result.UserInterests.Count > 0)
            {
                foreach (var interes in result.UserInterests)
                {
                    interes.InteresName = getinteres.FirstOrDefault(x => x.InterestId == interes.InterestId).InterestName;
                }

            }

            return Result<GetUserByUserIdResult>.Success(result);
            //var user = await _context.UserProfiles
            //                        .Include(x => x.ContactInfo)
            //                        .Include(x => x.UserStatus)
            //                        .Include(x => x.AvataPhotos)
            //                        .Include(x => x.UserGender)
            //                        .Include(x => x.WebAffiliations)
            //                        .Include(x => x.UserSettings)
            //                        .Include(x => x.Role)
            //                        .Include(x => x.UserRelationship)
            //                        .Include(x => x.UserGender.Gender)
            //                        .Include(x => x.UserRelationship)
            //                        .ToListAsync();
            //List<GetUserByUserIdResult> resultlist = new List<GetUserByUserIdResult>();
            //foreach (var data in user)
            //{
            //  var map  = _mapper.Map<GetUserByUserIdResult>(data);
            //   foreach(var webaff in data.WebAffiliations)
            //    {
            //        var mapweb = _mapper.Map<GetUserWebAfflication>(webaff);
            //        map.WebAffiliationUrl.Add(mapweb);
            //    }

            //    foreach (var avata in data.AvataPhotos)
            //    {
            //        var mapavata = _mapper.Map<GetUserAvatar>(avata);
            //        map.AvataPhotosUrl.Add(mapavata);
            //    }
            //    map.ContactInfo = _mapper.Map<GetUserContactInfo>(data.ContactInfo);
            //    resultlist.Add(map);
            //}
            //return Result<List<GetUserByUserIdResult>>.Success(resultlist);
        }
    }
}
