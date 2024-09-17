using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FptAccountServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FptAccountServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> CreateChatAsync(string username, string email,string fullname, string rollnumber, string campus)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, 
                "https://gateway.ptudev.net/api/identity-management/users/account/project-owner-request-token");

            var chat = new
            {
                ClientId = _configuration["FptAccount:ClientId"],
                ClientSecret = _configuration["FptAccount:ClientSecret"],
                Scope = _configuration["FptAccount:Scope"],
                UserName = _configuration["FptAccount:Username"],
                Password = _configuration["FptAccount:Password"],
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("Access Forbidden: Ensure your Private Key is correct.");
            }
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            var login = new HttpRequestMessage(HttpMethod.Post,
                "https://gateway.ptudev.net/api/identity-management/users/account/project-owners");
            login.Headers.Add("Authorization", $"Bearer {token}");
            var lissuser = new List<UserRequest>();
            var userRequest = new UserRequest
            {
                User = new User
                {
                    Id = "00000000-0000-0000-0000-000000000000",
                    Name = fullname,
                    Username = username,
                    Email = email,
                    AlternativeEmail = email,
                    CitizenCardId = "099988877786",
                    PhoneNumber = "0999888776",
                    UserType = "Societe-student"
                },
                ProjectCampuses = new List<ProjectCampus>
                {
                    new ProjectCampus
                    {
                        RoleIdOrCodes = new List<string> { "Societe-student" },
                        ProjectIdOrCode = "Societe",
                        ProjectUserId = "123",
                        IsActive = true,
                        UserInfoInCampus = new List<UserInfoInCampus>
                        {
                            new UserInfoInCampus
                            {
                                CampusIdOrCode = campus,
                                RollNumber = rollnumber
                            }
                        }
                    }
                }
            };
            lissuser.Add(userRequest);
            string json = JsonConvert.SerializeObject(lissuser);
            login.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginresponse = await _httpClient.SendAsync(login);
            if (loginresponse.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("Access Forbidden: Ensure your Private Key is correct.");
            }
            loginresponse.EnsureSuccessStatusCode();

            return await loginresponse.Content.ReadAsStringAsync();
        }


        public async Task<string> ResetPassAsync(string username)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://gateway.ptudev.net/api/identity-management/users/account/project-owner-request-token");

            var chat = new
            {
                ClientId = _configuration["FptAccount:ClientId"],
                ClientSecret = _configuration["FptAccount:ClientSecret"],
                Scope = _configuration["FptAccount:Scope"],
                UserName = _configuration["FptAccount:Username"],
                Password = _configuration["FptAccount:Password"],
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(chat), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("Access Forbidden: Ensure your Private Key is correct.");
            }
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            var login = new HttpRequestMessage(HttpMethod.Post,
                "https://gateway.ptudev.net/api/identity-management/users/account/project-owners/reset-password-by-username");
            login.Headers.Add("Authorization", $"Bearer {token}");
            var lissuser = new List<string>();
           
            lissuser.Add(username);
            string json = JsonConvert.SerializeObject(lissuser);
            login.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var loginresponse = await _httpClient.SendAsync(login);
            if (loginresponse.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new Exception("Access Forbidden: Ensure your Private Key is correct.");
            }
            loginresponse.EnsureSuccessStatusCode();

            return await loginresponse.Content.ReadAsStringAsync();
        }

    }
}
