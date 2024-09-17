using Application.Commands.UpdateGroupPostCommand;
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

namespace Application.Commands.UpdateGroupPhotoPostCommand
{
    public class UpdateGroupPhotoPostCommandHandler : ICommandHandler<UpdateGroupPhotoPostCommand, UpdateGroupPhotoPostCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _queryContext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly CheckingBadWord _checkContent;

        public UpdateGroupPhotoPostCommandHandler(fptforumCommandContext context, fptforumQueryContext queryContext, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _queryContext = queryContext;
            _mapper = mapper;
            _helper = new GuidHelper();
            _configuration = configuration;
            _checkContent = new CheckingBadWord();
        }
        public async Task<Result<UpdateGroupPhotoPostCommandResult>> Handle(UpdateGroupPhotoPostCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _queryContext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var GroupPostPhoto = await _queryContext.GroupPostPhotos.FindAsync(request.GroupPostPhotoId);
            if (GroupPostPhoto == null)
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

            var photoPost = _queryContext.GroupPostPhotos.Where(x => x.GroupPostPhotoId == request.GroupPostPhotoId).FirstOrDefault();
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

            var commandModel = new Domain.CommandModels.GroupPostPhoto 
            {
                GroupPostPhotoId = photoPost.GroupPostPhotoId,
                GroupPostId = photoPost.GroupPostId,
                Content = photoPost.Content,
                GroupPhotoId = photoPost.GroupPhotoId,
                GroupStatusId = photoPost.GroupStatusId,
                GroupPostPhotoNumber = photoPost.GroupPostPhotoNumber,
                IsHide = photoPost.IsHide,
                CreatedAt = photoPost.CreatedAt,
                UpdatedAt = photoPost.UpdatedAt,
                PostPosition = photoPost.PostPosition,
                IsBanned = photoPost.IsBanned,
                GroupId = photoPost.GroupId,
                IsPending = photoPost.IsPending,
            };
            _context.GroupPostPhotos.Update(commandModel);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UpdateGroupPhotoPostCommandResult>(photoPost);
            result.BannedWords = new List<BannedWord>();
            result.BannedWords = haveBadWord;
            if (haveBadWord.Any())
            {
                throw new ErrorException(StatusCodeEnum.UP01_Post_Have_Bad_Word);
            }
            else
            {
                return Result<UpdateGroupPhotoPostCommandResult>.Success(result);
            }

        }
    }
}

