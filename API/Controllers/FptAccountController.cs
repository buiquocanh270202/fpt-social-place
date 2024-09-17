using Application.Commands.CreateAccount;
using Application.Commands.ResetPassword;
using Application.Commands.UpdateUserChat;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FptAccountController : BaseController
    {
        private readonly ISender _sender;
        public FptAccountController(ISender sender)
        {
            _sender = sender;
        } 

        [HttpPost]
        [Route("createaccount")]
        public async Task<IActionResult> UpdateUserChatBox(CreateAccountCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassWord(ResetPasswordCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
