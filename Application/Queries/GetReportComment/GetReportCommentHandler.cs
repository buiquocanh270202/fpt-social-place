using Application.Queries.GetReportPost;
using Core.CQRS;
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

namespace Application.Queries.GetReportComment
{
    public class GetReportCommentHandler : IQueryHandler<GetReportCommentQuery, GetReportCommentResult>
    {
        private readonly fptforumQueryContext _context;

        public GetReportCommentHandler(fptforumQueryContext context)
        {
            _context = context;
        }

        public async Task<Result<GetReportCommentResult>> Handle(GetReportCommentQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if (request == null)
            {
                throw new ErrorException(StatusCodeEnum.RQ01_Request_Is_Null);
            }

            var reports = await _context.ReportComments
                    .Include(x => x.ReportBy)
                    .Where(x => x.Processing == true)
                    .ToListAsync(cancellationToken);

            // Nhóm dữ liệu dựa trên trường có giá trị
            var reportGroups = reports
                .GroupBy(x => new
                {
                    // Xác định trường không null để nhóm
                    NonNullField = x.CommentId ?? x.CommentPhotoPostId ?? x.CommentVideoPostId ?? x.CommentGroupPostId ?? x.CommentGroupVideoPostId ?? x.CommentPhotoGroupPostId ?? x.CommentSharePostId ?? x.CommentGroupSharePostId
                })
                .Select(g => new
                {
                    GroupKey = g.Key.NonNullField,
                    NumberOfReports = g.Count(),
                    FieldName = g.First().CommentId != null ? "CommentId" :
                        g.First().CommentPhotoPostId != null ? "CommentPhotoPostId" :
                        g.First().CommentVideoPostId != null ? "CommentVideoPostId" :
                        g.First().CommentGroupPostId != null ? "CommentGroupPostId" :
                        g.First().CommentGroupVideoPostId != null ? "CommentGroupVideoPostId" :
                        g.First().CommentPhotoGroupPostId != null ? "CommentPhotoGroupPostId" :
                        g.First().CommentSharePostId != null ? "CommentSharePostId" :
                        g.First().CommentGroupSharePostId != null ? "CommentGroupSharePostId" : string.Empty,
                    Content = g.First().Content,
                })
                .Where(g => g.GroupKey != null)
                .ToList();

            // Tạo danh sách kết quả với ánh xạ trường phù hợp
            var resultList = reportGroups
                .Select(g => new GetReportComment
                {
                    CommentId = g.FieldName == "CommentId" ? g.GroupKey : null,
                    CommentPhotoPostId = g.FieldName == "CommentPhotoPostId" ? g.GroupKey : null,
                    CommentVideoPostId = g.FieldName == "CommentVideoPostId" ? g.GroupKey : null,
                    CommentGroupPostId = g.FieldName == "CommentGroupPostId" ? g.GroupKey : null,
                    CommentGroupVideoPostId = g.FieldName == "CommentGroupVideoPostId" ? g.GroupKey : null,
                    CommentPhotoGroupPostId = g.FieldName == "CommentPhotoGroupPostId" ? g.GroupKey : null,
                    CommentSharePostId = g.FieldName == "CommentSharePostId" ? g.GroupKey : null,
                    CommentGroupSharePostId = g.FieldName == "CommentGroupSharePostId" ? g.GroupKey : null,
                    Content = g.Content,
                    NumberReporter = g.NumberOfReports
                })
                .ToList();

            var result = new GetReportCommentResult
            {
                result = resultList.OrderByDescending(x => x.NumberReporter)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)resultList.Count() / request.PageSize),
            };

            return Result<GetReportCommentResult>.Success(result);
        }
    }
}
