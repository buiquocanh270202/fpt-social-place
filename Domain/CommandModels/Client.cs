using System;
using System.Collections.Generic;

namespace Domain.CommandModels
{
    public partial class Client
    {
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string ClientUrl { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? ClientType { get; set; }
    }
}
