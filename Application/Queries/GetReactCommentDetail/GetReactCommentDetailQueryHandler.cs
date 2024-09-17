using Application.DTO.ReactDTO;
using Application.Queries.GetReactDetail;
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

namespace Application.Queries.GetReactCommentDetail
{
    public class GetReactCommentDetailQueryHandler : IQueryHandler<GetReactCommentDetailQuery, GetReactCommentDetailQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetReactCommentDetailQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetReactCommentDetailQueryResult>> Handle(GetReactCommentDetailQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new GetReactCommentDetailQueryResult();
            int pageSize = 10;
            switch (request.CommentType)
            {
                case "CommentUserPost":
                    var listUserReact = await (from react in _context.ReactComments
                                               join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                               from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                               where (react.CommentId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                               where react.CommentId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName
                                               orderby react.CreatedDate descending
                                               select new ReactDetailDTO
                                               {
                                                   ReactTypeId = react.ReactTypeId,
                                                   ReactName = react.ReactType.ReactTypeName,
                                                   UserId = react.UserId,
                                                   UserName = react.User.FirstName + " " + react.User.LastName,
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
                                                .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                .ToListAsync(cancellationToken);

                    result.ListUserReact = listUserReact;
                    break;

                case "CommentUserPhotoPost":
                    var listUserPhotoReact = await (from react in _context.ReactPhotoPostComments
                                                    join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                    from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                    where (react.CommentPhotoPostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                    orderby react.CreatedDate descending
                                                    select new ReactDetailDTO
                                                    {
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
                                                    .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                    .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                    .ToListAsync(cancellationToken);

                    result.ListUserReact = listUserPhotoReact;
                    break;

                case "CommentUserVideoPost":
                    var listUserVideoReact = await (from react in _context.ReactVideoPostComments
                                                    join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                    from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                    where (react.CommentVideoPostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                    orderby react.CreatedDate descending
                                                    select new ReactDetailDTO
                                                    {
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
                                                    .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                    .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                    .ToListAsync(cancellationToken);

                    result.ListUserReact = listUserVideoReact;
                    break;

                case "CommentGroupPost":
                    var listGroupReact = await (from react in _context.ReactGroupCommentPosts
                                                join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                from friend in _context.Friends.DefaultIfEmpty() // Left join to get friendship status
                                                where (react.CommentGroupPostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                orderby react.CreatedDate descending
                                                select new ReactDetailDTO
                                                {
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
                                                .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                .ToListAsync(cancellationToken);
                    result.ListUserReact = listGroupReact;
                    break;

                case "CommentGroupPhotoPost":
                    var listGroupPhotoReact = await (from react in _context.ReactGroupPhotoPostComments
                                                     join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                     from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                     where (react.CommentPhotoGroupPostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                     orderby react.CreatedDate descending
                                                     select new ReactDetailDTO
                                                     {
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
                                                     .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                    .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                     .ToListAsync(cancellationToken);
                    result.ListUserReact = listGroupPhotoReact;
                    break;

                case "CommentGroupVideoPost":
                    var listGroupVideoReact = await (from react in _context.ReactGroupVideoPostComments
                                                     join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                     from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                     where (react.CommentGroupVideoPostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                     orderby react.CreatedDate descending
                                                     select new ReactDetailDTO
                                                     {
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
                                                    .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                    .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                    .ToListAsync(cancellationToken);
                    result.ListUserReact = listGroupVideoReact;
                    break;

                case "CommentUserSharePost":
                    var listUserShareReact = await (from react in _context.ReactSharePostComments
                                                    join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                    from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                    where (react.CommentSharePostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                    orderby react.CreateDate descending
                                                    select new ReactDetailDTO
                                                    {
                                                        ReactTypeId = react.ReactTypeId,
                                                        ReactName = react.ReactType.ReactTypeName,
                                                        UserId = react.UserId,
                                                        UserName = react.User.FirstName + react.User.LastName,
                                                        CreatedDate = react.CreateDate,
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
                                                    .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                    .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                    .ToListAsync(cancellationToken);
                    result.ListUserReact = listUserShareReact;
                    break;

                case "CommentGroupSharePost":
                    var listGroupShareReact = await (from react in _context.ReactGroupSharePostComments
                                                     join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                     from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                     where (react.CommentGroupSharePostId == request.CommentId && react.ReactType.ReactTypeName == request.ReactName)
                                                     orderby react.CreateDate descending
                                                     select new ReactDetailDTO
                                                     {
                                                         ReactTypeId = react.ReactTypeId,
                                                         ReactName = react.ReactType.ReactTypeName,
                                                         UserId = react.UserId,
                                                         UserName = react.User.FirstName + react.User.LastName,
                                                         CreatedDate = react.CreateDate,
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
                                                    .Skip((request.PageNumber - 1) * pageSize) // Bỏ qua các mục trước trang hiện tại
                                                    .Take(pageSize) // Lấy số mục cho trang hiện tại
                                                    .ToListAsync(cancellationToken);
                    result.ListUserReact = listGroupShareReact;
                    break;

                case null:
                    break;
            }
            return Result<GetReactCommentDetailQueryResult>.Success(result);
        }
    }
}