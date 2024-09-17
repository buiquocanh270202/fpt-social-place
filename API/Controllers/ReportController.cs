using Application.Commands.CreateReportComment;
using Application.Commands.CreateReportPost;
using Application.Commands.CreateReportProfile;
using Application.Commands.CreateReportType;
using Application.Commands.CreateUserGender;
using Application.Commands.ProcessReportCommand;
using Application.Queries.GetListReportComment;
using Application.Queries.GetListReportGroup;
using Application.Queries.GetListReportPost;
using Application.Queries.GetListReportUser;
using Application.Queries.GetReportComment;
using Application.Queries.GetReportGroup;
using Application.Queries.GetReportPost;
using Application.Queries.GetReportType;
using Application.Queries.GetReportUser;
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
    public class ReportController : BaseController
    {
        private readonly ISender _sender;
        public ReportController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Roles = "Societe-admin")]
        [HttpPost]
        [Route("createreporttype")]
        public async Task<IActionResult> CreateReportType(CreateReportTypeCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getreporttype")]
        public async Task<IActionResult> GetReportType()
        {
            var input = new GetReportTypeQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReportComment")]
        public async Task<IActionResult> CreateReportComment(CreateReportCommentCommand command)
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
            command.ReportById = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReportPost")]
        public async Task<IActionResult> CreateReportPost(CreateReportPostCommand command)
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
            command.ReportById = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createReportProfile")]
        public async Task<IActionResult> CreateReportProfile(CreateReportProfileCommand command)
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
            command.ReportById = Guid.Parse(uid);
            var res = await _sender.Send(command);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getReportUser")]
        public async Task<IActionResult> GetReportUser([FromQuery] GetReportUserQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getListReportUser")]
        public async Task<IActionResult> GetListReportUser([FromQuery]GetListReportUserQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getReportGroup")]
        public async Task<IActionResult> GetReportGroup([FromQuery] GetReportGroupQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getListReportGroup")]
        public async Task<IActionResult> GetListReportGroup([FromQuery] GetListReportGroupQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getReportPost")]
        public async Task<IActionResult> GetReportPost([FromQuery] GetReportPostQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getListReportPost")]
        public async Task<IActionResult> GetListReportPost([FromQuery] GetListReportPostQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getReportComment")]
        public async Task<IActionResult> GetReportComment([FromQuery] GetReportCommentQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getListReportComment")]
        public async Task<IActionResult> GetListReportComment([FromQuery] GetListReportCommentQuery input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("processReport")]
        public async Task<IActionResult> ProcessReport([FromBody] ProcessReportCommand command)
        {
            var res = await _sender.Send(command);
            return Success(res.Value);
        }
    }
}
