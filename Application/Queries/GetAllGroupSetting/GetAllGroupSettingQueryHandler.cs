using Application.Queries.GetGender;
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

namespace Application.Queries.GetAllGroupSetting
{
    public class GetAllGroupSettingQueryHandler : IQueryHandler<GetAllGroupSettingQuery, List<GetAllGroupSettingQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllGroupSettingQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetAllGroupSettingQueryResult>>> Handle(GetAllGroupSettingQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var setting = await _context.GroupSettings.ToListAsync();
            var result = new List<GetAllGroupSettingQueryResult>();
            if (setting != null)
            {
                foreach (var set in setting)
                {
                    var mapsetting = _mapper.Map<GetAllGroupSettingQueryResult>(set);
                    result.Add(mapsetting);
                }
            }

            return Result<List<GetAllGroupSettingQueryResult>>.Success(result);
        }
    }
}
