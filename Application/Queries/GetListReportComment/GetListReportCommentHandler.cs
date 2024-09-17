using Application.DTO.CreateUserDTO;
using Application.Queries.GetListReportGroup;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportComment
{
    public class GetListReportCommentHandler : IQueryHandler<GetListReportCommentQuery, GetListReportCommentResult>
    {
        private readonly fptforumQueryContext _context;

        public GetListReportCommentHandler(fptforumQueryContext context)
        {
            _context = context;
        }

        public async Task<Result<GetListReportCommentResult>> Handle(GetListReportCommentQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reportListQuery = _context.ReportComments
                .AsNoTracking()
                .Include(x => x.ReportBy)
                .Where(x => x.Processing == true);

            Guid userId = Guid.NewGuid();
            // Áp dụng các điều kiện where tùy theo giá trị trong request
            if (request.CommentId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentId == request.CommentId);
                 userId = await _context.CommentPosts
                    .AsNoTracking()
                    .Where(x => x.CommentId == request.CommentId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentPhotoPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentPhotoPostId == request.CommentPhotoPostId);
                 userId = await _context.CommentPhotoPosts
                    .AsNoTracking()
                    .Where(x => x.CommentPhotoPostId == request.CommentPhotoPostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentVideoPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentVideoPostId == request.CommentVideoPostId);
                 userId = await _context.CommentVideoPosts
                    .AsNoTracking()
                    .Where(x => x.CommentVideoPostId == request.CommentVideoPostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentGroupPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentGroupPostId == request.CommentGroupPostId);
                 userId = await _context.CommentGroupPosts
                    .AsNoTracking()
                    .Where(x => x.CommentGroupPostId == request.CommentGroupPostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentGroupVideoPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentGroupVideoPostId == request.CommentGroupVideoPostId);
                 userId = await _context.CommentGroupVideoPosts
                    .AsNoTracking()
                    .Where(x => x.CommentGroupVideoPostId == request.CommentGroupVideoPostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentPhotoGroupPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId);
                 userId = await _context.CommentPhotoGroupPosts
                    .AsNoTracking()
                    .Where(x => x.CommentPhotoGroupPostId == request.CommentPhotoGroupPostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentSharePostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentSharePostId == request.CommentSharePostId);
                 userId = await _context.CommentSharePosts
                    .AsNoTracking()
                    .Where(x => x.CommentSharePostId == request.CommentSharePostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }
            else if (request.CommentGroupSharePostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.CommentGroupSharePostId == request.CommentGroupSharePostId);
                 userId = await _context.CommentGroupSharePosts
                    .AsNoTracking()
                    .Where(x => x.CommentGroupSharePostId == request.CommentGroupSharePostId)
                    .Select(x => x.UserId)
                    .FirstOrDefaultAsync();
            }

            var reportedUser = await _context.UserProfiles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.FullName)
            .FirstOrDefaultAsync();

            var reportList = reportListQuery.Select(x => new GetReportComment {
                ReportCommentId = x.ReportCommentId,
                ReportTypeId = x.ReportTypeId,
                CommentId = x.CommentId,
                CommentPhotoPostId = x.CommentPhotoPostId,
                CommentVideoPostId = x.CommentVideoPostId,
                CommentGroupPostId = x.CommentGroupPostId,
                CommentPhotoGroupPostId = x.CommentPhotoGroupPostId,
                CommentGroupVideoPostId = x.CommentGroupVideoPostId,
                CommentSharePostId = x.CommentSharePostId, 
                CommentGroupSharePostId = x.CommentGroupSharePostId,
                Content = x.Content,
                UserId = x.ReportById,
                UserName = x.ReportBy.FullName,
                AvatarUrl = _context.AvataPhotos.Where(ap => ap.UserId == x.ReportById && ap.IsUsed == true).Select(ap => ap.AvataPhotosUrl).FirstOrDefault(),
                ReportedUserId = userId,
                ReportedUserName = reportedUser,
                ReportedAvatarUrl = _context.AvataPhotos.Where(ap => ap.UserId == userId && ap.IsUsed == true).Select(ap => ap.AvataPhotosUrl).FirstOrDefault(),
                CreatedDate = x.CreatedDate,  
            }).ToList();

            var result = new GetListReportCommentResult {
                result = reportList.OrderByDescending(x => x.CreatedDate)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)reportList.Count() / request.PageSize),
            };
            return Result<GetListReportCommentResult>.Success(result);
        }
    }
}
