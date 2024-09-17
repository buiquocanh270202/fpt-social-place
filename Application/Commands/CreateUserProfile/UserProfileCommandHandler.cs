using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Domain.Exceptions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.DTO.CreateUserDTO;
using Application.Commands.GetUserProfile;
using Core.Helper;
using Domain.CommandModels;
using Domain.QueryModels;

namespace Application.Commands.CreateUserProfile
{
    public class UserProfileCommandHandler : ICommandHandler<UserProfileCommand, UserProfileCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public UserProfileCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }

        public async Task<Result<UserProfileCommandResult>> Handle(UserProfileCommand request, CancellationToken cancellationToken)
        {
            if(_context == null || _querycontext == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }

            if(request.Email == null && request.FeId == null)
            {
                throw new ErrorException(StatusCodeEnum.U04_Can_Not_Create);
            }

            var getuserbyemail = await _querycontext.UserProfiles.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
            var getuserbyfeid = await _querycontext.UserProfiles.FirstOrDefaultAsync(X => X.FeId.Equals(request.FeId));
            if (getuserbyemail != null || getuserbyfeid != null)
            {
                throw new ErrorException(StatusCodeEnum.U03_User_Exist);
            }

            var role = await _querycontext.Roles.FirstOrDefaultAsync(x=>x.NameRole == request.RoleName);
            var status = await _querycontext.UserStatuses.FirstOrDefaultAsync(x => x.StatusName == "Public");
            var listsetting = await _querycontext.Settings.ToListAsync();
            var userprofile = _mapper.Map<Domain.CommandModels.UserProfile>(request);
            userprofile.UserId = (Guid)request.UserId;
            userprofile.RoleId = role.RoleId;
            userprofile.IsActive = true;
            userprofile.UserStatusId = status.UserStatusId;
            userprofile.IsFirstTimeLogin = false;
            userprofile.CreatedAt = DateTime.Now;
            await _context.UserProfiles.AddAsync(userprofile);
            var usergender = new Domain.CommandModels.UserGender {
                UserGenderId = _helper.GenerateNewGuid(),
                GenderId = request.Gender.GenderId,
                UserId = userprofile.UserId,
                UserStatusId = status.UserStatusId,
                CreatedAt = DateTime.Now
            };
            var contactinfor = new Domain.CommandModels.ContactInfo
            {
                ContactInfoId = _helper.GenerateNewGuid(),
                SecondEmail = request.ContactInfor.SecondEmail,
                PrimaryNumber = request.ContactInfor.PrimaryNumber,
                SecondNumber = request.ContactInfor.SecondNumber,
                UserId = userprofile.UserId,
                UserStatusId = status.UserStatusId,
                CreatedAt = DateTime.Now
            };
            if (request.Relationship?.RelationshipId != null)
            {
                var userrelationship = new Domain.CommandModels.UserRelationship
                {
                    UserRelationshipId = _helper.GenerateNewGuid(),
                    RelationshipId = (Guid)request.Relationship.RelationshipId,
                    UserId = userprofile.UserId,
                    UserStatusId = status.UserStatusId,
                    CreatedAt = DateTime.Now
                };
                await _context.UserRelationships.AddAsync(userrelationship);
            }
            
            if(request.Avataphoto != null)
            {
                var avata = new Domain.CommandModels.AvataPhoto
                {

                    AvataPhotosId = _helper.GenerateNewGuid(),
                    AvataPhotosUrl = request.Avataphoto,
                    IsUsed = true,
                    UserId = userprofile.UserId,
                    UserStatusId = status.UserStatusId,
                    CreatedAt = DateTime.Now
                };
                await _context.AvataPhotos.AddAsync(avata);
            }
           
                foreach (var us in listsetting)
                {
                    var usersetting = new Domain.CommandModels.UserSetting
                    {
                        UserSettingId = _helper.GenerateNewGuid(),
                        SettingId = us.SettingId,
                        UserId = userprofile.UserId,
                        UserStatusId = status.UserStatusId,
                    };
                    await _context.UserSettings.AddAsync(usersetting);
                }
           
            if(request.Interes?.Count > 0)
            {
                foreach (var us in request.Interes)
                {
                    if(us.InterestId != null)
                    {
                        var userinteres = new Domain.CommandModels.UserInterest
                        {
                            UserInterestId = _helper.GenerateNewGuid(),
                            InterestId = (Guid)us.InterestId,
                            UserId = userprofile.UserId,
                            UserStatusId = status.UserStatusId,
                            CreatedAt = DateTime.Now
                        };
                        await _context.UserInterests.AddAsync(userinteres);
                    }
                }
            }
            
            if(request.WorkPlace?.Count >0)
            {
                foreach (var us in request.WorkPlace)
                {
                    if(us.WorkPlaceName != null)
                    {
                        var userworkplace = new Domain.CommandModels.WorkPlace
                        {
                            WorkPlaceId = _helper.GenerateNewGuid(),
                            WorkPlaceName = us.WorkPlaceName,
                            UserId = userprofile.UserId,
                            UserStatusId = status.UserStatusId,
                            CreatedAt = DateTime.Now
                        };
                        await _context.WorkPlaces.AddAsync(userworkplace);
                    }
                }
            }

            if(request.WebAffilication?.Count > 0)
            {
                foreach (var us in request.WebAffilication)
                {
                    if(us.WebAffiliationUrl != null)
                    {
                        var userinteres = new Domain.CommandModels.WebAffiliation
                        {
                            WebAffiliationId = _helper.GenerateNewGuid(),
                            WebAffiliationUrl = us.WebAffiliationUrl,
                            UserId = userprofile.UserId,
                            UserStatusId = status.UserStatusId,
                            CreatedAt = DateTime.Now
                        };
                        await _context.WebAffiliations.AddAsync(userinteres);
                    }
                }
            }
           
            await _context.UserGenders.AddAsync(usergender);     
            await _context.ContactInfos.AddAsync(contactinfor);   
            await _context.SaveChangesAsync();

            //mapping
            var result = _mapper.Map<UserProfileCommandResult>(request);
            result.UserId = userprofile.UserId;
            result.RoleId = userprofile.RoleId;
            result.IsFirstTimeLogin = userprofile.IsFirstTimeLogin;
            result.UserStatusId = userprofile.UserStatusId;
            result.IsActive = userprofile.IsActive;
            result.CreatedAt = userprofile.CreatedAt;
            result.UpdatedAt = userprofile.UpdatedAt;
            return Result<UserProfileCommandResult>.Success(result); 
        }
    }
}
