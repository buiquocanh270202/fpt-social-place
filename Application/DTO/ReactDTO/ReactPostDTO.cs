using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.ReactDTO
{
    public class ReactPostDTO
    {
        public Guid ReactPostId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName {  get; set; }
        public DateTime? CreatedDate {  get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactPhotoPostDTO
    {
        public Guid ReactPhotoPostId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactVideoPostDTO
    {
        public Guid ReactVideoPostId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactSharePostDTO
    {
        public Guid ReactSharePostId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactCommentDTO
    {
        public Guid ReactCommentId { get; set; }
        public Guid UserPostId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid CommentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactCommentPhotoDTO
    {
        public Guid ReactPhotoPostCommentId { get; set; }
        public Guid UserPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid CommentPhotoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactCommentVideoDTO
    {
        public Guid ReactVideoPostCommentId { get; set; }
        public Guid UserPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid CommentVideoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactSharePostCommentDTO
    {
        public Guid ReactSharePostCommentId { get; set; }
        public Guid SharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid? CommentSharePostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupPostDTO
    {
        public Guid ReactGroupPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupPhotoPostDTO
    {
        public Guid ReactGroupPhotoPostId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupVideoPostDTO
    {
        public Guid ReactGroupVideoPostId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupSharePostDTO
    {
        public Guid ReactGroupSharePostId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupCommentDTO
    {
        public Guid ReactGroupCommentPostId { get; set; }
        public Guid GroupPostId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid CommentGroupPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupCommentPhotoDTO
    {
        public Guid ReactPhotoPostCommentId { get; set; }
        public Guid GroupPostPhotoId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid CommentPhotoGroupPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupCommentVideoDTO
    {
        public Guid ReactGroupVideoCommentId { get; set; }
        public Guid GroupPostVideoId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid CommentGroupVideoPostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactGroupSharePostCommentDTO
    {
        public Guid ReactGroupSharePostCommentId { get; set; }
        public Guid GroupSharePostId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ReactTypeId { get; set; }
        public string? ReactTypeName { get; set; }
        public Guid? CommentGroupSharePostId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }

    public class ReactTypeCountDTO
    {
        public Guid ReactTypeId { get; set; }
        public string ReactTypeName { get; set; }
        public int NumberReact { get; set; }
    }

    public class ReactDetailDTO
    {
        public Guid ReactTypeId { get; set; }
        public string ReactName { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? AvataUrl { get; set; }
        public string? Status { get; set; }
    }
}
