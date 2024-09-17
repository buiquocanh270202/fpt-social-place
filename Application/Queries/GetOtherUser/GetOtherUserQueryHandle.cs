using Application.Queries.GetUserByUserId;
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

namespace Application.Queries.GetOtherUser
{
    public class GetOtherUserQueryHandle : IQueryHandler<GetOtherUserQuery, GetOtherUserQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetOtherUserQueryHandle(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetOtherUserQueryResult>> Handle(GetOtherUserQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var getusersetting = await _context.UserSettings.Include(x => x.Setting).Where(x => x.UserId == request.ViewUserId).ToListAsync();
            var getuser = await _context.UserProfiles
                                .Include(x => x.ContactInfo)
                                .Include(x => x.UserStatus)
                                .Include(x => x.AvataPhotos)
                                .Include(x => x.UserGender)
                                .Include(x => x.WebAffiliations)
                                .Include(x => x.UserSettings)
                                .Include(x => x.Role)
                                .Include(x => x.UserInterests)
                                .Include(x=>x.WorkPlaces)
                                .Include(x => x.UserRelationship)
                                .Include(x=>x.BlockUserUserIsBlockeds)
                                .Include(x=>x.BlockUserUsers)
                                .FirstOrDefaultAsync(x => x.UserId == request.ViewUserId);
            var getstatus = await _context.UserStatuses.ToListAsync();
            var getgender = await _context.Genders.ToListAsync();
            var getinteres = await _context.Interests.ToListAsync();
            var getrelationship = await _context.Relationships.ToListAsync();
            
            var result = _mapper.Map<GetOtherUserQueryResult>(getuser);
            result.ButtonFriend = true;
            if (getuser == null || getusersetting.Count == 0)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            if (getuser.IsActive == false)
            {
                throw new ErrorException(StatusCodeEnum.U02_Lock_User);
            }
            foreach (var item in getuser.BlockUserUserIsBlockeds)
            {
                if ((item.UserIsBlockedId == request.UserId
                    && item.UserId == request.ViewUserId)
                    || (item.UserIsBlockedId == request.ViewUserId 
                    && item.UserId == request.UserId))
                {
                    throw new ErrorException(StatusCodeEnum.U01_Not_Found);
                }
            }

            foreach (var item in getuser.BlockUserUsers)
            {
                if ((item.UserIsBlockedId == request.UserId
                    && item.UserId == request.ViewUserId)
                    || (item.UserIsBlockedId == request.ViewUserId
                    && item.UserId == request.UserId))
                {
                    throw new ErrorException(StatusCodeEnum.U01_Not_Found);
                }
            }
            
            if(getusersetting.FirstOrDefault(x=>x.Setting.SettingName.Equals("Profile Status")).UserStatusId 
                == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
            {
                result.ContactInfo = null;
                result.UserGender = null;
                result.UserRelationship = null;
                result.UserInterests = null;
                result.WorkPlaces = null;
                result.WebAffiliations = null;
                result.ButtonFriend = false;
                return Result<GetOtherUserQueryResult>.Success(result);
            }
            else if (getusersetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatusId
                == getstatus.FirstOrDefault(x => x.StatusName == "Public").UserStatusId)
            {
                if (getuser.ContactInfo.UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
                {
                    result.ContactInfo = null;
                }
                if (getuser.UserGender.UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
                {
                    result.UserGender = null;
                }
                if(getuser.UserRelationship != null)
                {
                    if (getuser.UserRelationship.UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
                    {
                        result.UserRelationship = null;
                    }
                }

                if (getuser.UserInterests.Count > 0)
                {
                    if (getuser.UserInterests.FirstOrDefault().UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
                    {
                        result.UserInterests = null;
                    }
                }
                if (getuser.WorkPlaces.Count > 0)
                {
                    if (getuser.WorkPlaces.FirstOrDefault().UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
                    {
                        result.WorkPlaces = null;
                    }
                }
                if (getuser.WebAffiliations.Count > 0)
                {
                    if (getuser.WebAffiliations.FirstOrDefault().UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId)
                    {
                        result.WebAffiliations = null;
                    }
                }
                var friend = await _context.Friends.FirstOrDefaultAsync(x => (x.UserId == request.UserId && x.FriendId == request.ViewUserId)
                                                    || (x.UserId == request.ViewUserId && x.FriendId == request.UserId));
                if (friend == null)
                {
                    if (getuser.ContactInfo.UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
                    {
                        result.ContactInfo = null;
                    }
                    if (getuser.UserGender.UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
                    {
                        result.UserGender = null;
                    }
                    if (getuser.UserRelationship != null)
                    {
                        if (getuser.UserRelationship.UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
                        {
                            result.UserRelationship = null;
                        }
                    }
                    if (getuser.UserInterests.Count > 0)
                    {
                        if (getuser.UserInterests.FirstOrDefault().UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
                        {
                            result.UserInterests = null;
                        }
                    }
                    if (getuser.WorkPlaces.Count > 0)
                    {
                        if (getuser.WorkPlaces.FirstOrDefault().UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
                        {
                            result.WorkPlaces = null;
                        }
                    }
                    if (getuser.WebAffiliations.Count > 0)
                    {
                        if (getuser.WebAffiliations.FirstOrDefault().UserStatusId == getstatus.FirstOrDefault(x => x.StatusName == "Friend").UserStatusId)
                        {
                            result.WebAffiliations = null;
                        }
                    }
                    if (getusersetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Send Friend Invitations")).UserStatusId
                        == getstatus.FirstOrDefault(x => x.StatusName == "Private").UserStatusId){
                        result.ButtonFriend = false;
                    }
                }
            }
           
            if(result.UserGender != null)
            {
                result.UserGender.GenderName = getgender.FirstOrDefault(x => x.GenderId == result.UserGender.GenderId).GenderName;
            }
            if(result.UserRelationship != null)
            {
                result.UserRelationship.RelationshipName = getrelationship.FirstOrDefault(x => x.RelationshipId == result.UserRelationship.RelationshipId).RelationshipName;
            }
            if(result.UserInterests != null)
            {
                foreach (var interes in result.UserInterests)
                {
                    interes.InteresName = getinteres.FirstOrDefault(x => x.InterestId == interes.InterestId).InterestName;
                }
            }
            
            return Result<GetOtherUserQueryResult>.Success(result);
        }
    }
}
