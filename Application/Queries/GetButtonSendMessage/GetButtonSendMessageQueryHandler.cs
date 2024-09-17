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

namespace Application.Queries.GetButtonSendMessage
{
    public class GetButtonSendMessageQueryHandler : IQueryHandler<GetButtonSendMessageQuery, GetButtonSendMessageQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetButtonSendMessageQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetButtonSendMessageQueryResult>> Handle(GetButtonSendMessageQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.ViewOtherId);

            if(user == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }
            var result = new GetButtonSendMessageQueryResult();

            var viewUserSetting = await _context.UserSettings.Include(x=>x.Setting)
                .Where(x => x.UserId == request.ViewOtherId).ToListAsync();
            var userSetting = viewUserSetting.FirstOrDefault(x => x.Setting.SettingName.Equals("Profile Status"));
            var statusPublic = await _context.UserStatuses.FirstOrDefaultAsync(x=>x.StatusName.Equals("Public"));
            var statusPrivate = await _context.UserStatuses.FirstOrDefaultAsync(x => x.StatusName.Equals("Private"));

            if(viewUserSetting?.Count > 0)
            {
                if(userSetting.UserStatusId == statusPublic.UserStatusId)
                {
                    result.IsHide = false;
                }
                else if(userSetting.UserStatusId == statusPrivate.UserStatusId)
                {
                    result.IsHide = true;
                }
            }

            return Result<GetButtonSendMessageQueryResult>.Success(result);
        }
    }
}
