using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.FriendStatusCommand
{
    public class FriendStatusCommandHandler : ICommandHandler<FriendStatusCommand, FriendStatusCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public FriendStatusCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<FriendStatusCommandResult>> Handle(FriendStatusCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if((request.Confirm == true && request.Cancle == true && request.Reject ==true)
                || (request.Confirm == true && request.Cancle == true) 
                || (request.Cancle == true && request.Reject == true)
                || (request.Confirm == true && request.Reject == true)
                ||(request.Confirm ==  false && request.Cancle == false && request.Reject == false))
            {
                throw new ErrorException(StatusCodeEnum.Error);
            }

            var friend = _querycontext.Friends.FirstOrDefault(x => (x.UserId == request.UserId && x.FriendId == request.FriendId)
                                                   || (x.FriendId == request.UserId && x.UserId == request.FriendId));
            if (friend == null)
            {
                throw new ErrorException(StatusCodeEnum.FR05_Not_Friend);
            }
            var result = new FriendStatusCommandResult();
            if(friend.Confirm == false)
            {
                if (request.Confirm == true)
                {
                    if (friend.UserId == request.FriendId)
                    {
                        var newfriend = new Domain.CommandModels.Friend
                        {
                            UserId = friend.UserId,
                            FriendId = friend.FriendId,
                            Confirm = true,
                            ReactCount = 0,
                            LastInteractionDate = DateTime.Now
                        };
                        _context.Friends.Update(newfriend);
                        result.UserId = newfriend.UserId;
                        result.FriendId = newfriend.FriendId;
                        result.Confirm = newfriend.Confirm;
                        result.Status = StatusCodeEnum.FR02_Accept_Friend.ToString().Split("_")[0];
                        result.Message = StatusCodeEnum.FR02_Accept_Friend.GetDescription();
                    }
                    else
                    {
                        throw new ErrorException(StatusCodeEnum.FR06_Can_Not_Friend);
                    }

                }
                else if (request.Cancle == true)
                {
                    if (friend.UserId == request.UserId)
                    {
                        var canclefriend = new Domain.CommandModels.Friend
                        {
                            UserId = friend.UserId,
                            FriendId = friend.FriendId,
                            Confirm = friend.Confirm,
                            ReactCount= friend.ReactCount,
                            LastInteractionDate = friend.LastInteractionDate
                        };
                        _context.Friends.Remove(canclefriend);
                        result.UserId = canclefriend.UserId;
                        result.FriendId = canclefriend.FriendId;
                        result.Confirm = canclefriend.Confirm;
                        result.Status = StatusCodeEnum.FR03_Cancle_Friend.ToString().Split("_")[0];
                        result.Message = StatusCodeEnum.FR03_Cancle_Friend.GetDescription();
                    }
                    else
                    {
                        throw new ErrorException(StatusCodeEnum.FR07_Can_Not_Cancel_Friend);
                    }

                }
                else if (request.Reject == true)
                {
                    if (friend.UserId == request.FriendId)
                    {
                        var rejectfriend = new Domain.CommandModels.Friend
                        {
                            UserId = friend.UserId,
                            FriendId = friend.FriendId,
                            Confirm = friend.Confirm,
                            ReactCount = friend.ReactCount,
                            LastInteractionDate = friend.LastInteractionDate
                        };
                        _context.Friends.Remove(rejectfriend);
                        result.UserId = rejectfriend.UserId;
                        result.FriendId = rejectfriend.FriendId;
                        result.Confirm = rejectfriend.Confirm;
                        result.Status = StatusCodeEnum.FR04_Reject_Friend.ToString().Split("_")[0];
                        result.Message = StatusCodeEnum.FR04_Reject_Friend.GetDescription();
                    }
                    else
                    {
                        throw new ErrorException(StatusCodeEnum.FR08_Can_Not_Reject_Friend);
                    }

                }
            }
            else
            {
                if (request.Reject == true)
                {
                    var rejectfriend = new Domain.CommandModels.Friend
                    {
                        UserId = friend.UserId,
                        FriendId = friend.FriendId,
                        Confirm = friend.Confirm,
                        ReactCount = friend.ReactCount,
                        LastInteractionDate = friend.LastInteractionDate
                    };
                    _context.Friends.Remove(rejectfriend);
                    result.UserId = rejectfriend.UserId;
                    result.FriendId = rejectfriend.FriendId;
                    result.Confirm = rejectfriend.Confirm;
                    result.Status = StatusCodeEnum.FR04_Reject_Friend.ToString().Split("_")[0];
                    result.Message = StatusCodeEnum.FR04_Reject_Friend.GetDescription();
                }
            }
            await _context.SaveChangesAsync();
            return Result<FriendStatusCommandResult>.Success(result);
        }
    }
}
