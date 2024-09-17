using Application.Commands.UpdateUserPostCommand;
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
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.UpdateGroupSharePost
{
    public class UpdateGroupSharePostCommandHandler : ICommandHandler<UpdateGroupSharePostCommand, UpdateGroupSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateGroupSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }
        public async Task<Result<UpdateGroupSharePostCommandResult>> Handle(UpdateGroupSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var groupSharePostCheck = await _queryContext.GroupSharePosts.FindAsync(request.GroupSharePostId);
            if (groupSharePostCheck == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            var userId = await _queryContext.GroupSharePosts
                                      .Where(a => a.GroupSharePostId == request.GroupSharePostId)
                                      .Select(a => a.UserId)
                                      .FirstOrDefaultAsync();

            if (request.UserId != userId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var groupSharePost = _queryContext.GroupSharePosts.Where(x => x.GroupSharePostId == request.GroupSharePostId).FirstOrDefault();
            if (groupSharePost != null) 
            {
                groupSharePost.Content = request.Content;
                groupSharePost.IsBanned = false;
            }
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(request.Content);
            if (haveBadWord.Any())
            {
                groupSharePost.IsBanned = true;
                groupSharePost.Content = _checkContent.MarkBannedWordsInContent(request.Content, haveBadWord);
            }

            Domain.CommandModels.GroupSharePost gsp = new Domain.CommandModels.GroupSharePost
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
                SharedToUserId = groupSharePost.SharedToUserId,
                CreateDate = groupSharePost.CreateDate,
                IsHide = groupSharePost.IsHide,
                GroupStatusId = groupSharePost.GroupStatusId,
                UpdateDate = DateTime.Now,
                IsBanned = groupSharePost.IsBanned,
                GroupId = groupSharePost.GroupId,
                UserSharedId = groupSharePost.UserSharedId,
                IsPending = groupSharePost.IsPending,
            };
            _context.GroupSharePosts.Update(gsp);
            _context.SaveChanges();

            var result = _mapper.Map<UpdateGroupSharePostCommandResult>(gsp);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateGroupSharePostCommandResult>.Success(result);
            }

        }
    }
}

