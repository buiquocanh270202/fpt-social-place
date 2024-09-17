using Application.DTO.ReactDTO;
using Application.Queries.GetPost;
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

namespace Application.Queries.GetReactByPhotoPost
{
    public class GetReactByPhotoPostQueryHandler : IQueryHandler<GetReactByPhotoPostQuery, GetReactByPhotoPostQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetReactByPhotoPostQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetReactByPhotoPostQueryResult>> Handle(GetReactByPhotoPostQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            bool isReact = false;
            // 1. Fetch Reactions and Include Related Data
            var listUserReact = await (from react in _context.ReactPhotoPosts
                                        join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                        from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                        where react.UserPostPhotoId == request.UserPostPhotoId
                                        orderby react.CreatedDate descending
                                        select new ReactPhotoPostDTO
                                        {
                                            ReactPhotoPostId = react.ReactPhotoPostId,
                                            UserPostPhotoId = react.UserPostPhotoId,
                                            ReactTypeId = react.ReactTypeId,
                                            ReactName = react.ReactType.ReactTypeName,
                                            UserId = react.UserId,
                                            UserName = react.User.FirstName + react.User.LastName,
                                            CreatedDate = react.CreatedDate,
                                            AvataUrl = avata != null ? avata.AvataPhotosUrl : null,
                                            Status = _context.Friends.Where(x => (x.UserId == react.UserId && x.FriendId == request.UserId) ||
                                                                                        (x.UserId == request.UserId && x.FriendId == react.UserId))
                                                                            .Select(y => y.Confirm)
                                                                            .FirstOrDefault() != null
                                                                            ? (_context.Friends.Any(x => (x.UserId == react.UserId && x.FriendId == request.UserId) ||
                                                                                                            (x.UserId == request.UserId && x.FriendId == react.UserId))
                                                                                ? (_context.Friends.FirstOrDefault(x => (x.UserId == react.UserId && x.FriendId == request.UserId) ||
                                                                                                                        (x.UserId == request.UserId && x.FriendId == react.UserId))
                                                                                    .Confirm ? "Friend" : "Pending")
                                                                                : "NotFriend")
                                                                            : "NotFriend"
                                        }
                                        )
                                        .Skip((request.PageNumber - 1) * 10) // Bỏ qua các mục trước trang hiện tại
                                        .Take(10) // Lấy số mục cho trang hiện tại
                                        .ToListAsync(cancellationToken);

            var listReact = await (from reactType in _context.ReactTypes // Start from ReactTypes
                                   join react in _context.ReactPhotoPosts.Where(r => r.UserPostPhotoId == request.UserPostPhotoId)
                                       on reactType.ReactTypeId equals react.ReactTypeId into reactGroup
                                   from react in reactGroup.DefaultIfEmpty() // Handle the case where there are no matches
                                   group react by new { reactType.ReactTypeId, reactType.ReactTypeName } into g
                                   select new ReactTypeCountDTO
                                   {
                                       ReactTypeId = g.Key.ReactTypeId,
                                       ReactTypeName = g.Key.ReactTypeName,
                                       NumberReact = g.Count(r => r != null) // Count only non-null reacts
                                   })
                                  .OrderByDescending(dto => dto.NumberReact) // Sort by NumberReact
                                  .ToListAsync(cancellationToken);

            var checkReact = await (_context.ReactPhotoPosts.Where(x => x.UserId == request.UserId && x.UserPostPhotoId == request.UserPostPhotoId)).ToListAsync(cancellationToken);
            if (checkReact.Count() != 0)
            {
                isReact = true;
            }

            // 2. Calculate Sum of Reactions
            var sumOfReacts = listUserReact.Count;

            // 3. Create Result
            var result = new GetReactByPhotoPostQueryResult
            {
                SumOfReact = sumOfReacts,
                ListUserReact = listUserReact,
                IsReact = isReact,
                ListReact = listReact
            };

            return Result<GetReactByPhotoPostQueryResult>.Success(result);
        }

    }
}
