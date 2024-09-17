using Application.Commands.CreateAccount;
using Application.Services;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.Exceptions;
using Domain.QueryModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordCommandResult>
    {

        private readonly FptAccountServices _fptAccountServices;
        private readonly EmailServices _emailServices;
        private readonly BodyEmailHelper _bodyEmailHelper;
        private readonly fptforumQueryContext _context;

        public ResetPasswordCommandHandler(FptAccountServices fptAccountServices
            , EmailServices emailServices,
            fptforumQueryContext context)
        {
            _fptAccountServices = fptAccountServices;
            _emailServices = emailServices;
            _bodyEmailHelper = new BodyEmailHelper();
            _context = context;
        }
        public async Task<Result<ResetPasswordCommandResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = new ResetPasswordCommandResult();
            string email = "";
            string password = "";
            bool st = true;
            string username = "";
            var profile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Email == email);
            var user = await _fptAccountServices
                .ResetPassAsync(request.Username);
            var jsonObject = JObject.Parse(user);
            var getpass = jsonObject["result"];
            var status = jsonObject["status"];
            if (status != null) {
                st = (bool)status;
                if (st == false)
                {
                    throw new ErrorException(Domain.Enums.StatusCodeEnum.RGT02_Does_Not_Existed);
                }
            }
             if(profile == null)
             {
                username = request.Username;
             }
            else
            {
               username = profile.FirstName + " " + profile.LastName;
            }
            foreach (var item in getpass)
            {
                email = ((JProperty)item).Name;
                password = ((JProperty)item).Value.ToString();
            }
            
            string body = _bodyEmailHelper.ResetPass(password, username);
            await _emailServices.SendEmailAsync(request.Username, "Reset Password", body);

            result.Username = username;
            result.Password = password;
            return Result<ResetPasswordCommandResult>.Success(result);
        }
    }
}
