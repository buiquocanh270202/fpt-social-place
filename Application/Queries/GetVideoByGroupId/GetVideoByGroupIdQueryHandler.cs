using Application.DTO.GroupFPTDTO;
using Application.Queries.GetImageByGroupId;
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

namespace Application.Queries.GetVideoByGroupId
{
    public class GetVideoByGroupIdQueryHandler : IQueryHandler<GetVideoByGroupIdQuery, GetVideoByGroupIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetVideoByGroupIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetVideoByGroupIdQueryResult>> Handle(GetVideoByGroupIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new GetVideoByGroupIdQueryResult();

            var groupVideo1 = _context.GroupPostVideos.Where(x => x.GroupPost.GroupId == request.GroupId && x.IsHide != true && x.IsBanned != true)
                                                    .Select(x => new VideoInGroupFPT
                                                    {
                                                        GroupId = x.GroupPost.UserId,
                                                        UrlVideo = x.GroupVideo.VideoUrl,
                                                        GroupPostId = x.GroupPost.GroupId,
                                                        GroupPostVideoId = x.GroupPostVideoId,
                                                        CreateDate = x.CreatedAt,
                                                    }).ToList();

            var groupVideo2 = _context.GroupPosts.Where(x => x.GroupId == request.GroupId && x.IsHide != true && x.IsBanned != true && !string.IsNullOrEmpty(x.GroupVideoId.ToString()))
                                                .Select(x => new VideoInGroupFPT
                                                {
                                                    GroupId = x.UserId,
                                                    UrlVideo = x.GroupVideo.VideoUrl,
                                                    GroupPostId = x.GroupPostId,
                                                    GroupPostVideoId = null,
                                                    CreateDate = x.CreatedAt,
                                                }).ToList();

            var allGroupVideos = groupVideo1.Concat(groupVideo2)
                                    .OrderByDescending(x => x.CreateDate);

            // Apply paging
            var pageSize = 20;
            var page = request.Page;
            var pagedVideos = allGroupVideos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            result.videoInGroupFPTList = pagedVideos;

            return Result<GetVideoByGroupIdQueryResult>.Success(result);

        }
    }
}
