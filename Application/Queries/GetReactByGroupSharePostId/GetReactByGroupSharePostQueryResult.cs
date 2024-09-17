using Application.DTO.ReactDTO;

namespace Application.Queries.GetReactByGroupSharePostId
{
    public class GetReactByGroupSharePostQueryResult
    {
        public int SumOfReact { get; set; }
        public bool? IsReact { get; set; }
        public List<ReactGroupSharePostDTO>? ListUserReact { get; set; }
        public List<ReactTypeCountDTO>? ListReact { get; set; }
    }
}