using Application.DTO.GroupDTO;
using Application.Queries.GetFriendyName;
using Application.Queries.GetListMemberRole;
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

namespace Application.Queries.GetMemberInGroup
{
    public class GetMemberInGroupQueryHandler : IQueryHandler<GetMemberInGroupQuery, GetMemberInGroupQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetMemberInGroupQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetMemberInGroupQueryResult>> Handle(GetMemberInGroupQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new GetMemberInGroupQueryResult();

            var joined = await _context.GroupMembers
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId && x.IsJoined == true);
           

            if(joined == null)
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }

            var member = await _context.GroupMembers
             .Where(x => x.GroupId == request.GroupId && x.IsJoined == true)
             .Include(x => x.GroupRole)
             .Include(x => x.User)
             .ThenInclude(x => x.AvataPhotos)
             .ToListAsync();

            if (member?.Count > 0)
            {
                foreach (var item in member)
                {
                    var friend = await _context.Friends.FirstOrDefaultAsync(x =>
                                        ((x.UserId == request.UserId && x.FriendId == item.UserId) ||
                                        (x.UserId == item.UserId && x.FriendId == request.UserId)));
                    var listSetting = await _context.UserSettings.Include(x => x.Setting)
                                        .Include(x => x.UserStatus).Where(x => x.UserId == item.UserId).ToListAsync();

                    var mem = new GetMemberInGroupDTO
                    {
                        UserId = item.UserId,
                        GroupId = request.GroupId,
                        MemberName = item.User.FirstName + " " + item.User.LastName,
                        Avata = item.User.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                        GroupRoleId = item.GroupRoleId,
                        GroupRoleName = item.GroupRole.GroupRoleName
                    };
                    if(friend != null)
                    {
                        if(friend.Confirm == true)
                        {
                            mem.IsFriend = true;
                            mem.IsRequested = false;
                            mem.SendFriendRequest = false;
                        }
                        else
                        {
                            mem.IsFriend = false;
                            mem.IsRequested = true;
                            mem.SendFriendRequest = false;
                        }
                        
                    }
                    else
                    {
                        mem.IsFriend = false;
                        mem.IsRequested = false;
                        mem.SendFriendRequest = true;
                        if (listSetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatus.StatusName.Equals("Private"))
                        {
                            mem.SendFriendRequest = false;
                        }
                        if (listSetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status")).UserStatus.StatusName.Equals("Friend"))
                        {
                            mem.SendFriendRequest = false;
                        }

                        if ((listSetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Send Friend Invitations")).UserStatus.StatusName.Equals("Private"))){
                            mem.SendFriendRequest = false;
                        }
                    }

                    if (item.GroupRole.GroupRoleName.Equals("Admin"))
                    {
                        result.GroupAdmin.Add(mem);
                    }
                    if (item.GroupRole.GroupRoleName.Equals("Censor"))
                    {
                        result.GroupMangager.Add(mem);
                    }
                    if (item.GroupRole.GroupRoleName.Equals("Member"))
                    {
                        result.GroupMember.Add(mem);
                    }
                }
            }
            result.GroupAdmin = result.GroupAdmin.OrderByDescending(m => m.IsFriend).ToList();
            result.GroupMangager = result.GroupMangager.OrderByDescending(m => m.IsFriend).ToList();
            result.GroupMember = result.GroupMember.OrderByDescending(m => m.IsFriend).ToList();
            return Result<GetMemberInGroupQueryResult>.Success(result);
        }
    }
}
