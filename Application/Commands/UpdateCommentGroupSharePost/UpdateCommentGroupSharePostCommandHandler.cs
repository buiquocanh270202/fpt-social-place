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

namespace Application.Commands.UpdateCommentGroupSharePost
{
    public class UpdateCommentGroupSharePostCommandHandler : ICommandHandler<UpdateCommentGroupSharePostCommand, UpdateCommentGroupSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateCommentGroupSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<UpdateCommentGroupSharePostCommandResult>> Handle(UpdateCommentGroupSharePostCommand request, CancellationToken cancellationToken)
        {
            // Check if the context is null
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // Check if the request content is null or whitespace
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }
            var GroupSharePost = await _querycontext.CommentGroupSharePosts.FindAsync(request.CommentGroupSharePostId);
            if (GroupSharePost == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }

            if (request.UserId != GroupSharePost.UserId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var comment = await _querycontext.CommentGroupSharePosts.Where(x => x.CommentGroupSharePostId == request.CommentGroupSharePostId).FirstOrDefaultAsync();
            List<CheckingBadWord.BannedWord> bannedWords = _checkContent.Compare2String(request.Content);

            if (comment != null) 
            {
                comment.Content = request.Content;
                if (bannedWords.Any())
                {
                    comment.IsHide = true;
                }
            }
            var cgsp = new Domain.CommandModels.CommentGroupSharePost 
            {
                CommentGroupSharePostId = comment.CommentGroupSharePostId,
                GroupSharePostId = comment.CommentGroupSharePostId,
                UserId = comment.UserId,
                Content = comment.Content,
                ParentCommentId = comment.CommentGroupSharePostId,
                ListNumber = comment.ListNumber,
                LevelCmt = comment.LevelCmt,
                IsHide = true,
                CreateDate = comment.CreateDate,
                IsBanned = comment.IsBanned,
            };
            _context.CommentGroupSharePosts.Update(cgsp);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<UpdateCommentGroupSharePostCommandResult>(comment);
            result.BannedWords = bannedWords;
            if (bannedWords.Any())
            {
                throw new ErrorException(StatusCodeEnum.CM03_Comment_Contain_Bad_Word);
            }
            // Return the result
            return Result<UpdateCommentGroupSharePostCommandResult>.Success(result);

        }
    }
}
