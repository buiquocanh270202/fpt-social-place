using Application.Commands.CreateUserCommentPost;
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

namespace Application.Commands.CreateUserCommentVideoPost
{
    public class CreateUserCommentVideoPostCommandHandler : ICommandHandler<CreateUserCommentVideoPostCommand, CreateUserCommentVideoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public CreateUserCommentVideoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<CreateUserCommentVideoPostCommandResult>> Handle(CreateUserCommentVideoPostCommand request, CancellationToken cancellationToken)
        {
            // Check for null context
            if (_context == null || _queryContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // Check if content is null or whitespace
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }

            // Initialize comment level and list number
            int levelCmt = 1;
            string listNumber = null;
            var postReactCount = await _queryContext.PostReactCounts.Where(prc => prc.UserPostVideoId == request.UserPostVideoId).FirstOrDefaultAsync();

            // Check if there's a parent comment
            if (request.ParentCommentId.HasValue)
            {
                // Find parent comment in the database
                var parentComment = await _queryContext.CommentVideoPosts.FindAsync(request.ParentCommentId.Value);

                // If parent comment doesn't exist, throw an error
                if (parentComment == null || parentComment.IsHide == true)
                {
                    throw new ErrorException(StatusCodeEnum.CM02_Parent_Comment_Not_Found);
                }

                // Determine the level of the new comment based on parent
                if (parentComment.LevelCmt == 1)
                {
                    levelCmt = 2;
                }
                else if (parentComment.LevelCmt == 2)
                {
                    listNumber = parentComment.CommentVideoPostId.ToString();
                    levelCmt = 3;
                }
                else if (parentComment.LevelCmt == 3)
                {
                    listNumber = parentComment.ListNumber;
                    levelCmt = 3;
                }
            }

            // Create new comment with determined level and list number
            var comment = new Domain.CommandModels.CommentVideoPost
            {
                CommentVideoPostId = _helper.GenerateNewGuid(),
                UserPostVideoId = request.UserPostVideoId,
                UserId = request.UserId,
                Content = request.Content,
                ParentCommentId = request.ParentCommentId,
                IsHide = false,
                CreatedDate = DateTime.Now,
                LevelCmt = levelCmt,
                ListNumber = listNumber
            };

            // Check for banned words
            List<CheckingBadWord.BannedWord> badWords = _checkContent.Compare2String(comment.Content);
            if (badWords.Any())
            {
                comment.IsHide = true;
            }

            if (postReactCount != null && !badWords.Any())
            {
                postReactCount.CommentCount++;
            }

            // Add comment to the database and save changes
            var prc = new Domain.CommandModels.PostReactCount
            {
                PostReactCountId = postReactCount.PostReactCountId,
                UserPostId = postReactCount.UserPostId,
                UserPostPhotoId = postReactCount.UserPostPhotoId,
                ReactCount = postReactCount.ReactCount,
                CommentCount = postReactCount.CommentCount,
                ShareCount = postReactCount.ShareCount,
                CreateAt = postReactCount.CreateAt,
                UpdateAt = DateTime.Now,
            };
            _context.PostReactCounts.Update(prc);
            await _context.CommentVideoPosts.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Map the result and include banned words
            var result = _mapper.Map<CreateUserCommentVideoPostCommandResult>(comment);
            result.BannedWords = badWords;
            if (badWords.Any())
            {
                throw new ErrorException(StatusCodeEnum.CM03_Comment_Contain_Bad_Word);
            }
            return Result<CreateUserCommentVideoPostCommandResult>.Success(result);
        }
    }
}
