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

namespace Application.Queries.GetAllUserForAdmin
{
    public class GetAllUserForAdminHandler : IQueryHandler<GetAllUserForAdmin, GetAllUserForAdminResult>
    {
        private readonly fptforumQueryContext _context;

        public GetAllUserForAdminHandler(fptforumQueryContext context)
        {
            _context = context;
        }

        public async Task<Result<GetAllUserForAdminResult>> Handle(GetAllUserForAdmin request, CancellationToken cancellationToken)
        {
            if(_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            var userList = await _context.UserProfiles
                .AsNoTracking()
                .ToListAsync();

            var allUserList = new List<GetAllUser>();

            foreach(var profile in userList)
            {
                var user = new GetAllUser();
                user.UserId = profile.UserId;
                user.Email = profile.Email;
                user.Name = profile.FullName;
                var avt = await _context.AvataPhotos
                    .AsNoTracking()
                    .Where(x => x.UserId == profile.UserId && x.IsUsed == true)
                    .FirstOrDefaultAsync();
                
                if( avt != null )
                {
                    user.AvatarUrl = avt.AvataPhotosUrl;
                }

                if (profile.IsActive)
                {
                    user.IsActive = true;
                }
                else 
                {
                    user.IsActive = false;
                }
                user.CreatedAt = profile.CreatedAt;

                allUserList.Add(user);   
            }

            var result = new GetAllUserForAdminResult
            {
                result = allUserList.OrderByDescending(x => x.CreatedAt)
                                    .Skip((request.Page - 1) * request.PageSize)
                                    .Take(request.PageSize)
                                    .ToList(),
                totalPage = (int)Math.Ceiling((double)allUserList.Count() / request.PageSize),
            };

            return Result<GetAllUserForAdminResult>.Success(result);
        }
    }
}
