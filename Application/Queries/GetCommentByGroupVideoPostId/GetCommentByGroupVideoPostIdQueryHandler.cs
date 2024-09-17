using Application.DTO.CommentDTO;
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

namespace Application.Queries.GetCommentByGroupVideoPostId
{
    public class GetCommentByGroupVideoPostIdQueryHandler : IQueryHandler<GetCommentByGroupVideoPostIdQuery, GetCommentByGroupVideoPostIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetCommentByGroupVideoPostIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetCommentByGroupVideoPostIdQueryResult>> Handle(GetCommentByGroupVideoPostIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var commentsQuery = from c in _context.CommentGroupVideoPosts // Thay đổi bảng truy vấn
                                  join a in _context.AvataPhotos on c.UserId equals a.UserId into ap
                                  from a in ap.Where(a => a.IsUsed == true).DefaultIfEmpty()
                                  where c.GroupPostVideoId == request.GroupPostVideoId && c.IsHide == false && c.IsBanned != true && c.User.IsActive == true// Thay đổi điều kiện lọc
                                  orderby c.CreatedDate ascending
                                  select new GroupVideoCommentDto // Sử dụng DTO tương ứng cho video comment
                                  {
                                      UserId = c.UserId,
                                      UserName = c.User.FirstName + " " + c.User.LastName,
                                      Url = a.AvataPhotosUrl,
                                      GroupPostVideoId = c.GroupPostVideoId, // Thay đổi thuộc tính ID
                                      CreatedDate = c.CreatedDate,
                                      Content = c.Content,
                                      IsHide = c.IsHide,
                                      CommentGroupVideoPostId = c.CommentGroupVideoPostId, // Thay đổi thuộc tính ID
                                      ParentCommentId = c.ParentCommentId,
                                      Level = c.LevelCmt,
                                      ListNumber = c.ListNumber,
                                      Replies = new List<GroupVideoCommentDto>(), // Sử dụng DTO tương ứng cho video comment
                                      TotalReactCount = _context.ReactGroupVideoPostComments.Count(rc => rc.CommentGroupVideoPostId == c.CommentGroupVideoPostId)
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

            var result = new GetCommentByGroupVideoPostIdQueryResult
            {
                Posts = BuildCommentHierarchy(comments) // Tái sử dụng hàm BuildCommentHierarchy
            };

            return Result<GetCommentByGroupVideoPostIdQueryResult>.Success(result);
        }

        // Hàm BuildCommentHierarchy có thể được tái sử dụng từ lớp GetCommentByGroupPostIdQueryHandler
        // vì logic xây dựng cấu trúc phân cấp bình luận là tương tự
        private List<GroupVideoCommentDto> BuildCommentHierarchy(List<GroupVideoCommentDto> comments)
        {
            var commentDict = comments.ToDictionary(c => c.CommentGroupVideoPostId);

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
