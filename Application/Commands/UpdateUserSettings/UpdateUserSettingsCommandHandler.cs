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

namespace Application.Commands.UpdateUserSettings
{
    public class UpdateUserSettingsCommandHandler : ICommandHandler<UpdateUserSettingsCommand, UpdateUserSettingsCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public UpdateUserSettingsCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<UpdateUserSettingsCommandResult>> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new UpdateUserSettingsCommandResult();
            if (request.UserSettings != null)
            {
                foreach (var set in request.UserSettings)
                {
                    var usersetting = await _querycontext.UserSettings.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.SettingId == set.SettingId);
                    var updatesetting = new Domain.CommandModels.UserSetting
                    {
                        UserSettingId = usersetting.UserSettingId,
                        SettingId = set.SettingId,
                        UserId = (Guid)request.UserId,
                        UserStatusId = set.UserStatusId
                    };
                    _context.UserSettings.Update(updatesetting);
                }
                    await _context.SaveChangesAsync();
                result.UserId = request.UserId;
                result.UserSettings  = request.UserSettings;
                
            }
            return Result<UpdateUserSettingsCommandResult>.Success(result);
        }
    }
}
