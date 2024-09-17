using Application.Commands.CreateGender;
using Application.Commands.CreateGroupRole;
using Application.Commands.CreateGroupSetting;
using Application.Commands.CreateGroupStatus;
using Application.Commands.CreateGroupTag;
using Application.Commands.CreateGroupType;
using Application.Queries.GetAllGroupRole;
using Application.Queries.GetAllGroupSetting;
using Application.Queries.GetAllGroupStatus;
using Application.Queries.GetAllGroupTag;
using Application.Queries.GetAllGroupType;
using Application.Queries.GetGender;
using Application.Queries.GetGroupStatusForCreate;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupEntityController : BaseController
    {
        private readonly ISender _sender;
        public GroupEntityController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("getgrouprole")]
        public async Task<IActionResult> GetGroupRole()
        {
            var input = new GetAllGroupRoleQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [Authorize(Roles = "Societe-admin")]
        [HttpPost]
        [Route("creategrouprole")]
        public async Task<IActionResult> CreateGroupRole(CreateGroupRoleCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getgrouptag")]
        public async Task<IActionResult> GetGroupTag()
        {
            var input = new GetAllGroupTagQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [Authorize(Roles = "Societe-admin")]
        [HttpPost]
        [Route("creategrouptag")]
        public async Task<IActionResult> CreateGroupTag(CreateGroupTagCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }


        [HttpGet]
        [Route("getgroupsetting")]
        public async Task<IActionResult> GetGroupSetting()
        {
            var input = new GetAllGroupSettingQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [Authorize(Roles = "Societe-admin")]
        [HttpPost]
        [Route("creategroupsetting")]
        public async Task<IActionResult> CreateGroupSetting(CreateGroupSettingCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }


        [HttpGet]
        [Route("getgrouptype")]
        public async Task<IActionResult> GetGroupTypes()
        {
            var input = new GetAllGroupTypeQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [Authorize(Roles = "Societe-admin")]
        [HttpPost]
        [Route("creategrouptype")]
        public async Task<IActionResult> CreateGroupType(CreateGroupTypeCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getgroupstatus")]
        public async Task<IActionResult> GetGroupStatus()
        {
            var input = new GetGroupStatusQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [Authorize(Roles = "Societe-admin")]
        [HttpPost]
        [Route("creategroupstatus")]
        public async Task<IActionResult> CreateGroupStatus(CreateGroupStatusCommand input)
        {
            var res = await _sender.Send(input);
            return Success(res.Value);
        }

        [HttpGet]
        [Route("getgroupstatusforcreate")]
        public async Task<IActionResult> GetGroupStatusForCreate()
        {
            var input = new GetGroupStatusForCreateQuery();
            var res = await _sender.Send(input);
            return Success(res.Value);
        }
    }
}
