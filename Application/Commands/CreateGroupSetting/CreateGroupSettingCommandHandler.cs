using Application.Commands.CreateRole;
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

namespace Application.Commands.CreateGroupSetting
{
    public class CreateGroupSettingCommandHandler : ICommandHandler<CreateGroupSettingCommand,CreateGroupSettingCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public CreateGroupSettingCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<CreateGroupSettingCommandResult>> Handle(CreateGroupSettingCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var setting = await _querycontext.GroupSettings.FirstOrDefaultAsync(x => x.GroupSettingName.Equals(request.GroupSettingName));
            if (setting != null)
            {
                throw new ErrorException(StatusCodeEnum.GR03_Group_Setting_Existed);
            }

            var newsetting = new Domain.CommandModels.GroupSetting
            {
                GroupSettingId = _helper.GenerateNewGuid(),
                GroupSettingName = request.GroupSettingName,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            await _context.GroupSettings.AddAsync(newsetting);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateGroupSettingCommandResult>(newsetting);
            return Result<CreateGroupSettingCommandResult>.Success(result);
        }
    }
}
