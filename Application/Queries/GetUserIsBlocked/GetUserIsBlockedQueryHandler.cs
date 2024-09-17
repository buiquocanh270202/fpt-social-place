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

namespace Application.Queries.GetUserIsBlocked
{
    public class GetUserIsBlockedQueryHandler : IQueryHandler<GetUserIsBlockedQuery, List<GetUserIsBlockedQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserIsBlockedQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<GetUserIsBlockedQueryResult>>> Handle(GetUserIsBlockedQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new List<GetUserIsBlockedQueryResult>();

            var listBlocked = await _context.BlockUsers.Include(x=>x.UserIsBlocked).ThenInclude(x=>x.AvataPhotos)
                .Where(x => x.UserId == request.UserId).ToListAsync();
            foreach (var item in listBlocked)
            {
                var user = new GetUserIsBlockedQueryResult
                {
                    UserBlockedId = item.UserIsBlockedId,
                    Avata = item.UserIsBlocked.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl,
                    FullName = item.UserIsBlocked.FirstName + " "+ item.UserIsBlocked.LastName 
                };
                result.Add(user);
            }

            return Result<List<GetUserIsBlockedQueryResult>>.Success(result);
        }
    }
}
