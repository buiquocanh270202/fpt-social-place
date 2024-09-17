using System;
namespace Application.Queries.GenInterest
{
	public class GetInterestResult
	{
        public Guid InterestId { get; set; }
        public string InterestName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
	}
}

