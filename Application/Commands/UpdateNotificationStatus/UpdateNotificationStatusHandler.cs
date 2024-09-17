using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.UpdateNotificationStatus
{
    public class UpdateNotificationStatusHandler : ICommandHandler<UpdateNotificationStatusCommand, UpdateNotificationStatusResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public UpdateNotificationStatusHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<UpdateNotificationStatusResult>> Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var notify = await _querycontext.Notifications.FirstOrDefaultAsync(x => x.NotificationId == request.NotificationId);

            if (notify == null)
            {
                throw new ErrorException(StatusCodeEnum.Error);
            }

            var updateNotify = new Domain.CommandModels.Notification
            {
                UserId = notify.UserId,
                SenderId = notify.SenderId,
                NotificationTypeId = notify.NotificationTypeId,
                UserStatusId = notify.UserStatusId,
                NotificationId = request.NotificationId,
                NotiMessage = notify.NotiMessage,
                CreatedAt = notify.CreatedAt,
                NotifiUrl = notify.NotifiUrl,
                IsRead = true,
            };
            _context.Notifications.Update(updateNotify);
            await _context.SaveChangesAsync();

            var result = new UpdateNotificationStatusResult
            {
                UserId = notify.UserId,
                SenderId = notify.SenderId,
                NotificationTypeId = notify.NotificationTypeId,
                UserStatusId = notify.UserStatusId,
                NotificationId = request.NotificationId,
                NotiMessage = notify.NotiMessage,
                CreatedAt = notify.CreatedAt,
                NotifiUrl = notify.NotifiUrl,
                IsRead = true,
            };

            return Result<UpdateNotificationStatusResult>.Success(result);
        }

    }
}
