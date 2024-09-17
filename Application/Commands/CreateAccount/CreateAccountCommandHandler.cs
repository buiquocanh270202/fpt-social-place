using Application.Services;
using AutoMapper;
using Core.CQRS;
using Core.CQRS.Command;
using Core.Helper;
using Domain.CommandModels;
using Domain.Exceptions;
using Domain.QueryModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, CreateAccountCommandResult>
    {

        private readonly FptAccountServices _fptAccountServices;
        private readonly EmailServices _emailServices;
        private readonly BodyEmailHelper _bodyEmailHelper;

        public CreateAccountCommandHandler(FptAccountServices fptAccountServices, EmailServices emailServices)
        {
            _fptAccountServices = fptAccountServices;
            _emailServices = emailServices;
            _bodyEmailHelper = new BodyEmailHelper();
        }
        public async Task<Result<CreateAccountCommandResult>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var result = new CreateAccountCommandResult();
            string pass = "";
            bool success = false;
            try
            {
                var user = await _fptAccountServices
                    .CreateChatAsync(request.Username, request.Email, request.FullName, request.RollNumber, request.Campus);
                var getuser = JObject.Parse(user);
                 pass = getuser["successUsers"]?[0]?["password"]?.ToString();
               

            }
            catch (Exception)
            {

                throw new Exception("API Error!");
            }
            if (string.IsNullOrEmpty(pass))
            {
                
                throw new ErrorException(Domain.Enums.StatusCodeEnum.RGT01_Existed);
            }
            else
            {
                success = true;
            }
            if (success) 
            {
                string body = _bodyEmailHelper.Register(request.Email, pass, request.FullName);
                await _emailServices.SendEmailAsync(request.Email, "Welcome to FPT Social", body);
            }
            result.UserName = request.Username;
            result.Email = request.Email;
            result.Password = pass;
            return Result<CreateAccountCommandResult>.Success(result);
        }
    }
}
