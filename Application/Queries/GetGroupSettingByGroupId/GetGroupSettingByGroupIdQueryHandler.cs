using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
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

namespace Application.Queries.GetGroupSettingByGroupId
{
    public class GetGroupSettingByGroupIdQueryHandler : IQueryHandler<GetGroupSettingByGroupIdQuery, List<GetGroupSettingByGroupIdQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGroupSettingByGroupIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetGroupSettingByGroupIdQueryResult>>> Handle(GetGroupSettingByGroupIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new List<GetGroupSettingByGroupIdQueryResult>();
            var groupsetting = await _context.GroupSettingUses.Include(x => x.GroupSetting).Include(x=>x.GroupStatus)
                .Where(x=>x.GroupId == request.GroupId).ToListAsync();
            var grouprole = await _context.GroupMembers.Include(x => x.GroupRole).FirstOrDefaultAsync(x => x.UserId == request.UserId
                                                                                                && x.GroupId == request.GroupId);

            if (grouprole.GroupRole.GroupRoleName.Equals("Admin"))
            {
                foreach (var item in groupsetting)
                {
                    var newsetting = new GetGroupSettingByGroupIdQueryResult
                    {
                        GroupId = request.GroupId,
                        GroupSettingId = item.GroupSettingId,
                        GroupSettingName = item.GroupSetting.GroupSettingName,
                        GroupStatusName = item.GroupSetting.GroupSettingName,
                        GroupStatusId = item.GroupStatusId
                    };
                    result.Add(newsetting);
                }
            }
            else
            {
                throw new ErrorException(StatusCodeEnum.GR11_Not_Permission);
            }

            return Result<List<GetGroupSettingByGroupIdQueryResult>>.Success(result);
        }
    }
}
