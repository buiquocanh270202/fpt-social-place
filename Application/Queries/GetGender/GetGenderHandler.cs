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

namespace Application.Queries.GetGender
{
    public class GetGenderHandler : IQueryHandler<GetGenderQuery, List<GetGenderReuslt>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetGenderHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetGenderReuslt>>> Handle(GetGenderQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var gender = await _context.Genders.ToListAsync();
            List<GetGenderReuslt> result = new List<GetGenderReuslt>();
            if(gender != null)
            {
                foreach (var gen in gender)
                {
                    var mapgender = _mapper.Map<GetGenderReuslt>(gen);
                    result.Add(mapgender);
                }
            }
           
            return Result<List<GetGenderReuslt>>.Success(result);
        }
    }
}
