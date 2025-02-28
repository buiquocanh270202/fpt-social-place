﻿using Application.Commands.CreateUserCommentPost;
using Application.DTO.CommentDTO;
using Application.Queries.GetAllFriend;
using Application.Queries.GetAllFriendOtherProfiel;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.GetCommentByPostId
{
    public class GetCommentByPostIdQueryHandler : IQueryHandler<GetCommentByPostIdQuery, GetCommentByPostIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetCommentByPostIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetCommentByPostIdQueryResult>> Handle(GetCommentByPostIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var commentsQuery = from c in _context.CommentPosts
                                join a in _context.AvataPhotos on c.UserId equals a.UserId into ap
                                from a in ap.Where(a => a.IsUsed == true).DefaultIfEmpty()
                                where c.UserPostId == request.UserPostId && c.IsHide == false && c.IsBanned != true && c.User.IsActive == true
                                select new CommentDto
                                {
                                    UserId = c.UserId,
                                    UserName = c.User.FirstName + " " + c.User.LastName,
                                    Url = a.AvataPhotosUrl,
                                    UserPostId = c.UserPostId,
                                    CreatedDate = c.CreatedDate,
                                    Content = c.Content,
                                    IsHide = c.IsHide,
                                    CommentId = c.CommentId,
                                    ParentCommentId = c.ParentCommentId,
                                    Level = c.LevelCmt,
                                    ListNumber = c.ListNumber,
                                    Replies = new List<CommentDto>(),
                                    TotalReactCount = _context.ReactComments.Count(rc => rc.CommentId == c.CommentId)
                                };

            if (request.Type == "New")
            {
                commentsQuery = commentsQuery.OrderByDescending(c => c.CreatedDate);
            }
            else if (request.Type == "Most relevant")
            {
                commentsQuery = commentsQuery.OrderByDescending(c => c.TotalReactCount).ThenByDescending(c => c.CreatedDate);
            }
            else
            {
                commentsQuery = commentsQuery.OrderBy(c => c.CreatedDate);
            }

            var comments = await commentsQuery.ToListAsync(cancellationToken);


            var result = new GetCommentByPostIdQueryResult
            {
                Posts = BuildCommentHierarchy(comments)
            };

            return Result<GetCommentByPostIdQueryResult>.Success(result);
        }

        private List<CommentDto> BuildCommentHierarchy(List<CommentDto> comments)
        {
            var commentDict = comments.ToDictionary(c => c.CommentId);

            foreach (var comment in comments)
            {
                // Xử lý comment level 3
                if (comment.Level == 3 && !string.IsNullOrEmpty(comment.ListNumber))
                {
                    Guid parentId = Guid.Parse(comment.ListNumber); // Chuyển ListNumber thành Guid

                    if (commentDict.ContainsKey(parentId))
                    {
                        commentDict[parentId].Replies.Add(comment); // Thêm vào Replies của comment cha (level 2)
                    }
                }
                else if (comment.ParentCommentId.HasValue && commentDict.ContainsKey(comment.ParentCommentId.Value))
                {
                    commentDict[comment.ParentCommentId.Value].Replies.Add(comment); // Thêm vào Replies của comment cha (level 2)

                }
            }

            return comments.Where(c => !c.ParentCommentId.HasValue).ToList();
        }

    }
}
