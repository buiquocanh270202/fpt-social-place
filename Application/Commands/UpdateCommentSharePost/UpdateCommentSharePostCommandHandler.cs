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
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateCommentSharePost
{
    public class UpdateCommentSharePostCommandHandler : ICommandHandler<UpdateCommentSharePostCommand, UpdateCommentSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateCommentSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }

        public async Task<Result<UpdateCommentSharePostCommandResult>> Handle(UpdateCommentSharePostCommand request, CancellationToken cancellationToken)
        {
            // Check if the context is null
            if (_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // Check if the request content is null or whitespace
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                throw new ErrorException(StatusCodeEnum.CM01_Comment_Not_Null);
            }
            var SharePost = await _querycontext.CommentSharePosts.FindAsync(request.CommentSharePostId);
            if (SharePost == null)
            {
                throw new ErrorException(StatusCodeEnum.CM04_Comment_Not_Found);
            }

            if (request.UserId != SharePost.UserId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var comment = await _querycontext.CommentSharePosts.Where(x => x.CommentSharePostId == request.CommentSharePostId).FirstOrDefaultAsync();
            List<CheckingBadWord.BannedWord> bannedWords = _checkContent.Compare2String(request.Content);

            if (comment != null) 
            {
                comment.Content = request.Content;
                if (bannedWords.Any())
                {
                    comment.IsHide = true;
                }
            }
            var csp = new Domain.CommandModels.CommentSharePost 
            {
                CommentSharePostId = comment.CommentSharePostId,
                SharePostId = comment.SharePostId,
                UserId = comment.UserId,
                Content = comment.Content,
                ParentCommentId = comment.ParentCommentId,
                ListNumber = comment.ListNumber,
                LevelCmt = comment.LevelCmt,
                IsHide = comment.IsHide,
                CreateDate = comment.CreateDate,
                IsBanned = comment.IsBanned,
            };
            _context.CommentSharePosts.Update(csp);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<UpdateCommentSharePostCommandResult>(comment);
            result.BannedWords = bannedWords;
            if (bannedWords.Any())
            {
                throw new ErrorException(StatusCodeEnum.CM03_Comment_Contain_Bad_Word);
            }
            // Return the result
            return Result<UpdateCommentSharePostCommandResult>.Success(result);

        }
    }
}
