using Application.DTO.GetUserProfileDTO;
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

namespace Application.Queries.GetWebAffilication
{
    public class GetWebAffilicationHandler : IQueryHandler<GetWebAffilicationQuery, List<GetWebAffilicationResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetWebAffilicationHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetWebAffilicationResult>>> Handle(GetWebAffilicationQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            if(request.UserId == null)
            {
                var listallweb = await _context.WebAffiliations.Include(x=>x.User).ToListAsync();
                List<GetWebAffilicationResult> results = new List<GetWebAffilicationResult>();
                if(listallweb != null)
                {
                    foreach (var web in listallweb)
                    {
                        var mapweb = _mapper.Map<GetWebAffilicationResult>(web);
                        results.Add(mapweb);
                    }
                }

                return Result<List<GetWebAffilicationResult>>.Success(results);
            }
            else
            {
                var userweb = await _context.WebAffiliations.Include(x => x.User).Where(x => x.UserId == request.UserId).ToListAsync();
                if(userweb.Count == 0)
                {
                    throw new ErrorException(StatusCodeEnum.U05_Not_Has_WebAffilication);
                }
                List<GetWebAffilicationResult> results = new List<GetWebAffilicationResult>();
                foreach (var web in userweb)
                {
                    var mapweb = _mapper.Map<GetWebAffilicationResult>(web);
                    results.Add(mapweb);
                }
                return Result<List<GetWebAffilicationResult>>.Success(results);
            }
        }
    }
}
