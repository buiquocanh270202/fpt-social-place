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

namespace Application.Commands.UpdateSharePost
{
    public class UpdateSharePostCommandHandler : ICommandHandler<UpdateSharePostCommand, UpdateSharePostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateSharePostCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }
        public async Task<Result<UpdateSharePostCommandResult>> Handle(UpdateSharePostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var sharePost = await _querycontext.SharePosts.FindAsync(request.SharePostId);
            if (sharePost == null)
            {
                throw new ErrorException(StatusCodeEnum.UP02_Post_Not_Found);
            }
            var userId = await _querycontext.SharePosts
                                      .Where(a => a.SharePostId == request.SharePostId)
                                      .Select(a => a.UserId)
                                      .FirstOrDefaultAsync();

            if (request.UserId != userId)
            {
                throw new ErrorException(StatusCodeEnum.UP03_Not_Authorized);
            }

            var photoPost = _querycontext.SharePosts.Where(x => x.SharePostId == request.SharePostId).FirstOrDefault();
            if (photoPost != null) 
            {
                photoPost.Content = request.Content;
                photoPost.UserStatusId = request.UserStatusId;
                photoPost.IsBanned = false;
            }
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(request.Content);
            if (haveBadWord.Any())
            {
                photoPost.IsBanned = true;
                photoPost.Content = _checkContent.MarkBannedWordsInContent(request.Content, haveBadWord);
            }
            Domain.CommandModels.SharePost sp = new Domain.CommandModels.SharePost
            {
                SharePostId = photoPost.SharePostId,
                UserId = photoPost.UserId,
                Content = photoPost.Content,
                UserPostId = photoPost.UserPostId,
                UserPostVideoId = photoPost.UserPostVideoId,
                UserPostPhotoId = photoPost.UserPostPhotoId,
                GroupPostId = photoPost.GroupPostId,
                GroupPostPhotoId = photoPost.GroupPostPhotoId,
                GroupPostVideoId = photoPost.GroupPostVideoId,
                CreatedDate = photoPost.CreatedDate,
                UserStatusId = photoPost.UserStatusId,
                IsHide = photoPost.IsHide,
                UpdateDate = DateTime.Now,
                IsBanned = photoPost.IsBanned,
                UserSharedId = photoPost.UserSharedId,
            };
            _context.SharePosts.Update(sp);
            _context.SaveChanges();

            var result = _mapper.Map<UpdateSharePostCommandResult>(sp);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateSharePostCommandResult>.Success(result);
            }

        }
    }
}

