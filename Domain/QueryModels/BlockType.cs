using System;
using System.Collections.Generic;

namespace Domain.QueryModels
{
    public partial class BlockType
    {
        public BlockType()
        {
            BlockUsers = new HashSet<BlockUser>();
        }

        public Guid BlockTypeId { get; set; }
        public string BlockTypeName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<BlockUser> BlockUsers { get; set; }
    }
}
