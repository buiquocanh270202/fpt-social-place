using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("alternativeEmail")]
    public string AlternativeEmail { get; set; }
    
    [JsonProperty("citizenCardId")]
    public string CitizenCardId { get; set; }
    
    [JsonProperty("phoneNumber")]
    public string PhoneNumber { get; set; }
    
    [JsonProperty("userType")]
    public string UserType { get; set; }
    public string Avata { get; set; }
    public string FullName { get; set; }
}

public class UserInfoInCampus
{
    [JsonProperty("campusIdOrCode")]
    public string CampusIdOrCode { get; set; }
    
    [JsonProperty("RollNumber")]
    public string RollNumber { get; set; }
}

public class ProjectCampus
{
    [JsonProperty("roleIdOrCodes")]
    public List<string> RoleIdOrCodes { get; set; }
    
    [JsonProperty("projectIdOrCode")]
    public string ProjectIdOrCode { get; set; }
    
    [JsonProperty("projectUserId")]
    public string ProjectUserId { get; set; }
    
    [JsonProperty("isActive")]
    public bool IsActive { get; set; }
    
    [JsonProperty("userInfoInCampus")]
    public List<UserInfoInCampus> UserInfoInCampus { get; set; }
}

public class UserRequest
{
    [JsonProperty("user")]
    public User User { get; set; }
    
    [JsonProperty("projectCampuses")]
    public List<ProjectCampus> ProjectCampuses { get; set; }
}
