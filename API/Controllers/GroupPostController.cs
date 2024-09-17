using Application.Commands.AprroveGroupPost;
using Application.Commands.CreateCommentForGroupSharePost;
using Application.Commands.CreateGroupPost;
using Application.Commands.CreateReactCommentGroupPost;
using Application.Commands.CreateReactCommentGroupPostPhoto;
using Application.Commands.CreateReactCommentUserPost;
using Application.Commands.CreateReactCommentUserPostPhoto;
using Application.Commands.CreateReactCommentUserPostVideo;
using Application.Commands.CreateReactGroupPhotoPost;
using Application.Commands.CreateReactGroupPost;
using Application.Commands.CreateReactGroupVideoPost;
using Application.Commands.CreateReactUserPhotoPost;
using Application.Commands.CreateReactUserPost;
using Application.Commands.CreateReactUserVideoPost;
using Application.Commands.CreateUserCommentGroupPhotoPost;
using Application.Commands.CreateUserCommentGroupPost;
using Application.Commands.CreateUserCommentGroupVideoPost;
using Application.Commands.CreateUserCommentPhotoPost;
using Application.Commands.CreateUserCommentVideoPost;
using Application.Commands.DeleteCommentGroupPhotoPost;
using Application.Commands.DeleteCommentGroupPost;
using Application.Commands.DeleteCommentGroupSharePost;
using Application.Commands.DeleteCommentGroupVideoPost;
using Application.Commands.DeleteCommentUserPost;
using Application.Commands.DeleteGroupPhotoPost;
using Application.Commands.DeleteGroupPost;
using Application.Commands.DeleteGroupSharePost;
using Application.Commands.DeleteGroupVideoPost;
using Application.Commands.DeleteUserPhotoPost;
using Application.Commands.DeleteUserVideoPost;
using Application.Commands.Post;
using Application.Commands.ShareGroupPostCommand;
using Application.Commands.ShareUserPostCommand;
using Application.Commands.UpdateCommentGroupPhotoPost;
using Application.Commands.UpdateCommentGroupPost;
using Application.Commands.UpdateCommentGroupSharePost;
using Application.Commands.UpdateCommentGroupVideoPost;
using Application.Commands.UpdateCommentUserPost;
using Application.Commands.UpdateGroupPhotoPostCommand;
using Application.Commands.UpdateGroupPostCommand;
using Application.Commands.UpdateGroupSharePost;
using Application.Commands.UpdateGroupVideoPostCommand;
using Application.Queries.GetChildGroupPost;
using Application.Queries.GetCommentbyGroupPhotoPostId;
using Application.Queries.GetCommentByGroupPostId;
using Application.Queries.GetCommentByGroupSharePost;
using Application.Queries.GetCommentByGroupVideoPostId;
using Application.Queries.GetCommentByPostId;
using Application.Queries.GetGroupPostByGroupId;
using Application.Queries.GetGroupPostByGroupPostId;
using Application.Queries.GetGroupPostIdPendingByGroupId;
using Application.Queries.GetShareGroupPostById;
using Application.Queries.SearchGroupPost;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupPostController : BaseController
    {
        private readonly ISender _sender;

        public GroupPostController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createGroupPost")]
        public async Task<IActionResult> CreateGroupPost(CreateGroupPostCommand input)
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

        [HttpPost]
        [Route("commentGroupPost")]
        public async Task<IActionResult> CreateGroupPostComment(CreateUserCommentGroupPostCommand input)
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


        [HttpPost]
        [Route("commentGroupVideoPost")]
        public async Task<IActionResult> CreateGroupVideoComment(CreateUserCommentGroupVideoPostCommand input)
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

        [HttpPost]
        [Route("commentGroupPhotoPost")]
        public async Task<IActionResult> CreateGroupPhotoComment(CreateUserCommentGroupPhotoPostCommand input)
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
        [Route("getGroupPostComment")]
        public async Task<IActionResult> GetGroupPostComment([FromQuery] GetCommentByGroupPostIdQuery command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getGroupVideoPostComment")]
        public async Task<IActionResult> GetGroupVideoPostComment([FromQuery] GetCommentByGroupVideoPostIdQuery command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpGet]
        [Route("getGroupPhotoPostComment")]
        public async Task<IActionResult> GetGroupPhotoPostComment([FromQuery] GetCommentByGroupPhotoPostIdQuery command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpGet]
        [Route("getGroupPostByGroupId")]
        public async Task<IActionResult> GetGroupPostByGroupId([FromQuery] GetGroupPostByGroupIdQuery input)
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
        [Route("getGroupPostByGroupPostId")]
        public async Task<IActionResult> GetGroupPostByGroupPostId([FromQuery] GetGroupPostByGroupPostIdQuery input)
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
        [Route("getChildGroupPost")]
        public async Task<IActionResult> GetChildGroupPost([FromQuery] GetChildGroupPostQuery input)
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

        [HttpPost]
        [Route("shareGroupPost")]
        public async Task<IActionResult> ShareGroupPost(ShareGroupPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("updateGroupPost")]
        public async Task<IActionResult> UpdateGroupPost(UpdateGroupPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("updateGroupPhotoPost")]
        public async Task<IActionResult> UpdateGroupPhotoPost(UpdateGroupPhotoPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("updateGroupVideoPost")]
        public async Task<IActionResult> UpdateGroupVideoPost(UpdateGroupVideoPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpDelete]
        [Route("deleteCommentGroupPost")]
        public async Task<IActionResult> DeleteCommentGroupPost([FromQuery] DeleteCommentGroupPostCommand input)
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
        [Route("deleteCommentGroupPhotoPost")]
        public async Task<IActionResult> DeleteCommentGroupPhotoPost([FromQuery] DeleteCommentGroupPhotoPostCommand input)
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
            if(string.IsNullOrEmpty(uid))
            {
                return BadRequest();
            }
            input.UserId = Guid.Parse(uid);
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpDelete]
        [Route("deleteCommentGroupVideoPost")]
        public async Task<IActionResult> DeleteCommentGroupVideoPost([FromQuery] DeleteCommentGroupVideoPostCommand input)
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
        [Route("deleteGroupPost")]
        public async Task<IActionResult> DeleteGroupPost([FromQuery] DeleteGroupPostCommand input)
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
        [Route("getGroupPostIdPendingByGroupId")]
        public async Task<IActionResult> GetGroupPostIdPendingByGroupId([FromQuery] GetGroupPostIdPendingByGroupIdQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("commentGroupSharePost")]
        public async Task<IActionResult> CommentGroupSharePost(CreateCommentForGroupSharePostCommand input)
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
        [Route("getGroupSharePostComment")]
        public async Task<IActionResult> getGroupSharePostComment([FromQuery] GetCommentByGroupSharePostQuery command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("updateGroupSharePost")]
        public async Task<IActionResult> UpdateGroupSharePost(UpdateGroupSharePostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("updateCommentGroupSharePost")]
        public async Task<IActionResult> UpdateCommentGroupSharePost(UpdateCommentGroupSharePostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpDelete]
        [Route("deleteGroupSharePost")]
        public async Task<IActionResult> DeleteGroupSharePost([FromQuery] DeleteGroupSharePostCommand input)
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
        [Route("deleteCommentGroupSharePost")]
        public async Task<IActionResult> DeleteCommentGroupSharePost([FromQuery] DeleteCommentGroupSharePostCommand input)
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

        [HttpPost]
        [Route("updateGroupCommentPost")]
        public async Task<IActionResult> UpdateGroupCommentPost(UpdateCommentGroupPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("updateGroupCommentVideoPost")]
        public async Task<IActionResult> UpdateGroupCommentVideoPost(UpdateCommentGroupVideoPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("updateGroupCommentPhotoPost")]
        public async Task<IActionResult> UpdateGroupCommentPhotoPost(UpdateCommentGroupPhotoPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("approveGroupPost")]
        public async Task<IActionResult> ApproveGroupPost(ApproveGroupPostCommand command)
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
            command.UserId = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpGet]
        [Route("getGroupSharePostByGroupPostId")]
        public async Task<IActionResult> GetGroupSharePostByGroupPostId([FromQuery] GetShareGroupPostByIdQuery input)
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
        [Route("deleteGroupVideoPost")]
        public async Task<IActionResult> DeleteGroupVideoPost([FromQuery] DeleteGroupVideoPostCommand input)
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
        [Route("deleteGroupPhotoPost")]
        public async Task<IActionResult> DeleteGroupPhotoPost([FromQuery] DeleteGroupPhotoPostCommand input)
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
    }
}
