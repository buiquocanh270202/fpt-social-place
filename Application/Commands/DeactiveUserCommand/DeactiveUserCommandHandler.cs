using Core.CQRS;
using Core.CQRS.Command;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeactiveUserCommand
{
    public class DeactiveUserCommandHandler : ICommandHandler<DeactiveUserCommand, DeactiveUserCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;

        public DeactiveUserCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext)
        {
            _context = context;
            _querycontext = querycontext;
        }

        public async Task<Result<DeactiveUserCommandResult>> Handle(DeactiveUserCommand request, CancellationToken cancellationToken)
        {
            if (_context == null || _context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var user = await _querycontext.UserProfiles
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            if (user.IsActive == false)
            {
                var result = new DeactiveUserCommandResult();
                result.Message = "User is not active";
                return Result<DeactiveUserCommandResult>.Success(result);
            }

            // Deactive trong bảng UserProfile
            var commandUser = await _context.UserProfiles
                .Where(x => x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (commandUser == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            commandUser.IsActive = false;
            /*await _context.SaveChangesAsync(cancellationToken);*/

            // Chỉnh avatar isUse thành false
            /*var avatar = await _context.AvataPhotos
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if(avatar != null)
            {
                avatar.IsUsed = false;
            }*/

            // Ẩn comment
            var commentPost = await _context.CommentPosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentPost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentPost>(comment.CommentId, "CommentId", cancellationToken);
            }

            var commentPhotoPost = await _context.CommentPhotoPosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentPhotoPost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentPhotoPost>(comment.CommentPhotoPostId, "CommentPhotoPostId", cancellationToken);
            }

            var commentVideoPost = await _context.CommentVideoPosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentVideoPost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentVideoPost>(comment.CommentVideoPostId, "CommentVideoPostId", cancellationToken);
            }

            var commentGroupPost = await _context.CommentGroupPosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentGroupPost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentGroupPost>(comment.CommentGroupPostId, "CommentGroupPostId", cancellationToken);
            }

            var commentPhotoGroupPost = await _context.CommentPhotoGroupPosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentPhotoGroupPost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentPhotoGroupPost>(comment.CommentPhotoGroupPostId, "CommentPhotoGroupPostId", cancellationToken);
            }

            var commentGroupVideoPost = await _context.CommentGroupVideoPosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentGroupVideoPost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentGroupVideoPost>(comment.CommentGroupVideoPostId, "CommentGroupVideoPostId", cancellationToken);
            }

            var commentSharePost = await _context.CommentSharePosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentSharePost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentSharePost>(comment.CommentSharePostId, "CommentSharePostId", cancellationToken);
            }

            var commentGroupSharePost = await _context.CommentGroupSharePosts
                .AsNoTracking()
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);
            foreach (var comment in commentGroupSharePost)
            {
                await SoftDeleteCommentAndChildrenAsync<Domain.CommandModels.CommentGroupSharePost>(comment.CommentGroupSharePostId, "CommentGroupSharePostId", cancellationToken);
            }

            // Ẩn post
            var userPosts = await _context.UserPosts
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            // Lưu tất cả các bình luận và tài nguyên liên quan vào danh sách cần cập nhật
            var userPostIds = userPosts.Select(p => p.UserPostId).ToList();
            var userPostPhotos = await _context.UserPostPhotos
                .Where(x => userPostIds.Contains(x.UserPostId) && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            var userPostVideos = await _context.UserPostVideos
                .Where(x => userPostIds.Contains(x.UserPostId) && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            var groupPost = await _context.GroupPosts
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            // Lưu tất cả các bình luận và tài nguyên liên quan vào danh sách cần cập nhật
            var groupPostIds = groupPost.Select(p => p.GroupPostId).ToList();
            var groupPostPhoto = await _context.GroupPostPhotos
                .Where(x => userPostIds.Contains(x.GroupPostId) && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            var groupPostVideo = await _context.GroupPostVideos
                .Where(x => userPostIds.Contains(x.GroupPostId) && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            var sharePost = await _context.SharePosts
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            var groupSharePost = await _context.GroupSharePosts
                .Where(x => x.UserId == user.UserId && x.IsHide != true && x.IsBanned != true)
                .ToListAsync(cancellationToken);

            // Cập nhật trạng thái của bài đăng
            foreach (var post in userPosts)
            {
                post.IsHide = true;
            }
            
            // Cập nhật trạng thái của ảnh
            foreach (var photo in userPostPhotos)
            {
                photo.IsHide = true;
            }

            // Cập nhật trạng thái của video
            foreach (var video in userPostVideos)
            {
                video.IsHide = true;
            }

            // Cập nhật trạng thái của bài đăng
            foreach (var post in groupPost)
            {
                post.IsHide = true;
            }

            // Cập nhật trạng thái của ảnh
            foreach (var photo in groupPostPhoto)
            {
                photo.IsHide = true;
            }

            // Cập nhật trạng thái của video
            foreach (var video in groupPostVideo)
            {
                video.IsHide = true;
            }

            // Cập nhật trạng thái của bài đăng
            foreach (var post in sharePost)
            {
                post.IsHide = true;
            }

            // Cập nhật trạng thái của bài đăng
            foreach (var post in groupSharePost)
            {
                post.IsHide = true;
            }

            await _context.SaveChangesAsync();

            var results = new DeactiveUserCommandResult();
            results.Message = "Sucess to deactive user.";
            return Result<DeactiveUserCommandResult>.Success(results);
        }

        public async Task SoftDeleteCommentAndChildrenAsync<TEntity>(
            Guid commentId,
            string commentIdProperty,
            CancellationToken cancellationToken)
            where TEntity : class
                {
                    var dbSet = _context.Set<TEntity>();

                    // Tìm tất cả các bình luận con theo cách đệ quy
                    var commentsToUpdate = new List<TEntity>();
                    var stack = new Stack<Guid>();
                    stack.Push(commentId);

                    while (stack.Count > 0)
                    {
                        Guid currentCommentId = stack.Pop();

                        // Tìm các bình luận con của bình luận hiện tại
                        var childComments = await dbSet
                            .Where(x => EF.Property<Guid?>(x, "ParentCommentId") == currentCommentId)
                            .ToListAsync(cancellationToken);

                        foreach (var childComment in childComments)
                        {
                            var childCommentId = (Guid)typeof(TEntity)
                                .GetProperty(commentIdProperty)
                                .GetValue(childComment);

                            stack.Push(childCommentId);
                            commentsToUpdate.Add(childComment);
                        }
                    }

                    // Đánh dấu IsHide = true cho tất cả các bình luận cần xóa mềm
                    foreach (var commentToUpdate in commentsToUpdate)
                    {
                        var isHidePropertyInfo = typeof(TEntity).GetProperty("IsHide");
                        if (isHidePropertyInfo != null)
                        {
                            isHidePropertyInfo.SetValue(commentToUpdate, true);
                        }
                    }

                    // Tìm bình luận gốc và xóa mềm nó
                    var rootComment = await dbSet.FindAsync(new object[] { commentId }, cancellationToken);
                    if (rootComment != null)
                    {
                        var isHidePropertyOriginal = typeof(TEntity).GetProperty("IsHide");
                        if (isHidePropertyOriginal != null)
                        {
                            isHidePropertyOriginal.SetValue(rootComment, true);
                        }
                    }
                    // Lưu thay đổi
                    await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
