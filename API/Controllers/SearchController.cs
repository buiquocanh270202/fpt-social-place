using Application.Queries.FindUserByName;
using Application.Queries.GetFriendyName;
using Application.Queries.GetGroupByGroupId;
using Application.Queries.GetGroupByName;
using Application.Queries.SearchFunction;
using Application.Queries.SearchGroupPost;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SearchController : BaseController
    {
        private readonly ISender _sender;
        public SearchController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("searchfriendbyname")]
        public async Task<IActionResult> GetGroupByGroupId([FromQuery] GetFriendByNameQuery input)
        {
            var rawToken = HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(rawToken))
            {
                return BadRequest();
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                return BadRequest();
            }
            var uid = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            if (string.IsNullOrEmpty(uid))
            {
                return BadRequest();
            }
            input.AccessUserId = Guid.Parse(uid);
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("searchAll")]
        public async Task<IActionResult> GetInforByContent([FromQuery] SearchFunctionQuery input)
        {
            var rawToken = HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(rawToken))
            {
                return BadRequest();
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                return BadRequest();
            }
            var uid = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            if (string.IsNullOrEmpty(uid))
            {
                return BadRequest();
            }
            input.UserId = Guid.Parse(uid);
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("searchuserbyname")]
        public async Task<IActionResult> SearchUserByName([FromQuery] FindUserByNameQuery input)
        {
            var rawToken = HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(rawToken))
            {
                return BadRequest();
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                return BadRequest();
            }
            var uid = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            if (string.IsNullOrEmpty(uid))
            {
                return BadRequest();
            }
            input.UserId = Guid.Parse(uid);
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("searchgroupbygroupname")]
        public async Task<IActionResult> SearchGroupByGroupName ([FromQuery] GetGroupByNameQuery input)
        {
            var rawToken = HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(rawToken))
            {
                return BadRequest();
            }
            var handle = new JwtSecurityTokenHandler();
            var jsontoken = handle.ReadToken(rawToken) as JwtSecurityToken;
            if (jsontoken == null)
            {
                return BadRequest();
            }
            var uid = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value;
            if (string.IsNullOrEmpty(uid))
            {
                return BadRequest();
            }
            input.UserId = Guid.Parse(uid);
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("searchGroupPost")]
        public async Task<IActionResult> SearchGroupPost([FromQuery] SearchGroupPostQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
