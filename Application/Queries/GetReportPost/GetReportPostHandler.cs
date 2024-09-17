using Application.Queries.GetReportGroup;
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

namespace Application.Queries.GetReportPost
{
    public class GetReportPostHandler : IQueryHandler<GetReportPostQuery, GetReportPostResult>
    {
        private readonly fptforumQueryContext _context;

        public GetReportPostHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<GetReportPostResult>> Handle(GetReportPostQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reports = await _context.ReportPosts
                    .Include(x => x.ReportBy)
                    .Where(x => x.Processing == true)
                    .ToListAsync(cancellationToken);

            // Nhóm dữ liệu dựa trên trường có giá trị
            var reportGroups = reports
                .GroupBy(x => new
                {
                    // Xác định trường không null để nhóm
                    NonNullField = x.UserPostId ?? x.UserPostPhotoId ?? x.UserPostVideoId ?? x.GroupPostId ?? x.GroupPostVideoId ?? x.GroupPostPhotoId ?? x.SharePostId ?? x.GroupSharePostId
                })
                .Select(g => new
                {
                    GroupKey = g.Key.NonNullField,
                    NumberOfReports = g.Count(),
                    FieldName = g.First().UserPostId != null ? "UserPostId" :
                        g.First().UserPostPhotoId != null ? "UserPostPhotoId" :
                        g.First().UserPostVideoId != null ? "UserPostVideoId" :
                        g.First().GroupPostId != null ? "GroupPostId" :
                        g.First().GroupPostVideoId != null ? "GroupPostVideoId" :
                        g.First().GroupPostPhotoId != null ? "GroupPostPhotoId" :
                        g.First().SharePostId != null ? "SharePostId" :
                        g.First().GroupSharePostId != null ? "GroupSharePostId" : string.Empty
                })
                .Where(g => g.GroupKey != null)
                .ToList();

            // Tạo danh sách kết quả với ánh xạ trường phù hợp
            var resultList = reportGroups
                .Select(g => new GetReportPost
                {
                    UserPostId = g.FieldName == "UserPostId" ? g.GroupKey : null,
                    UserPostPhotoId = g.FieldName == "UserPostPhotoId" ? g.GroupKey : null,
                    UserPostVideoId = g.FieldName == "UserPostVideoId" ? g.GroupKey : null,
                    GroupPostId = g.FieldName == "GroupPostId" ? g.GroupKey : null,
                    GroupPostVideoId = g.FieldName == "GroupPostVideoId" ? g.GroupKey : null,
                    GroupPostPhotoId = g.FieldName == "GroupPostPhotoId" ? g.GroupKey : null,
                    SharePostId = g.FieldName == "SharePostId" ? g.GroupKey : null,
                    GroupSharePostId = g.FieldName == "GroupSharePostId" ? g.GroupKey : null,
                    NumberReporter = g.NumberOfReports
                })
                .ToList();

            var result = new GetReportPostResult
            {
                result = resultList.OrderByDescending(x => x.NumberReporter)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)resultList.Count() / request.PageSize),
            };

            return Result<GetReportPostResult>.Success(result);
        }
    }
}
