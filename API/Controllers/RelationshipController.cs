using Application.Queries.GetGender;
using Application.Queries.GetRelationship;
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
    public class RelationshipController : BaseController
    {
        private readonly ISender _sender;
        public RelationshipController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("getrelationship")]
        public async Task<IActionResult> GetRelationship()
        {
            var input = new GetRelationshipQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
