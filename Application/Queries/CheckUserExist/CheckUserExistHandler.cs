using Core.CQRS;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.QueryModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckUserExist
{

    public class CheckUserExistHandler : IQueryHandler<CheckUserExistQuery, CheckUserExistQueryResult>
    {
        private readonly fptforumQueryContext _context;
        public CheckUserExistHandler(fptforumQueryContext context)
        {
            _context = context;
        }
        public async Task<Result<CheckUserExistQueryResult>> Handle(CheckUserExistQuery request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var result = new CheckUserExistQueryResult();

            Domain.QueryModels.UserProfile user = new Domain.QueryModels.UserProfile();
            var admin = new Domain.QueryModels.AdminProfile();
            if(request.RoleName.Equals("Societe-admin"))
            {
                if (request.UserId != null)
                {
                    admin = await _context.AdminProfiles.FirstOrDefaultAsync(x => x.AdminId == request.UserId);
                }
                if (admin == null)
                {
                    result.enumcode = StatusCodeEnum.U01_Not_Found;
                    result.Message = StatusCodeEnum.U01_Not_Found.GetDescription();
                    result.IsAdmin = true;
                    return Result<CheckUserExistQueryResult>.Success(result);
                }
                if (admin.IsActive == false)
                {
                    throw new ErrorException(StatusCodeEnum.U06_User_Not_Active);
                }
                result.enumcode = StatusCodeEnum.U03_User_Exist;
                result.Message = StatusCodeEnum.U03_User_Exist.GetDescription();
                result.UserId = user.UserId;
                result.IsAdmin = true;
            }
            else
            {
                if (request.UserId != null)
                {
                    user = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId);
                }

                if (user == null)
                {
                    result.enumcode = StatusCodeEnum.U01_Not_Found;
                    result.Message = StatusCodeEnum.U01_Not_Found.GetDescription();
                    result.IsAdmin = false;
                    return Result<CheckUserExistQueryResult>.Success(result);
                }
                if (user.IsActive == false)
                {
                    throw new ErrorException(StatusCodeEnum.U06_User_Not_Active);
                }
                result.enumcode = StatusCodeEnum.U03_User_Exist;
                result.Message = StatusCodeEnum.U03_User_Exist.GetDescription();
                result.UserId = user.UserId;
                result.UserNumber = user.UserNumber;
                result.Email = user.Email;
                result.FeId = user.FeId;
                result.IsAdmin = false;
            }
         
            return Result<CheckUserExistQueryResult>.Success(result);
        }
    }
}
