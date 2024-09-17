using Application.DTO.GroupFPTDTO;
using Application.DTO.UserPostVideoDTO;
using Application.Queries.GetImageByUserId;
using Application.Queries.GetVideoByUserId;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Query;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetImageByGroupId
{
    public class GetImageByGroupIdQueryHandler : IQueryHandler<GetImageByGroupIdQuery, GetImageByGroupIdQueryResult>
    {
        private readonly fptforumQueryContext _context;
        private readonly IMapper _mapper;

        public GetImageByGroupIdQueryHandler(fptforumQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<GetImageByGroupIdQueryResult>> Handle(GetImageByGroupIdQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var result = new GetImageByGroupIdQueryResult();

            string checkGroupStatus = (from g in _context.GroupFpts
                                       join gs in _context.GroupStatuses on g.GroupStatusId equals gs.GroupStatusId
                                       where g.GroupId == request.GroupId
                                       select gs.GroupStatusName
                                      ).FirstOrDefault();

            bool IsMember = _context.GroupMembers.Where(x => x.UserId ==  request.UserId && x.GroupId == request.GroupId).Any();

            if (checkGroupStatus == "Private" && IsMember != true) 
            {
                return Result<GetImageByGroupIdQueryResult>.Success(result);
            }

            var groupPhoto1 = _context.GroupPostPhotos.Where(x => x.GroupPost.GroupId == request.GroupId && x.IsHide != true && x.IsBanned != true)
                                                    .Select(x => new ImageInGroupFPT
                                                    {
                                                        GroupId = x.GroupPost.UserId,
                                                        UrlImage = x.GroupPhoto.PhotoUrl,
                                                        GroupPostId = x.GroupPost.GroupId,
                                                        GroupPostPhotoId = x.GroupPostPhotoId,
                                                        CreateDate = x.CreatedAt,
                                                    }).ToList();

            var groupPhoto2 = _context.GroupPosts.Where(x => x.GroupId == request.GroupId && x.IsHide != true && x.IsBanned != true && !string.IsNullOrEmpty(x.GroupPhotoId.ToString()))
                                                .Select(x => new ImageInGroupFPT
                                                {
                                                    GroupId = x.UserId,
                                                    UrlImage = x.GroupPhoto.PhotoUrl,
                                                    GroupPostId = x.GroupPostId,
                                                    GroupPostPhotoId = null,
                                                    CreateDate = x.CreatedAt,
                                                }).ToList();

            var allGroupPhotos = groupPhoto1.Concat(groupPhoto2)
                                    .OrderByDescending(x => x.CreateDate);

            // Apply paging
            var pageSize = 20;
            var page = request.Page;
            var pagedPhotos = allGroupPhotos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            result.imageInGroupFPTList = pagedPhotos;

            return Result<GetImageByGroupIdQueryResult>.Success(result);

        }
    }
}
