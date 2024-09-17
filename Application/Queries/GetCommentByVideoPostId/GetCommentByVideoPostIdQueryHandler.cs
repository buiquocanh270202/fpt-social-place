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

namespace Application.Queries.GetCommentByVideoPostId
{
    public class GetCommentByVideoPostIdQueryHandler : IQueryHandler<GetCommentByVideoPostIdQuery, GetCommentByVideoPostIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetCommentByVideoPostIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetCommentByVideoPostIdQueryResult>> Handle(GetCommentByVideoPostIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // Lấy danh sách bình luận, kèm thông tin người dùng, ảnh đại diện (nếu có) và sắp xếp
            var commentsQuery = from c in _context.CommentVideoPosts
                                  join a in _context.AvataPhotos on c.UserId equals a.UserId into ap
                                  from a in ap.Where(a => a.IsUsed == true).DefaultIfEmpty()
                                  where c.UserPostVideoId == request.UserPostVideoId && c.IsHide == false && c.IsBanned != true && c.User.IsActive == true
                                  orderby c.CreatedDate ascending
                                  select new CommentVideoDto
                                  {
                                    UserId = c.UserId,
                                    UserName = c.User.FirstName + " " + c.User.LastName,
                                    Url = a.AvataPhotosUrl,
                                    UserPostVideoId = c.UserPostVideoId,
                                    CreatedDate = c.CreatedDate,
                                    Content = c.Content,
                                    IsHide = c.IsHide,
                                    CommentVideoPostId = c.CommentVideoPostId,
                                    ParentCommentId = c.ParentCommentId,
                                    ListNumber = c.ListNumber, 
                                    Level = c.LevelCmt, 
                                    Replies = new List<CommentVideoDto>(),
                                    TotalReactCount = _context.ReactVideoPostComments.Count(rc => rc.CommentVideoPostId == c.CommentVideoPostId)
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

            // Xây dựng cấu trúc phân cấp bình luận
            var result = new GetCommentByVideoPostIdQueryResult
            {
                Posts = BuildCommentHierarchy(comments)
            };

            return Result<GetCommentByVideoPostIdQueryResult>.Success(result);
        }

        // Hàm xây dựng cấu trúc phân cấp (giống ví dụ trước, nhưng điều chỉnh cho CommentVideoDto)
        private List<CommentVideoDto> BuildCommentHierarchy(List<CommentVideoDto> comments)
        {
            var commentDict = comments.ToDictionary(c => c.CommentVideoPostId);

            foreach (var comment in comments)
            {
                if (comment.Level == 3 && !string.IsNullOrEmpty(comment.ListNumber)) // Xử lý comment level 3
                {
                    Guid parentId = Guid.Parse(comment.ListNumber);
                    if (commentDict.ContainsKey(parentId))
                    {
                        commentDict[parentId].Replies.Add(comment);
                    }
                }
                else if (comment.ParentCommentId.HasValue && commentDict.ContainsKey(comment.ParentCommentId.Value))
                {
                    commentDict[comment.ParentCommentId.Value].Replies.Add(comment);
                }
            }
            return comments.Where(c => !c.ParentCommentId.HasValue).ToList();
        }

    }

}
