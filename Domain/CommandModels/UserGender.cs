using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class UserGender
    {
        public Guid UserGenderId { get; set; }
        public Guid GenderId { get; set; }
        public Guid UserId { get; set; }
        public Guid? UserStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Gender Gender { get; set; } = null!;
        public virtual UserProfile User { get; set; } = null!;
        public virtual UserStatus UserStatus { get; set; } = null!;
    }
}
