using Application.DTO.UpdateSettingDTO;
using Application.Queries.GetUserStatus;
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

namespace Application.Queries.GetUserSettings
{
    public class GetUserSettingsQueryHandler : IQueryHandler<GetUserSettingsQuery, GetUserSettingsQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserSettingsQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetUserSettingsQueryResult>> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var setting = await _context.UserSettings.Where(x=>x.UserId == request.UserId).ToListAsync();
            var result = new GetUserSettingsQueryResult();
            result.UserId = (Guid)request.UserId;
            foreach(var item in setting)
            {
                var map = _mapper.Map<UserSettingDTO>(item);
                result.Usersettings.Add(map);
            }
            
            return Result<GetUserSettingsQueryResult>.Success(result);
        }
    }
}
