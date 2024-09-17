using Application.Commands.GetUserProfile;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Enums;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserCommand
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserCommandResult>
    {
        private readonly fptforumCommandContext _context;
        private readonly fptforumQueryContext _querycontext;
        private readonly IMapper _mapper;
        private readonly GuidHelper _helper;

        public UpdateUserCommandHandler(fptforumCommandContext context, fptforumQueryContext querycontext, IMapper mapper)
        {
            _context = context;
            _querycontext = querycontext;
            _mapper = mapper;
            _helper = new GuidHelper();
        }
        public async Task<Result<UpdateUserCommandResult>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (_context == null)
            {
                throw new ErrorException(StatusCodeEnum.Context_Not_Found);
            }
            var getuser = await _querycontext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            var usergender = await _querycontext.UserGenders.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            var usercontact = await _querycontext.ContactInfos.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            var userrelationship = await _querycontext.UserRelationships.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            var userweb = await _querycontext.WebAffiliations.Where(x => x.UserId == request.UserId).ToListAsync();
            var userinteres = await _querycontext.UserInterests.Where(x => x.UserId == request.UserId).ToListAsync();
            var userwork = await _querycontext.WorkPlaces.Where(x => x.UserId == request.UserId).ToListAsync();
            var useravata = await _querycontext.AvataPhotos.Where(x => x.UserId == request.UserId).ToListAsync();
            var status = await _querycontext.UserStatuses.ToListAsync();
            var listsetting = await _querycontext.Settings.ToListAsync();
            if (getuser  == null)
            {
                throw new ErrorException(StatusCodeEnum.U01_Not_Found);
            }

            var userprofile = new Domain.CommandModels.UserProfile
            {
                UserId  = (Guid)request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDay = request.BirthDay,
                AboutMe = request.AboutMe,
                HomeTown= request.HomeTown,
                CoverImage = request.CoverImage,
                Email = getuser.Email,
                FeId = getuser.FeId,
                UserNumber = getuser.UserNumber,
                RoleId = getuser.RoleId,
                UserStatusId = getuser.UserStatusId,
                IsFirstTimeLogin = getuser.IsFirstTimeLogin,
                IsActive = getuser.IsActive,
                CreatedAt  = getuser.CreatedAt,
                UpdatedAt = DateTime.Now
            };

             _context.UserProfiles.Update(userprofile);
            if(request.UserGender != null)
            {
                var updategender = new Domain.CommandModels.UserGender
                {
                    UserGenderId = usergender.UserGenderId,
                    GenderId = (Guid)request.UserGender.GenderId,
                    UserId = usergender.UserId,
                    UserStatusId = request.UserGender.UserStatusId,
                    CreatedAt = usergender.CreatedAt,
                    UpdatedAt = DateTime.Now,
                };
                _context.UserGenders.Update(updategender);
            }
            if (request.ContactInfo != null)
            {
                var updatecontac = new Domain.CommandModels.ContactInfo
                {
                    ContactInfoId = usercontact.ContactInfoId,
                    SecondEmail = request.ContactInfo.SecondEmail,
                    PrimaryNumber = request.ContactInfo.PrimaryNumber,
                    SecondNumber = request.ContactInfo.SecondNumber,
                    UserId = userprofile.UserId,
                    UserStatusId = (Guid)request.ContactInfo.UserStatusId,
                    CreatedAt = usercontact.CreatedAt,
                    UpdatedAt = DateTime.Now,
            };
                _context.ContactInfos.Update(updatecontac);
            } 

           
            if(request.UserRelationship != null)
            {
                if(userrelationship != null)
                {
                    var updaterelationship = new Domain.CommandModels.UserRelationship
                    {
                        UserRelationshipId = userrelationship.UserRelationshipId,
                        RelationshipId = (Guid)request.UserRelationship.RelationshipId,
                        UserId = userprofile.UserId,
                        UserStatusId = (Guid)request.UserRelationship.UserStatusId,
                        CreatedAt = userrelationship.CreatedAt,
                        UpdatedAt = DateTime.Now,
                    };
                    _context.UserRelationships.Update(updaterelationship);
                }
                else
                {
                    var updaterelationship = new Domain.CommandModels.UserRelationship
                    {
                        UserRelationshipId = _helper.GenerateNewGuid(),
                        RelationshipId = (Guid)request.UserRelationship.RelationshipId,
                        UserId = userprofile.UserId,
                        UserStatusId = (Guid)request.UserRelationship.UserStatusId,
                        CreatedAt = DateTime.Now,
                    };
                    await _context.UserRelationships.AddAsync(updaterelationship);
                }
               
            }
            if (request.Avataphoto != null)
            {
                var avata = new Domain.CommandModels.AvataPhoto
                {

                    AvataPhotosId = _helper.GenerateNewGuid(),
                    AvataPhotosUrl = request.Avataphoto,
                    IsUsed = true,
                    UserId = userprofile.UserId,
                    UserStatusId = status.FirstOrDefault(x => x.StatusName == "Public").UserStatusId,
                    CreatedAt = userprofile.CreatedAt,
                    UpdatedAt = DateTime.Now
                };
                await _context.AvataPhotos.AddAsync(avata);
                
                foreach(var a in useravata)
                {
                    if(a.AvataPhotosId != avata.AvataPhotosId)
                    {
                        var updateavata = new Domain.CommandModels.AvataPhoto
                        {
                            AvataPhotosId = a.AvataPhotosId,
                            AvataPhotosUrl = a.AvataPhotosUrl,
                            UserId = a.UserId,
                            UserStatusId = a.UserStatusId,
                            IsUsed = false,
                            CreatedAt = a.CreatedAt,
                            UpdatedAt = DateTime.Now
                        };
                        _context.AvataPhotos.Update(updateavata);
                    }  
                }
            }

            if (request.UserInterests != null)
            {
                if(request.UserInterests.Count > 0)
                {
                    foreach (var us in request.UserInterests)
                    {
                        if(us.InterestId == null)
                        {
                            var addinteres = new Domain.CommandModels.UserInterest
                            {
                                UserInterestId = _helper.GenerateNewGuid(),
                                InterestId = (Guid)us.InterestId,
                                UserId = getuser.UserId,
                                UserStatusId = (Guid)us.UserStatusId,
                                CreatedAt = DateTime.Now
                            };
                            await _context.UserInterests.AddAsync(addinteres);
                        }
                        else
                        {
                            var existing = userinteres.FirstOrDefault(x => x.InterestId == us.InterestId);
                            if (existing == null)
                            {
                                var addinteres = new Domain.CommandModels.UserInterest
                                {
                                    UserInterestId = _helper.GenerateNewGuid(),
                                    InterestId = (Guid)us.InterestId,
                                    UserId = getuser.UserId,
                                    UserStatusId = (Guid)us.UserStatusId,
                                    CreatedAt = DateTime.Now
                                };
                                await _context.UserInterests.AddAsync(addinteres);
                            }
                            else
                            {
                                var updateinteres = new Domain.CommandModels.UserInterest
                                {
                                    UserInterestId = existing.UserInterestId,
                                    InterestId = existing.InterestId,
                                    UserId = existing.UserId,
                                    UserStatusId = (Guid)us.UserStatusId,
                                    CreatedAt = existing.CreatedAt,
                                    UpdatedAt = DateTime.Now,
                                };
                                _context.UserInterests.Update(updateinteres);
                            }
                        }
                        
                    }

                    var removeitem = userinteres.Where(x => !request.UserInterests.Any(y => y.InterestId == x.InterestId)).ToList();
                    foreach (var item in removeitem)
                    {
                        var interesrm = new Domain.CommandModels.UserInterest
                        {
                            UserInterestId = item.UserInterestId,
                            InterestId = item.InterestId,
                            UserId = item.UserId,
                            UserStatusId = item.UserStatusId,
                            CreatedAt = item.CreatedAt,
                            UpdatedAt = item.UpdatedAt
                        };
                        _context.UserInterests.Remove(interesrm);
                    }
                }

            }
            else
            {
                foreach(var item in userinteres)
                {
                    var interesrm = new Domain.CommandModels.UserInterest
                    {
                        UserInterestId = item.UserInterestId,
                        InterestId = item.InterestId,
                        UserId = item.UserId,
                        UserStatusId = item.UserStatusId,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    };
                    _context.UserInterests.Remove(interesrm);
                }
            }

            if (request.WorkPlaces  != null)
            {
                if(request.WorkPlaces.Count > 0)
                {
                    foreach (var us in request.WorkPlaces)
                    {
                        if(us.WorkPlaceId == null)
                        {
                            var addwork = new Domain.CommandModels.WorkPlace
                            {
                                WorkPlaceId = _helper.GenerateNewGuid(),
                                WorkPlaceName = us.WorkPlaceName,
                                UserId = getuser.UserId,
                                UserStatusId = (Guid)us.UserStatusId,
                                CreatedAt = DateTime.Now
                            };
                            await _context.WorkPlaces.AddAsync(addwork);
                        }
                        else
                        {
                            var existing = userwork.FirstOrDefault(x => x.WorkPlaceId == us.WorkPlaceId);
                            if (existing == null)
                            {

                            }
                            else
                            {
                                var updatework = new Domain.CommandModels.WorkPlace
                                {
                                    WorkPlaceId = existing.WorkPlaceId,
                                    WorkPlaceName = us.WorkPlaceName,
                                    UserId = getuser.UserId,
                                    UserStatusId = (Guid)us.UserStatusId,
                                    CreatedAt = existing.CreatedAt,
                                    UpdatedAt = DateTime.Now
                                };
                                _context.WorkPlaces.Update(updatework);
                            }
                        }         
                    }

                    var removeitem = userwork.Where(x => !request.WorkPlaces.Any(y => y.WorkPlaceId == x.WorkPlaceId)).ToList();
                    foreach (var item in removeitem)
                    {
                        var workrm = new Domain.CommandModels.WorkPlace
                        {
                            WorkPlaceId = item.WorkPlaceId,
                            WorkPlaceName = item.WorkPlaceName,
                            UserId = item.UserId,
                            UserStatusId = item.UserStatusId,
                            CreatedAt = item.CreatedAt,
                            UpdatedAt = item.UpdatedAt
                        };
                        _context.WorkPlaces.Remove(workrm);
                    }
                }

            }
            else
            {
                foreach(var item in userwork)
                {
                    var workrm = new Domain.CommandModels.WorkPlace
                    {
                        WorkPlaceId = item.WorkPlaceId,
                        WorkPlaceName = item.WorkPlaceName,
                        UserId = item.UserId,
                        UserStatusId = item.UserStatusId,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    };
                    _context.WorkPlaces.Remove(workrm);
                }
            }

            if (request.WebAffiliations != null)
            {
                if(request.WebAffiliations.Count > 0)
                {
                    foreach (var us in request.WebAffiliations)
                    {
                        if(us.WebAffiliationId == null)
                        {
                            var adduserweb = new Domain.CommandModels.WebAffiliation
                            {
                                WebAffiliationId = _helper.GenerateNewGuid(),
                                WebAffiliationUrl = us.WebAffiliationUrl,
                                UserId = getuser.UserId,
                                UserStatusId = (Guid)us.UserStatusId,
                                CreatedAt = DateTime.Now
                            };
                            await _context.WebAffiliations.AddAsync(adduserweb);
                        }
                        else
                        {
                            var existing = userweb.FirstOrDefault(x => x.WebAffiliationId == us.WebAffiliationId);
                            if (existing == null)
                            {
                                var adduserweb = new Domain.CommandModels.WebAffiliation
                                {
                                    WebAffiliationId = _helper.GenerateNewGuid(),
                                    WebAffiliationUrl = us.WebAffiliationUrl,
                                    UserId = getuser.UserId,
                                    UserStatusId = (Guid)us.UserStatusId,
                                    CreatedAt = DateTime.Now
                                };
                                await _context.WebAffiliations.AddAsync(adduserweb);
                            }
                            else
                            {
                                existing.WebAffiliationUrl = us.WebAffiliationUrl;
                                existing.UserStatusId = (Guid)us.UserStatusId;
                                existing.UpdatedAt = DateTime.Now;
                                var updateweb = new Domain.CommandModels.WebAffiliation
                                {
                                    WebAffiliationId = existing.WebAffiliationId,
                                    WebAffiliationUrl = us.WebAffiliationUrl,
                                    UserId = getuser.UserId,
                                    UserStatusId = (Guid)us.UserStatusId,
                                    CreatedAt = existing.CreatedAt,
                                    UpdatedAt = DateTime.Now
                                };
                                _context.WebAffiliations.Update(updateweb);
                            }
                        }  
                    }

                    var removeitem = userweb.Where(x => !request.WebAffiliations.Any(y => y.WebAffiliationId == x.WebAffiliationId)).ToList();
                    foreach (var item in removeitem)
                    {
                        var webrm = new Domain.CommandModels.WebAffiliation
                        {
                            WebAffiliationId = item.WebAffiliationId,
                            WebAffiliationUrl = item.WebAffiliationUrl,
                            UserId = item.UserId,
                            UserStatusId = item.UserStatusId,
                            CreatedAt = item.CreatedAt,
                            UpdatedAt = item.UpdatedAt
                        };
                        _context.WebAffiliations.Remove(webrm);
                    }
                }

            }
            else
            {
                foreach(var item in userweb)
                {
                    var webrm = new Domain.CommandModels.WebAffiliation
                    {
                        WebAffiliationId = item.WebAffiliationId,
                        WebAffiliationUrl = item.WebAffiliationUrl,
                        UserId = item.UserId,
                        UserStatusId = item.UserStatusId,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    };
                    _context.WebAffiliations.Remove(webrm);
                }
            }
            await _context.SaveChangesAsync();

            //mapping
            var result = _mapper.Map<UpdateUserCommandResult>(request);
            result.UpdatedAt = DateTime.Now;
            return Result<UpdateUserCommandResult>.Success(result);
        }
    }
}
