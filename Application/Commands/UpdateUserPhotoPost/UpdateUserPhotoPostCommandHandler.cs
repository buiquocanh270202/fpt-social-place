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

namespace Application.Commands.UpdateUserPhotoPost
{
    public class UpdateUserPhotoPostCommandHandler : ICommandHandler<UpdateUserPhotoPostCommand, UpdateUserPhotoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateUserPhotoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }
        public async Task<Result<UpdateUserPhotoPostCommandResult>> Handle(UpdateUserPhotoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userPostPhoto = await _queryContext.UserPostPhotos.FindAsync(request.UserPostPhotoId);
            if (userPostPhoto == null)
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

            var photoPost = _queryContext.UserPostPhotos.Where(x => x.UserPostPhotoId == request.UserPostPhotoId).FirstOrDefault();
            if (photoPost != null) 
            {
                photoPost.Content = request.Content;
                photoPost.IsBanned = false;
            }
            List<CheckingBadWord.BannedWord> haveBadWord = _checkContent.Compare2String(request.Content);
            if (haveBadWord.Any())
            {
                photoPost.IsBanned = true;
                photoPost.Content = _checkContent.MarkBannedWordsInContent(request.Content, haveBadWord);
            }
            var upp = new Domain.CommandModels.UserPostPhoto
            {
                UserPostPhotoId = photoPost.UserPostPhotoId,
                UserPostId = photoPost.UserPostId,
                PhotoId = photoPost.PhotoId,
                Content = photoPost.Content,
                UserPostPhotoNumber = photoPost.UserPostPhotoNumber,
                UserStatusId = photoPost.UserStatusId,
                IsHide = photoPost.IsHide,
                CreatedAt = photoPost.CreatedAt,
                UpdatedAt = photoPost.UpdatedAt,
                PostPosition = photoPost.PostPosition,
                IsBanned = photoPost.IsBanned,
            };
            _context.UserPostPhotos.Update(upp);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UpdateUserPhotoPostCommandResult>(photoPost);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateUserPhotoPostCommandResult>.Success(result);
            }
        }
    }
}

