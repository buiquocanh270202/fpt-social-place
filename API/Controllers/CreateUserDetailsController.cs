using Application.Commands.CreateUserGender;
using Application.Commands.CreateUserInterest;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Societe-admin")]
    public class CreateUserDetailsController : BaseController
    {
        private readonly ISender _sender;
        public CreateUserDetailsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("postuserinterest")]
        public async Task<IActionResult> CreateUserInterest(UserInterestCommand userInterest)
        {
            var res = await _sender.Send(userInterest);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("createusergender")]
        public async Task<IActionResult> CreateUserGender(CreateUserGenderCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
