using Application.Commands.CreateNewReact;
using Application.Commands.CreateReactCommentGroupPost;
using Application.Commands.CreateReactCommentGroupPostPhoto;
using Application.Commands.CreateReactCommentGroupVideoPost;
using Application.Commands.CreateReactCommentUserPost;
using Application.Commands.CreateReactCommentUserPostPhoto;
using Application.Commands.CreateReactCommentUserPostVideo;
using Application.Commands.CreateReactForCommentGroupSharePost;
using Application.Commands.CreateReactForCommentSharePost;
using Application.Commands.CreateReactForGroupSharePost;
using Application.Commands.CreateReactForSharePost;
using Application.Commands.CreateReactGroupPhotoPost;
using Application.Commands.CreateReactGroupPost;
using Application.Commands.CreateReactGroupVideoPost;
using Application.Commands.CreateReactUserPhotoPost;
using Application.Commands.CreateReactUserPost;
using Application.Commands.CreateReactUserVideoPost;
using Application.Commands.Post;
using Application.Queries.GetAllReactType;
using Application.Queries.GetReactByCommentGroupPhotoId;
using Application.Queries.GetReactByCommentGroupPostId;
using Application.Queries.GetReactByCommentGroupSharePostId;
using Application.Queries.GetReactByCommentGroupVideoId;
using Application.Queries.GetReactByCommentId;
using Application.Queries.GetReactByCommentPhotoId;
using Application.Queries.GetReactByCommentSharePostId;
using Application.Queries.GetReactByCommentVideoId;
using Application.Queries.GetReactByGroupPhotoPost;
using Application.Queries.GetReactByGroupPost;
using Application.Queries.GetReactByGroupSharePostId;
using Application.Queries.GetReactByGroupVideoPost;
using Application.Queries.GetReactByPhotoPost;
using Application.Queries.GetReactByPost;
using Application.Queries.GetReactBySharePostId;
using Application.Queries.GetReactByVideoPost;
using Application.Queries.GetReactCommentDetail;
using Application.Queries.GetReactDetail;
using Application.Queries.GetUserStatus;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserReactController : BaseController
    {
        private readonly ISender _sender;

        public UserReactController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createNewReact")]
        public async Task<IActionResult> CreateNewReact(CreateNewReactCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactPost")]
        public async Task<IActionResult> CreateReactPost(CreateReactUserPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactPhotoPost")]
        public async Task<IActionResult> CreateReactPhotoPost(CreateReactUserPhotoPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactVideoPost")]
        public async Task<IActionResult> CreateReactVideoPost(CreateReactUserVideoPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactCommentUserPost")]
        public async Task<IActionResult> CreateReactCommentUserPost(CreateReactCommentUserPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactCommentUserVideoPost")]
        public async Task<IActionResult> CreateReactCommentUserVideoPost(CreateReactCommentUserPostVideoCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactCommentUserPhotoPost")]
        public async Task<IActionResult> CreateReactCommentUserPhotoPost(CreateReactCommentUserPostPhotoCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactType")]
        public async Task<IActionResult> GetAllReactType()
        {
            var input = new GetAllReactTypeQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByPostId")]
        public async Task<IActionResult> GetAllReactByPostId([FromQuery] GetReactByPostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByCommentId")]
        public async Task<IActionResult> GetAllReactByCommentId([FromQuery] GetReactByCommentIdQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByCommentVideoId")]
        public async Task<IActionResult> GetAllReactByCommentVideoId([FromQuery] GetReactByCommentVideoIdQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByCommentPhotoId")]
        public async Task<IActionResult> GetAllReactByCommentPhotoId([FromQuery] GetReactByCommentPhotoIdQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }
        //=================DocHere===============================
        [HttpGet]
        [Route("getAllReactByPhotoPostId")]
        public async Task<IActionResult> GetAllReactByPhotoPostId([FromQuery] GetReactByPhotoPostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByVideoPostId")]
        public async Task<IActionResult> GetAllReactByVideoPostId([FromQuery] GetReactByVideoPostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }
        //=================DocHere=============================== 

        [HttpPost]
        [Route("createReactGroupPost")]
        public async Task<IActionResult> CreateReactGroupPost(CreateReactGroupPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactGroupPhotoPost")]
        public async Task<IActionResult> CreateReactGroupPhotoPost(CreateReactGroupPhotoPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactGroupVideoPost")]
        public async Task<IActionResult> CreateReactVideoPost(CreateReactGroupVideoPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);

        }

        [HttpPost]
        [Route("createReactCommentGroupPost")]
        public async Task<IActionResult> CreateReactCommentGroupPost(CreateReactCommentGroupPostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactCommentGroupVideoPost")]
        public async Task<IActionResult> CreateReactCommentGroupVideoPost(CreateReactCommentGroupPostVideoCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactCommentGroupPhotoPost")]
        public async Task<IActionResult> CreateReactCommentUserPhotoPost(CreateReactCommentGroupPostPhotoCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByGroupCommentId")]
        public async Task<IActionResult> GetAllReactByGroupCommentId([FromQuery] GetReactByCommentGroupPostIdQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByGroupCommentVideoId")]
        public async Task<IActionResult> GetAllReactByGroupCommentVideoId([FromQuery] GetReactByCommentGroupVideoIdQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByGroupCommentPhotoId")]
        public async Task<IActionResult> GetAllReactByGroupCommentPhotoId([FromQuery] GetReactByCommentGroupPhotoIdQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByGroupPostId")]
        public async Task<IActionResult> GetAllReactByGroupPostId([FromQuery] GetReactByGroupPostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByGroupPhotoPostId")]
        public async Task<IActionResult> GetAllReactByGroupPhotoPostId([FromQuery] GetReactByGroupPhotoPostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByGroupVideoPostId")]
        public async Task<IActionResult> GetAllReactByGroupVideoPostId([FromQuery] GetReactByGroupVideoPostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactCommentSharePost")]
        public async Task<IActionResult> CreateReactSharePostComment(CreateReactForCommentSharePostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactSharePost")]
        public async Task<IActionResult> CreateReactSharePost(CreateReactForSharePostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactBySharePostId")]
        public async Task<IActionResult> GetAllReactBySharePostId([FromQuery] GetReactBySharePostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByCommentSharePostId")]
        public async Task<IActionResult> GetAllReactByCommentSharePostId([FromQuery] GetReactByCommentSharePostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactCommentGroupSharePost")]
        public async Task<IActionResult> CreateReactShareGroupPostComment(CreateReactForCommentGroupSharePostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReactGroupSharePost")]
        public async Task<IActionResult> CreateReactGroupSharePost(CreateReactForGroupSharePostCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllReactByCommentGroupSharePostId")]
        public async Task<IActionResult> GetAllReactByCommentGroupSharePostId([FromQuery] GetReactByCommentGroupSharePostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);

        }

        [HttpGet]
        [Route("getAllReactByGroupSharePostId")]
        public async Task<IActionResult> GetAllReactByGroupSharePostId([FromQuery] GetReactByGroupSharePostQuery query)
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
            query.UserId = Guid.Parse(uid);
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getReactPostDetail")]
        public async Task<IActionResult> GetReactPostDetail([FromQuery] GetReactDetailQuery query)
        {
            var res = await _sender.Send(query);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getReactCommentPostDetail")]
        public async Task<IActionResult> GetReactCommentPostDetail([FromQuery] GetReactCommentDetailQuery query)
        {
            var res = await _sender.Send(query);
            return Success(res.Value);
        }
    }
}
