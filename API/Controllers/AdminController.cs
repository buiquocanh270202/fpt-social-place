using Application.Commands.CreateAdminProfile;
using Application.Commands.GetUserProfile;
using Application.Queries.GetAllGroupForAdmin;
using Application.Queries.GetAllUserForAdmin;
using Application.Queries.GetDataForAdmin;
using Application.Queries.GetNumberUserByDayForAdmin;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Societe-admin")]
    public class AdminController : BaseController
    {
        private readonly ISender _sender;
        public AdminController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("createadminprofile")]
        public async Task<IActionResult> CreateAdmin(CreateAdminProfileCommand input)
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
            input.AdminId = Guid.Parse(uid);
            input.RoleName = jsontoken.Claims.FirstOrDefault(claim => claim.Type == "role").Value;
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getDataForAdmin")]
        public async Task<IActionResult> GetDataForAdmin()
        {
            var input = new GetDataForAdmin();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllUserForAdmin")]
        public async Task<IActionResult> GetAllUserForAdmin([FromQuery] GetAllUserForAdmin input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getAllGroupForAdmin")]
        public async Task<IActionResult> GetAllGroupForAdmin([FromQuery] GetAllGroupForAdmin input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getNumberUserByDayForAdmin")]
        public async Task<IActionResult> GetNumberUserByDayForAdmin([FromQuery] GetNumberUserByDayForAdmin input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
