using Application.Commands.CreateChatBox;
using Application.Commands.CreateUserChat;
using Application.Commands.CreateUserInterest;
using Application.Commands.DeleteChat;
using Application.Commands.UpdateChat;
using Application.Commands.UpdateUserChat;
using Application.Queries.GetChatDetails;
using Application.Queries.GetUserByUserId;
using Application.Queries.GetUserOnChat;
using Application.Queries.SearchUserInChat;
using Application.Services;
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
    public class ChatController : BaseController
    {

        private readonly ISender _sender;
        public ChatController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createchatbox")]
        public async Task<IActionResult> CreateUserChat(CreateChatBoxCommand userchat)
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
            userchat.UserId = Guid.Parse(uid);
            var res = await _sender.Send(userchat);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createuserchat")]
        public async Task<IActionResult> CreateUserChatBox(CreateUserChatCommand userchat)
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
            userchat.UserId = Guid.Parse(uid);
            var res = await _sender.Send(userchat);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("updateuserchat")]
        public async Task<IActionResult> UpdateUserChatBox(UpdateUserChatCommand userchat)
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
            userchat.UserId = Guid.Parse(uid);
            var res = await _sender.Send(userchat);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getchatdetailbyid")]
        public async Task<IActionResult> GetChatDetailsById([FromQuery] GetChatDetailsQuery input)
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
        [Route("searchinchat")]
        public async Task<IActionResult> GetUserInChat([FromQuery] GetUserOnChatQuery input)
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
        [Route("searchuserforchat")]
        public async Task<IActionResult> SearchUserForChat([FromQuery] SearchUserInChatQuery input)
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


        [HttpDelete]
        [Route("deletechat")]
        public async Task<IActionResult> DeleteChatBox(DeleteChatCommand userchat)
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
            userchat.UserId = Guid.Parse(uid);
            var res = await _sender.Send(userchat);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("updatechat")]
        public async Task<IActionResult> UpdateChatBox(UpdateChatCommand userchat)
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
            userchat.UserId = Guid.Parse(uid);
            var res = await _sender.Send(userchat);
            return Success(res.Value);
        }
    }
}
