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
using System.Text;
using System.Threading.Tasks;
using static Application.Services.CheckingBadWord;

namespace Application.Commands.UpdateGroupVideoPostCommand
{
    public class UpdateGroupVideoPostCommandHandler : ICommandHandler<UpdateGroupVideoPostCommand, UpdateGroupVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateGroupVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }
        public async Task<Result<UpdateGroupVideoPostCommandResult>> Handle(UpdateGroupVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _queryContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var GroupPostVideo = await _queryContext.GroupPostVideos.FindAsync(request.GroupPostVideoId);
            if (GroupPostVideo == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            var userId = await _queryContext.GroupPosts
                                      .Where(a => a.GroupPostId == request.GroupPostId)
                                      .Select(a => a.UserId)
                                      .FirstOrDefaultAsync();

            if (request.UserId != userId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var VideoPost = _context.GroupPostVideos.Where(x => x.GroupPostVideoId == request.GroupPostVideoId).FirstOrDefault();
            if (VideoPost != null)
            {
                VideoPost.Content = request.Content;
                VideoPost.IsBanned = false;
            }
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(request.Content);
            if (haveBadWord.Any())
            {
                VideoPost.IsBanned = true;
                VideoPost.Content = _checkContent.MarkBannedWordsInContent(request.Content, haveBadWord);
            }
            var commandModel = new Domain.CommandModels.GroupPostVideo 
            {
                GroupPostVideoId = VideoPost.GroupPostVideoId,
                GroupPostId = VideoPost.GroupPostId,
                Content = VideoPost.Content,
                GroupVideoId = VideoPost.GroupPostId,
                GroupStatusId = VideoPost.GroupStatusId,
                GroupPostVideoNumber = VideoPost.GroupPostVideoNumber,
                IsHide = VideoPost.IsHide,
                CreatedAt = VideoPost.CreatedAt,
                UpdatedAt = VideoPost.UpdatedAt,
                PostPosition = VideoPost.PostPosition,
                IsBanned = VideoPost.IsBanned,
                GroupId = VideoPost.GroupId,
                IsPending = VideoPost.IsPending,
            };
            _context.GroupPostVideos.Update(commandModel);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UpdateGroupVideoPostCommandResult>(VideoPost);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateGroupVideoPostCommandResult>.Success(result);
            }
        }
    }
}
