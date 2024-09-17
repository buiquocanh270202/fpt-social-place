using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AprroveGroupPost
{
    public class ApproveGroupPostCommandHandler : ICommandHandler<ApproveGroupPostCommand, ApproveGroupPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public ApproveGroupPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<ApproveGroupPostCommandResult>> Handle(ApproveGroupPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var checkRole = await (from gm in _querycontext.GroupMembers
                             join gr in _querycontext.GroupRoles on gm.GroupRoleId equals gr.GroupRoleId
                             where gm.UserId == request.UserId
                             select gr.GroupRoleName).FirstOrDefaultAsync();
            var result = new ApproveGroupPostCommandResult();

            if (checkRole.ToString() == "Admin" || checkRole.ToString() == "Moderator")
            {
                var groupPost = await _querycontext.GroupPosts.Where(x => x.GroupPostId == request.GroupPostId).FirstOrDefaultAsync();
                var groupSharePost = await _querycontext.GroupSharePosts.Where(x => x.GroupSharePostId == request.GroupSharePostId).FirstOrDefaultAsync();

                Domain.CommandModels.GroupPost gp = new Domain.CommandModels.GroupPost();
                Domain.CommandModels.GroupSharePost gsp = new Domain.CommandModels.GroupSharePost();

                if (groupPost != null)
                {
                    gp = new Domain.CommandModels.GroupPost
                    {
                        GroupPostId = groupPost.GroupPostId,
                        UserId = groupPost.UserId,
                        Content = groupPost.Content,
                        GroupPostNumber = groupPost.GroupPostNumber,
                        GroupStatusId = groupPost.GroupStatusId,
                        CreatedAt = groupPost.CreatedAt,
                        IsHide = groupPost.IsHide,
                        UpdatedAt = DateTime.Now,
                        GroupPhotoId = groupPost.GroupPhotoId,
                        GroupVideoId = groupPost.GroupVideoId,
                        NumberPost = groupPost.NumberPost,
                        IsBanned = groupPost.IsBanned,
                        IsPending = groupPost.IsPending,
                        GroupId = groupPost.GroupId,
                    };
                }
                if (groupSharePost != null)
                {
                    gsp = new Domain.CommandModels.GroupSharePost
                    {
                        GroupSharePostId = groupSharePost.GroupSharePostId,
                        UserId = groupSharePost.UserId,
                        Content = groupSharePost.Content,
                        UserPostId = groupSharePost.UserPostId,
                        UserPostVideoId = groupSharePost.UserPostVideoId,
                        UserPostPhotoId = groupSharePost.UserPostPhotoId,
                        GroupPostId = groupSharePost.GroupPostId,
                        GroupPostPhotoId = groupSharePost.GroupPostPhotoId,
                        GroupPostVideoId = groupSharePost.GroupPostVideoId,
                        GroupStatusId = groupSharePost.GroupStatusId,
                        CreateDate = groupSharePost.CreateDate,
                        IsHide = groupSharePost.IsHide,
                        UpdateDate = DateTime.Now,
                        SharedToUserId = groupSharePost.SharedToUserId,
                        IsBanned = groupSharePost.IsBanned,
                        IsPending = groupSharePost.IsPending,
                        GroupId = groupSharePost.GroupId,
                        UserSharedId = groupSharePost.UserSharedId,
                    };
                }

                switch (request.Type)
                {
                    case "Approve":
                        if (!String.IsNullOrEmpty((request.GroupPostId).ToString()))
                        {
                            gp.IsPending = false;
                            _context.GroupPosts.Update(gp);
                            _context.SaveChanges();
                            result.Message = "Approve group post success";
                            result.IsApprove = true;
                        }
                        else if (!String.IsNullOrEmpty((request.GroupSharePostId).ToString()))
                        {
                            gsp.IsPending = false;
                            _context.GroupSharePosts.Update(gsp);
                            _context.SaveChanges();
                            result.Message = "Approve group share post success";
                            result.IsApprove = true;
                        }
                        break;
                    case "Decline":
                        if (!String.IsNullOrEmpty((request.GroupPostId).ToString()))
                        {
                            gp.IsHide = true;
                            _context.GroupPosts.Update(gp);
                            _context.SaveChanges();
                            result.Message = "Approve group post success";
                            result.IsApprove = true;
                        }
                        else if (!String.IsNullOrEmpty((request.GroupSharePostId).ToString()))
                        {
                            gsp.IsHide = true;
                            _context.GroupSharePosts.Update(gsp);
                            _context.SaveChanges();
                            result.Message = "Approve group share post success";
                            result.IsApprove = true;
                        }
                        break;
                }

            }
            return Result<ApproveGroupPostCommandResult>.Success(result);

        }
    }
}
