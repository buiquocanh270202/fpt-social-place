using Application.DTO.ReactDTO;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
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

namespace Application.Queries.GetReactByCommentVideoId
{
    public class GetReactByCommentVideoIdQueryHandler : IQueryHandler<GetReactByCommentVideoIdQuery, GetReactByCommentVideoIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public GetReactByCommentVideoIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<GetReactByCommentVideoIdQueryResult>> Handle(GetReactByCommentVideoIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            bool isReact = false;
            // 1. Fetch Reactions and Include Related Data
            var listUserReact = await (from reactComment in _context.ReactVideoPostComments
                                       join avata in _context.AvataPhotos on reactComment.UserId equals avata.UserId into avataGroup
                                       from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty()
                                       where reactComment.CommentVideoPostId == request.CommentVideoPostId
                                       group new { reactComment, avata } by reactComment.ReactVideoPostCommentId into g
                                       select new ReactCommentVideoDTO
                                       {
                                           ReactVideoPostCommentId = g.Key,
                                           UserPostVideoId = g.First().reactComment.UserPostVideoId,
                                           UserId = g.First().reactComment.UserId,
                                           UserName = g.First().reactComment.User.FirstName + g.First().reactComment.User.LastName,
                                           ReactTypeId = g.First().reactComment.ReactTypeId,
                                           ReactTypeName = g.First().reactComment.ReactType.ReactTypeName,
                                           CommentVideoPostId = g.First().reactComment.CommentVideoPostId,
                                           CreatedDate = g.First().reactComment.CreatedDate,
                                           AvataUrl = g.First().avata != null ? g.First().avata.AvataPhotosUrl : null,
                                           Status = _context.Friends.Where(x => (x.UserId == g.First().reactComment.UserId && x.FriendId == request.UserId) ||
                                                                                        (x.UserId == request.UserId && x.FriendId == g.First().reactComment.UserId))
                                                                            .Select(y => y.Confirm)
                                                                            .FirstOrDefault() != null
                                                                            ? (_context.Friends.Any(x => (x.UserId == g.First().reactComment.UserId && x.FriendId == request.UserId) ||
                                                                                                            (x.UserId == request.UserId && x.FriendId == g.First().reactComment.UserId))
                                                                                ? (_context.Friends.FirstOrDefault(x => (x.UserId == g.First().reactComment.UserId && x.FriendId == request.UserId) ||
                                                                                                                        (x.UserId == request.UserId && x.FriendId == g.First().reactComment.UserId))
                                                                                    .Confirm ? "Friend" : "Pending")
                                                                                : "NotFriend")
                                                                            : "NotFriend"
                                       })
                                       .Skip((request.PageNumber - 1) * 10) // Bỏ qua các mục trước trang hiện tại
                                       .Take(10) // Lấy số mục cho trang hiện tại
                                       .ToListAsync(cancellationToken);

            var listReact = await (from reactType in _context.ReactTypes // Start from ReactTypes
                                   join reactComment in _context.ReactVideoPostComments
                                       on reactType.ReactTypeId equals reactComment.ReactTypeId into reactGroup
                                   from reactComment in reactGroup.DefaultIfEmpty() // Handle the case where there are no matches
                                   where reactComment == null || reactComment.CommentVideoPostId == request.CommentVideoPostId // Filter by CommentVideoPostId
                                   group reactComment by new { reactType.ReactTypeId, reactType.ReactTypeName } into g // Group by ID and Name
                                   select new ReactTypeCountDTO
                                   {
                                       ReactTypeId = g.Key.ReactTypeId,
                                       ReactTypeName = g.Key.ReactTypeName,
                                       NumberReact = g.Count(r => r != null) // Count only non-null reacts
                                   })
                                   .OrderByDescending(dto => dto.NumberReact) // Sort by NumberReact
                                   .ToListAsync(cancellationToken);


            var checkReact = await (_context.ReactVideoPostComments.Where(x => x.UserId == request.UserId && x.CommentVideoPostId == request.CommentVideoPostId)).ToListAsync(cancellationToken);
            if (checkReact.Count() != 0)
            {
                isReact = true;
            }

            // 2. Calculate Sum of Reactions
            var sumOfReacts = listUserReact.Count;

            // 3. Create Result
            var result = new GetReactByCommentVideoIdQueryResult
            {
                SumOfReact = sumOfReacts,
                ListCommentReact = listUserReact,
                ListReact = listReact,
                IsReact = isReact
            };

            return Result<GetReactByCommentVideoIdQueryResult>.Success(result);
        }

    }
}
