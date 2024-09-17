using Application.DTO.CreateUserDTO;
using Application.DTO.GetUserProfileDTO;
using Application.DTO.NotificationDTO;
using Application.Queries.GetGender;
using Application.Queries.GetUserPost;
using Application.Queries.GetUserProfile;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserNotificationsList
{
    public class GetUserNotificationsListQueryHandler : IQueryHandler<GetUserNotificationsListQuery, List<GetUserNotificationsListQueryResult>>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;
        private readonly SplitString _splitString;

        public GetUserNotificationsListQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _splitString = new SplitString();
        }

        public async Task<Result<List<GetUserNotificationsListQueryResult>>> Handle(GetUserNotificationsListQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var notifications = (from n in _context.Notifications
                                 join s in _context.UserProfiles on n.SenderId equals s.UserId
                                 where n.UserId == request.UserId && n.UserId != n.SenderId
                                 orderby n.CreatedAt descending
                                 select new
                                 {
                                     n.NotificationId,
                                     n.SenderId,
                                     SenderName = s.FirstName + " " + s.LastName,
                                     n.NotiMessage,
                                     n.IsRead,
                                     n.NotifiUrl,
                                     n.CreatedAt,
                                     SenderAvatar = (from a in _context.AvataPhotos
                                                     where a.UserId == n.SenderId && a.IsUsed
                                                     orderby a.CreatedAt descending
                                                     select a.AvataPhotosUrl).FirstOrDefault()
                                 })
                     .Skip((request.Page - 1) * request.PageSize) // Bỏ qua số phần tử của trang trước đó
                     .Take(request.PageSize) // Lấy số phần tử trong trang hiện tại
                     .AsEnumerable() // Convert to in-memory collection to allow for nullable string property assignment
                     .Select(n => new NotificationOutDTO
                     {
                         NotificationId = n.NotificationId,
                         SenderId = n.SenderId.ToString(),
                         SenderName = n.SenderName,
                         SenderAvatar = n.SenderAvatar ?? string.Empty,
                         Message = _splitString.SplitStringForNotify(n.NotiMessage).Last(),
                         Url = n.NotifiUrl,
                         IsRead = (bool)n.IsRead,
                         CreatedAt = (DateTime)n.CreatedAt,
                     }).ToList();

            var result = _mapper.Map<List<GetUserNotificationsListQueryResult>>(notifications);
            return Result<List<GetUserNotificationsListQueryResult>>.Success(result);
        }
    }
}
