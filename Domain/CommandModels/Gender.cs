using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class Gender
    {
        public Gender()
        {
            UserGenders = new HashSet<UserGender>();
        }

        public Guid GenderId { get; set; }
        public string GenderName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<UserGender> UserGenders { get; set; }
    }
}
