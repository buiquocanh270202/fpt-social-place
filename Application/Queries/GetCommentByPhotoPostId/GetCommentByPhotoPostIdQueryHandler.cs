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

namespace Application.Queries.GetCommentByPhotoPostId
{
    public class GetCommentByPhotoPostIdQueryHandler : IQueryHandler<GetCommentByPhotoPostIdQuery, GetCommentByPhotoPostIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetCommentByPhotoPostIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<GetCommentByPhotoPostIdQueryResult>> Handle(GetCommentByPhotoPostIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            // Lấy danh sách bình luận, kèm theo thông tin người dùng và ảnh đại diện (nếu có)
            var commentsQuery = from c in _context.CommentPhotoPosts
                                  join a in _context.AvataPhotos on c.UserId equals a.UserId into ap
                                  from a in ap.Where(a => a.IsUsed == true).DefaultIfEmpty()
                                  where c.UserPostPhotoId == request.UserPostPhotoId && c.IsHide == false && c.IsBanned != true && c.User.IsActive == true
                                  orderby c.CreatedDate ascending
                                  select new CommentPhotoDto
                                  {
                                      UserId = c.UserId,
                                      UserName = c.User.FirstName + " " + c.User.LastName, // Lấy tên từ Users
                                      Url = a.AvataPhotosUrl, // Xử lý trường hợp không có ảnh đại diện
                                      UserPostPhotoId = c.UserPostPhotoId,
                                      CreatedDate = c.CreatedDate,
                                      Content = c.Content,
                                      IsHide = c.IsHide,
                                      CommentPhotoPostId = c.CommentPhotoPostId, // Giả sử đây là khóa chính
                                      ParentCommentId = c.ParentCommentId,
                                      Level = c.LevelCmt,
                                      ListNumber = c.ListNumber,
                                      Replies = new List<CommentPhotoDto>(), // Khởi tạo danh sách phản hồi
                                      TotalReactCount = _context.ReactPhotoPostComments.Count(rc => rc.CommentPhotoPostId == c.CommentPhotoPostId)
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
            var result = new GetCommentByPhotoPostIdQueryResult
            {
                Posts = BuildCommentHierarchy(comments)
            };

            return Result<GetCommentByPhotoPostIdQueryResult>.Success(result);
        }

        // Hàm xây dựng cấu trúc phân cấp (giống ví dụ trước, nhưng điều chỉnh cho CommentPhotoDto)
        private List<CommentPhotoDto> BuildCommentHierarchy(List<CommentPhotoDto> comments)
        {
            var commentDict = comments.ToDictionary(c => c.CommentPhotoPostId);

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
