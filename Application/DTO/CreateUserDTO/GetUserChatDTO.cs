using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.CreateUserDTO
{
    public class User
    {
        public int Id { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsOnline { get; set; }
        public string Username { get; set; }
        public string Secret { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public Dictionary<string, object> CustomJson { get; set; }
        public LastMessage LastMessage { get; set; }
        public DateTime Created { get; set; }
    }

    public class LastMessage
    {
        public string Text { get; set; }
        public string Created { get; set; }
        public List<object> Attachments { get; set; }
    }

}
