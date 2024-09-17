using Application.Commands.GetUserProfile;
using Application.DTO.GroupFPTDTO;
using Application.DTO.UserPostDTO;
using Application.Queries.GetPost;
using Application.Queries.UserProfile;
using Core.CQRS.Query;
using Domain.CommandModels;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.SearchFunction
{
    public class SearchFunctionQueryResult
    {
        public List<GroupFPTDTO>? groups { get; set; }
        public List<GetPostDTO>? userPosts { get; set; }
        public List<UserDTO>? userProfiles { get; set; }
        public int? totalPage { get; set; }

    }

    public class UserDTO
    {
        public string? UserName { get; set; }
        public Guid? UserId { get; set; }
        public string? AvataUrl { get; set; }
        public bool? isFriended { get; set; }
    }
}
