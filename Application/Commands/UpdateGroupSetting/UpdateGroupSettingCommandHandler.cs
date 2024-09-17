using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
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

namespace Application.Commands.UpdateGroupSetting
{
    public class UpdateGroupSettingCommandHandler : ICommandHandler<UpdateGroupSettingCommand, UpdateGroupSettingCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public UpdateGroupSettingCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<UpdateGroupSettingCommandResult>> Handle(UpdateGroupSettingCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var grouprole = await _querycontext.GroupMembers.Include(x => x.GroupRole)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId
                                     && x.GroupId == request.GroupId);
       

            if (grouprole.GroupRole.GroupRoleName.Equals("Admin"))
            {
               if(request.updateSettingDTOs?.Count > 0)
                {
                    foreach (var item in request.updateSettingDTOs)
                    {
                        var groupsetting = await _querycontext.GroupSettingUses
                            .FirstOrDefaultAsync(x => x.GroupId == request.GroupId && x.GroupSettingId == item.SettingId);
                        var newsetting = new Domain.CommandModels.GroupSettingUse
                        {
                            GroupId = request.GroupId,
                            GroupSettingId = item.SettingId,
                            GroupStatusId = item.GroupStatusId,
                            CreatedAt = groupsetting?.CreatedAt,
                            UpdatedAt = DateTime.Now
                        };
                        _context.GroupSettingUses.Update(newsetting);
                        await _context.SaveChangesAsync();
                    }
                }
               
            }
            else
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }

            var result = new UpdateGroupSettingCommandResult {Message = "Update Setting Success!" };
            return Result<UpdateGroupSettingCommandResult>.Success(result);
        }
    }
}
