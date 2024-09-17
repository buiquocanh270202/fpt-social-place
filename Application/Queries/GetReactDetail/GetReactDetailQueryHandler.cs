using Application.DTO.ReactDTO;
using Application.Queries.GetReactByPost;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
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

namespace Application.Queries.GetReactDetail
{
    public class GetReactDetailQueryHandler : IQueryHandler<GetReactDetailQuery, GetReactDetailQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetReactDetailQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetReactDetailQueryResult>> Handle(GetReactDetailQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new GetReactDetailQueryResult();
            int pageSize = 10;
            switch (request.PostType)
            {
                case "UserPost":
                    var listUserReact = await (from react in _context.ReactPosts
                                               join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                               from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                               where react.UserPostId == request.PostId && react.ReactType.ReactTypeName == request.ReactName
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

                    result.ListUserReact = listUserReact;

                    break;

                case "UserPhotoPost":
                    var listUserPhotoReact = await (from react in _context.ReactPhotoPosts
                                                    join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                    from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                    where (react.UserPostPhotoId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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

                case "UserVideoPost":
                    var listUserVideoReact = await (from react in _context.ReactVideoPosts
                                                    join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                    from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                    where (react.UserPostVideoId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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

                case "GroupPost":
                    var listGroupReact = await (from react in _context.ReactGroupPosts
                                                join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                where (react.GroupPostId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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

                case "GroupPhotoPost":
                    var listGroupPhotoReact = await (from react in _context.ReactGroupPhotoPosts
                                                     join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                     from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                     from friend in _context.Friends.DefaultIfEmpty() // Left join to get friendship status
                                                     where (react.GroupPostPhotoId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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

                case "GroupVideoPost":
                    var listGroupVideoReact = await (from react in _context.ReactGroupVideoPosts
                                                     join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                     from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                     where (react.GroupPostVideoId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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

                case "UserSharePost":
                    var listUserShareReact = await (from react in _context.ReactSharePosts
                                                    join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                    from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                    where (react.SharePostId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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

                case "GroupSharePost":
                    var listGroupShareReact = await (from react in _context.ReactGroupSharePosts
                                                     join avata in _context.AvataPhotos on react.UserId equals avata.UserId into avataGroup
                                                     from avata in avataGroup.Where(x => x.IsUsed == true).DefaultIfEmpty() // Left join
                                                     where (react.GroupSharePostId == request.PostId && react.ReactType.ReactTypeName == request.ReactName)
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
            return Result<GetReactDetailQueryResult>.Success(result);
        }
    }
}