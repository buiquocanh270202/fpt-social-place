using Application.DTO.GroupDTO;
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

namespace Application.Queries.GetGroupByGroupId
{
    public class GetGroupByGroupIdQueryHandler : IQueryHandler<GetGroupByGroupIdQuery, GetGroupByGroupIdQueryResult>
    {

        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupByGroupIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetGroupByGroupIdQueryResult>> Handle(GetGroupByGroupIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var group = await _context.GroupFpts.Include(x => x.GroupType)
                                                .Include(x => x.GroupTagUseds)
                                                .Include(x => x.GroupMembers)
                                                .Include(x=>x.CreatedBy)
                                                .FirstOrDefaultAsync(x => x.GroupId == request.GroupId && x.IsDelete != true);
            var groupsetting = await _context.GroupSettingUses.Include(x => x.GroupSetting)
                                                                .Include(x => x.GroupStatus)
                                                               .Where(x => x.GroupId == request.GroupId).ToListAsync();
            var member = await _context.GroupMembers
                                    .Where(x => x.GroupId == request.GroupId && x.IsJoined == true)
                                    .Include(x => x.GroupRole)
                                    .Include(x => x.User)
                                    .ToListAsync();

            var memjoin = await _context.GroupMembers.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.GroupId == request.GroupId);

            var admin = member?.FirstOrDefault(x => x.UserId == request.UserId && x.GroupRole.GroupRoleName.Equals("Admin"));
            var censor = member?.FirstOrDefault(x => x.UserId == request.UserId && x.GroupRole.GroupRoleName.Equals("Censor"));

            if (group == null)
            {
                throw new ErrorException(StatusCodeEnum.GR08_Group_Is_Not_Exist);
            }

            var getgroup = new GetGroupByGroupIdQueryResult
            {
                GroupId = (Guid)request.GroupId,
                GroupName = group.GroupName,
                GroupNumber = group.GroupNumber,
                GroupDescription = group.GroupDescription,
                CoverImage = group.CoverImage,
                GroupAdmin = group.CreatedBy.FullName,
                GroupTypeId = group.GroupTypeId,
                GroupTypeName = group.GroupType.GroupTypeName,
                CreateAt = group.CreatedDate,
            };
            foreach (var st in groupsetting)
            {
                var gst = new GroupSettingDTO
                {
                    GroupSettingId = st.GroupSettingId,
                    GroupSettingName = st.GroupSetting.GroupSettingName,
                    GroupStatusId = st.GroupStatusId,
                    GroupStatusName = st.GroupStatus.GroupStatusName
                };
                getgroup.GroupSettings.Add(gst);
            }
            getgroup.IsAdmin = false;
            getgroup.IsCensor = false;
            if (memjoin != null)
            {
                getgroup.isRequest = true;
                if (memjoin.IsJoined == false)
                {
                    getgroup.IsJoin = false;
                    
                }
                else
                {
                    getgroup.IsJoin = true;
                    if (admin != null)
                    {
                        getgroup.IsAdmin = true;
                        getgroup.IsCensor = false;

                    }
                    else if (censor != null)
                    {
                        getgroup.IsAdmin = false;
                        getgroup.IsCensor = true;

                    }

                }

                if(memjoin.InvatedBy != null && memjoin.IsInvated == false)
                {
                    getgroup.isRequest = false;
                }
            }
            else
            {
                getgroup.IsJoin = false;
                getgroup.isRequest = false;
            }


            foreach (var mem in member)
            {
                if (mem.IsJoined == true)
                {
                    var friend = await _context.Friends.FirstOrDefaultAsync(x =>
                                        ((x.UserId == request.UserId && x.FriendId == mem.UserId) ||
                                        (x.UserId == mem.UserId && x.FriendId == request.UserId)) && x.Confirm == true);
                    var user = await _context.AvataPhotos.FirstOrDefaultAsync(x => x.UserId == mem.UserId && x.IsUsed == true);
                    if (friend != null)
                    {
                        var gmem = new GroupMemberDTO
                        {
                            GroupId = mem.GroupId,
                            UserId = mem.UserId,
                            MemberName = mem.User.FirstName + " " + mem.User.LastName,
                            Avata = user?.AvataPhotosUrl,
                            GroupRoleId = mem.GroupRoleId,
                            GroupRoleName = mem.GroupRole.GroupRoleName
                        };
                        getgroup.GroupMember.Add(gmem);
                    }
                }
            }
            if (getgroup.GroupMember?.Count < 10)
            {
                foreach (var mem in member)
                {
                    if (mem.IsJoined == true)
                    {
                        var user = await _context.AvataPhotos.FirstOrDefaultAsync(x => x.UserId == mem.UserId && x.IsUsed == true);

                        var gmem = new GroupMemberDTO
                        {
                            GroupId = mem.GroupId,
                            UserId = mem.UserId,
                            MemberName = mem.User.FirstName + " " + mem.User.LastName,
                            Avata = user?.AvataPhotosUrl,
                            GroupRoleId = mem.GroupRoleId,
                            GroupRoleName = mem.GroupRole.GroupRoleName
                        };
                        var oldmem = getgroup.GroupMember.FirstOrDefault(x => x.UserId == mem.UserId);
                        if (oldmem == null) 
                        {
                            getgroup.GroupMember.Add(gmem);
                        } 

                    }
                    
                    if(getgroup.GroupMember.Count == 10)
                    {
                        break;
                    }
                }
            }
            if(getgroup.GroupMember?.Count > 0)
            {
                getgroup.GroupMember = getgroup.GroupMember.Distinct().ToList();
            }
            getgroup.MemberCount = member.Where(x=>x.IsJoined == true).ToList().Count;
            var result = getgroup;
            return Result<GetGroupByGroupIdQueryResult>.Success(result);
        }
    }
}
