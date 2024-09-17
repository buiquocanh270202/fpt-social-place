namespace Application.Queries.GetUserRelationships
{
    public class GetUserRelationshipResult
    {
        public Guid UserRelationShipId { get; set; }
        public Guid RelationshipId { get; set; }

        public string? RelationshipName { get; set; }
        public Guid UserId { get; set; }
        public Guid UserStatusId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}