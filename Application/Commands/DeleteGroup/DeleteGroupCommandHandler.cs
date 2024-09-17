using Application.Commands.UpdateGroupInfor;
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

namespace Application.Commands.DeleteGroup
{
    public class DeleteGroupCommandHandler : ICommandHandler<DeleteGroupCommand, DeleteGroupCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;


        public DeleteGroupCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;

        }
        public async Task<Result<DeleteGroupCommandResult>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var user = await _context.AdminProfiles.FirstOrDefaultAsync(x => x.AdminId == request.UserId);
            var adminrole = await _context.Roles.FirstOrDefaultAsync(x => x.NameRole.Equals("Societe-admin"));
            var result = new DeleteGroupCommandResult();
            var grouprole = await _querycontext.GroupMembers.Include(x => x.GroupRole)
               .FirstOrDefaultAsync(x => x.UserId == request.UserId
                                    && x.GroupId == request.GroupId);
            var group = await _querycontext.GroupFpts.FirstOrDefaultAsync(x => x.GroupId == request.GroupId);
            if (grouprole != null)
            {
                if (grouprole.GroupRole.GroupRoleName.Equals("Admin"))
                {

                    if (group != null)
                    {
                        if (group.IsDelete == true)
                        {
                            throw new ErrorException(StatusCodeEnum.GR08_Group_Is_Not_Exist);
                        }
                        var newgroup = new Domain.CommandModels.GroupFpt
                        {
                            GroupId = request.GroupId,
                            GroupNumber = group.GroupNumber,
                            GroupName = group.GroupName,
                            GroupDescription = group.GroupDescription,
                            GroupTypeId = group.GroupTypeId,
                            CreatedById = group.CreatedById,
                            CoverImage = group.CoverImage,
                            GroupStatusId = group.GroupStatusId,
                            CreatedDate = group.CreatedDate,
                            UpdateAt = group.UpdateAt,
                            IsDelete = true
                        };
                        _context.GroupFpts.Update(newgroup);
                        result.Message = "Delete Group Success!";
                        result.IsDelete = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }else if(user != null)
            {
                if (user.RoleId == adminrole.RoleId)
                {

                    if (group != null)
                    {
                        if (group.IsDelete == true)
                        {
                            throw new ErrorException(StatusCodeEnum.GR08_Group_Is_Not_Exist);
                        }
                        var newgroup = new Domain.CommandModels.GroupFpt
                        {
                            GroupId = request.GroupId,
                            GroupNumber = group.GroupNumber,
                            GroupName = group.GroupName,
                            GroupDescription = group.GroupDescription,
                            GroupTypeId = group.GroupTypeId,
                            CreatedById = group.CreatedById,
                            CoverImage = group.CoverImage,
                            GroupStatusId = group.GroupStatusId,
                            CreatedDate = group.CreatedDate,
                            UpdateAt = group.UpdateAt,
                            IsDelete = true
                        };
                        _context.GroupFpts.Update(newgroup);
                        result.Message = "Delete Group Success!";
                        result.IsDelete = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }

            return Result<DeleteGroupCommandResult>.Success(result);
        }
    }
}
