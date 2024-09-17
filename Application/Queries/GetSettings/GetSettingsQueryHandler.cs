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

namespace Application.Queries.GetSettings
{
    public class GetSettingsQueryHandler : IQueryHandler<GetSettingsQuery, List<GetSettingsQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetSettingsQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetSettingsQueryResult>>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var settings = await _context.Settings.ToListAsync();
            List<GetSettingsQueryResult> result = new List<GetSettingsQueryResult>();
            if (settings != null)
            {
                foreach (var set in settings)
                {
                    var mapsettings = _mapper.Map<GetSettingsQueryResult>(set);
                    result.Add(mapsettings);
                }
            }

            return Result<List<GetSettingsQueryResult>>.Success(result);
        }
    }
}
