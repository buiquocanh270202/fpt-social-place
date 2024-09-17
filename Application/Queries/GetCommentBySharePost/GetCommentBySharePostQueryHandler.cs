using Application.DTO.CommentDTO;
using Application.Queries.GetCommentByPostId;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Queries.GetCommentBySharePost
{
    public class GetCommentBySharePostQueryHandler : IQueryHandler<GetCommentBySharePostQuery, GetCommentBySharePostQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetCommentBySharePostQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetCommentBySharePostQueryResult>> Handle(GetCommentBySharePostQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var commentsQuery = from c in _context.CommentSharePosts
                                 join a in _context.AvataPhotos on c.UserId equals a.UserId into ap
                                 from a in ap.Where(a => a.IsUsed == true).DefaultIfEmpty()
                                 where c.SharePostId == request.SharePostId && c.IsHide == false && c.IsBanned != true && c.User.IsActive == true
                                 orderby c.CreateDate ascending
                                 select new CommentSharePostDto
                                 {
                                     UserId = c.UserId,
                                     UserName = c.User.FirstName + " " + c.User.LastName,
                                     Url = a.AvataPhotosUrl,
                                     SharePostId = c.SharePostId,
                                     CreatedDate = c.CreateDate,
                                     Content = c.Content,
                                     IsHide = c.IsHide,
                                     CommentSharePostId = c.CommentSharePostId,
                                     ParentCommentId = c.ParentCommentId,
                                     Level = c.LevelCmt,
                                     ListNumber = c.ListNumber,
                                     Replies = new List<CommentSharePostDto>(),
                                     TotalReactCount = _context.ReactSharePostComments.Count(rc => rc.CommentSharePostId == c.CommentSharePostId)
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

            var result = new GetCommentBySharePostQueryResult
            {
                Posts = BuildCommentHierarchy(comments)
            };

            return Result<GetCommentBySharePostQueryResult>.Success(result);
        }

        private List<CommentSharePostDto> BuildCommentHierarchy(List<CommentSharePostDto> comments)
        {
            var commentDict = comments.ToDictionary(c => c.CommentSharePostId);

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
