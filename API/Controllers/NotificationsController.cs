using Application.Commands.UpdateNotificationStatus;
using Application.Queries.GetUserByUserId;
using Application.Queries.GetUserNotificationsList;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsController : BaseController
    {
        private readonly ISender _sender;
        public NotificationsController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet]
        [Route("getnotificationslistbyuserid")]
        public async Task<IActionResult> GetNotificationsListByUserid([FromQuery] GetUserNotificationsListQuery input)
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

        [HttpPut]
        [Route("UpdateNotificationStatusbynotificationid")]
        public async Task<IActionResult> UpdateNotificationStatusByNotificationId([FromQuery] UpdateNotificationStatusCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
