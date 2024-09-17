using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.CommentDTO
{
    public class CommentDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserPostId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<CommentDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class CommentVideoDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentVideoPostId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<CommentVideoDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class CommentPhotoDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentPhotoPostId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<CommentPhotoDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class CommentSharePostDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentSharePostId { get; set; }
        public Guid SharePostId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<CommentSharePostDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class GroupCommentDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentGroupPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<GroupCommentDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class GroupPhotoCommentDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentPhotoGroupPostId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<GroupPhotoCommentDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class GroupVideoCommentDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentGroupVideoPostId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<GroupVideoCommentDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }

    public class CommentGroupSharePostDto
    {
        public Guid UserId { get; set; }
        public string Url { get; set; }
        public Guid CommentGroupSharePostId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public Guid? ParentCommentId { get; set; }
        public bool? IsHide { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Level { get; set; }
        public List<CommentGroupSharePostDto>? Replies { get; set; }
        public string? ListNumber { get; set; }
        public int? TotalReactCount { get; set; }
    }
}
