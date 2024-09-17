using Application.Queries.GetListReportUser;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Queries.GetListReportPost
{
    public class GetListReportPostHandler : IQueryHandler<GetListReportPostQuery, GetListReportPostResult>
    {
        private readonly fptforumQueryContext _context;

        public GetListReportPostHandler(fptforumQueryContext context)
        {
            _context = context;
        }

        public async Task<Result<GetListReportPostResult>> Handle(GetListReportPostQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            // Tạo truy vấn ban đầu
            var reportListQuery = _context.ReportPosts
                .AsNoTracking()
                .Include(x => x.ReportBy)
                .Where(x => x.Processing == true);

            // Áp dụng các điều kiện where tùy theo giá trị trong request
            if (request.UserPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.UserPostId == request.UserPostId);
            }
            else if (request.UserPostPhotoId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.UserPostPhotoId == request.UserPostPhotoId);
            }
            else if (request.UserPostVideoId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.UserPostVideoId == request.UserPostVideoId);
            }
            else if (request.GroupPostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.GroupPostId == request.GroupPostId);
            }
            else if (request.GroupPostVideoId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.GroupPostVideoId == request.GroupPostVideoId);
            }
            else if (request.GroupPostPhotoId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.GroupPostPhotoId == request.GroupPostPhotoId);
            }
            else if (request.SharePostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.SharePostId == request.SharePostId);
            }
            else if (request.GroupSharePostId != null)
            {
                reportListQuery = reportListQuery.Where(x => x.GroupSharePostId == request.GroupSharePostId);
            }

            // Thực hiện truy vấn và ánh xạ kết quả
            var reportList = await reportListQuery
                .Select(x => new GetReportPost
                {
                    ReportPostId = x.ReportPostId,
                    ReportTypeId = x.ReportTypeId,
                    UserPostId = x.UserPostId,
                    UserPostPhotoId = x.UserPostPhotoId,
                    UserPostVideoId = x.UserPostVideoId,
                    GroupPostId = x.GroupPostId,
                    GroupPostPhotoId = x.GroupPostPhotoId,
                    GroupPostVideoId = x.GroupPostVideoId,
                    SharePostId = x.SharePostId,
                    GroupSharePostId = x.GroupSharePostId,
                    UserId = x.ReportById,
                    UserName = x.ReportBy.FullName,
                    AvatarUrl = _context.AvataPhotos.Where(ap => ap.UserId == x.ReportById && ap.IsUsed == true).Select(ap => ap.AvataPhotosUrl).FirstOrDefault(),
                    CreatedDate = x.CreatedDate,
                })
                .ToListAsync(cancellationToken);

            // Tính toán phân trang
            var paginatedList = reportList
                .OrderByDescending(x => x.CreatedDate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = new GetListReportPostResult
            {
                result = paginatedList,
                totalPage = (int)Math.Ceiling((double)reportList.Count() / request.PageSize),
            };

            return Result<GetListReportPostResult>.Success(result);
        }
    }
}
