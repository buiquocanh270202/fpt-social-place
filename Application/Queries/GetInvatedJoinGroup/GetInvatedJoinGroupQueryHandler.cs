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

namespace Application.Queries.GetInvatedJoinGroup
{
    public class GetInvatedJoinGroupQueryHandler : IQueryHandler<GetInvatedJoinGroupQuery, List<GetInvatedJoinGroupQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetInvatedJoinGroupQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<List<GetInvatedJoinGroupQueryResult>>> Handle(GetInvatedJoinGroupQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new List<GetInvatedJoinGroupQueryResult>();
            var invated = await _context.GroupMembers.Include(x=>x.Group).Include(x=>x.InvatedByNavigation)
                                .ThenInclude(x=>x.AvataPhotos).Where(x => x.UserId == request.UserId &&x.InvatedBy != null && x.IsInvated == false).ToListAsync();
            if(invated != null)
            {
                foreach (var item in invated)
                {
                    var group = new GetInvatedJoinGroupQueryResult {
                        GroupId = item.GroupId,
                        GroupName = item.Group.GroupName,
                        CoverImage = item.Group.CoverImage,
                        InvatedBy = (Guid)item.InvatedBy,
                        InvatedByName = item.InvatedByNavigation.FirstName + " " + item.InvatedByNavigation.LastName,
                        InvatedByAvata = item.InvatedByNavigation.AvataPhotos.FirstOrDefault(x => x.IsUsed == true)?.AvataPhotosUrl
                    };
                    result.Add(group);
                }
            }
            return Result<List<GetInvatedJoinGroupQueryResult>>.Success(result);
        }
    }
}
