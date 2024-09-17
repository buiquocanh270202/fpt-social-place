using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateGroupCommand
{
    public class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, CreateGroupCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateGroupCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<CreateGroupCommandResult>> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var group = await _querycontext.GroupFpts.FirstOrDefaultAsync(x => x.GroupName.Equals(request.GroupName));
            var user = await _querycontext.UserProfiles.Include(x=>x.Role).FirstOrDefaultAsync(x => x.UserId == request.CreatedById);
            var grouptag = await _querycontext.GroupTags.ToListAsync();
            var status = await _querycontext.GroupStatuses.ToListAsync();
            var setting = await _querycontext.GroupSettings.ToListAsync();
            var groupstatus = await _querycontext.GroupStatuses.ToListAsync();
            var role = await _querycontext.GroupRoles.ToListAsync();
           
            if(request.GroupName == null || request.GroupName == "")
            {
                throw new ErrorException(StatusCodeEnum.GR07_Group_Name_Not_Null);
            }
            if(user == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }
            if(group != null)
            {
                throw new ErrorException(StatusCodeEnum.GR06_Group_Name_Existed);
            }
            var newgroup = new Domain.CommandModels.GroupFpt {
                GroupId = _helper.GenerateNewGuid(),
                GroupNumber = user.UserNumber?.ToLower() + _helper.GenerateNewGuid().ToString(),
                GroupName= request.GroupName,
                GroupDescription = request.GroupDescription,
                CreatedById = (Guid)request.CreatedById,
                CoverImage = request.CoverImage,
                CreatedDate = DateTime.Now,
                GroupTypeId = request.GroupTypeId,
                GroupStatusId = request.UserStatusId,
                IsDelete = false,
            };
            await _context.GroupFpts.AddAsync(newgroup);

            var newgrouptag = new Domain.CommandModels.GroupTagUsed
            {
                GroupId = newgroup.GroupId,
                GroupStatusId = status.FirstOrDefault(x=>x.GroupStatusName.Equals("Public")).GroupStatusId,
                CreatedAt = DateTime.Now,
                UpdateAt = null
            };
            if (user.Role.NameRole.Equals("Societe-student"))
            {
                newgrouptag.TagId = grouptag.FirstOrDefault(x => x.TagName.Equals("Student")).TagId;
            }

            if (user.Role.NameRole.Equals("Societe-staff"))
            {
                newgrouptag.TagId = grouptag.FirstOrDefault(x => x.TagName.Equals("Staff")).TagId;
            }
            if (user.Role.NameRole.Equals("Societe-member"))
            {
                newgrouptag.TagId = grouptag.FirstOrDefault(x => x.TagName.Equals("Member")).TagId;
            }

            await _context.GroupTagUseds.AddAsync(newgrouptag);

            foreach(var st in setting)
            {
                var newgroupsetting = new Domain.CommandModels.GroupSettingUse
                {
                    GroupId = newgroup.GroupId,
                    GroupSettingId = st.GroupSettingId,
                    GroupStatusId = groupstatus.FirstOrDefault(x => x.GroupStatusName.Equals("Private")).GroupStatusId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                await _context.GroupSettingUses.AddAsync(newgroupsetting);
            }

            var member = new Domain.CommandModels.GroupMember
            {
                GroupId = newgroup.GroupId,
                UserId  = (Guid)request.CreatedById,
                GroupRoleId = role.FirstOrDefault(x=>x.GroupRoleName.Equals("Admin")).GroupRoleId,
                IsJoined = true,
                JoinedDate = DateTime.Now
            };
            await _context.GroupMembers.AddAsync(member);

            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateGroupCommandResult>(newgroup);

            return Result<CreateGroupCommandResult>.Success(result);
        }
    }
}
