using Application.DTO.CommentDTO;
using Application.Queries.GetCommentByPostId;
using AutoMapper;
using Core.CQRS.Query;
using Core.CQRS;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetCommentByGroupPostId
{
    public class GetCommentByGroupPostIdQueryHandler : IQueryHandler<GetCommentByGroupPostIdQuery, GetCommentByGroupPostIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetCommentByGroupPostIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetCommentByGroupPostIdQueryResult>> Handle(GetCommentByGroupPostIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var commentsQuery = from c in _context.CommentGroupPosts
                                    join a in _context.AvataPhotos on c.UserId equals a.UserId into ap
                                    from a in ap.Where(a => a.IsUsed == true).DefaultIfEmpty()
                                    where c.GroupPostId == request.GroupPostId && c.IsHide == false && c.IsBanned != true && c.User.IsActive == true
                                    orderby c.CreatedDate ascending
                                    select new GroupCommentDto
                                    {
                                        UserId = c.UserId,
                                        UserName = c.User.FirstName + " " + c.User.LastName,
                                        Url = a.AvataPhotosUrl,
                                        GroupPostId = c.GroupPostId,
                                        CreatedDate = c.CreatedDate,
                                        Content = c.Content,
                                        IsHide = c.IsHide,
                                        CommentGroupPostId = c.CommentGroupPostId,
                                        ParentCommentId = c.ParentCommentId,
                                        Level = c.LevelCmt,
                                        ListNumber = c.ListNumber,
                                        Replies = new List<GroupCommentDto>(),
                                        TotalReactCount = _context.ReactGroupCommentPosts.Count(rc => rc.CommentGroupPostId == c.CommentGroupPostId)
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

            var result = new GetCommentByGroupPostIdQueryResult
            {
                Posts = BuildCommentHierarchy(comments)
            };

            return Result<GetCommentByGroupPostIdQueryResult>.Success(result);
        }

        private List<GroupCommentDto> BuildCommentHierarchy(List<GroupCommentDto> comments)
        {
            var commentDict = comments.ToDictionary(c => c.CommentGroupPostId);

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

