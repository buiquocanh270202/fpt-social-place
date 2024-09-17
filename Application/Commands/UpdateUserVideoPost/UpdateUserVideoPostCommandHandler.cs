using Application.Commands.UpdateUserPhotoPost;
using Application.Commands.UpdateUserVideoPost;
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

namespace Application.Commands.UpdateUserVideoPost
{
    public class UpdateUserVideoPostCommandHandler : ICommandHandler<UpdateUserVideoPostCommand, UpdateUserVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateUserVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }
        public async Task<Result<UpdateUserVideoPostCommandResult>> Handle(UpdateUserVideoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPostVideo = await _queryContext.UserPostVideos.FindAsync(request.UserPostVideoId);
            if (userPostVideo == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            var userId = await _queryContext.UserPosts
                                      .Where(a => a.UserPostId == request.UserPostId)
                                      .Select(a => a.UserId)
                                      .FirstOrDefaultAsync();

            if (request.UserId != userId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var VideoPost = _queryContext.UserPostVideos.Where(x => x.UserPostVideoId == request.UserPostVideoId).FirstOrDefault();
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
            var cgp = new Domain.CommandModels.UserPostVideo 
            {
                UserPostVideoId = VideoPost.UserPostVideoId,
                UserPostId = VideoPost.UserPostId,
                VideoId = VideoPost.VideoId,
                Content = VideoPost.Content,
                UserPostVideoNumber = VideoPost.UserPostVideoNumber,
                UserStatusId = VideoPost.UserStatusId,
                IsHide = VideoPost.IsHide,
                CreatedAt = VideoPost.CreatedAt,
                UpdatedAt = VideoPost.UpdatedAt,
                PostPosition = VideoPost.PostPosition,
                IsBanned = VideoPost.IsBanned,
            };
            _context.UserPostVideos.Update(cgp);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UpdateUserVideoPostCommandResult>(VideoPost);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateUserVideoPostCommandResult>.Success(result);
            }
        }
    }
}
