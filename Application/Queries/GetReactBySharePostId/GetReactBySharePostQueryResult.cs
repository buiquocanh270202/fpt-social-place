using Application.DTO.ReactDTO;

namespace Application.Queries.GetReactBySharePostId
{
    public class GetReactBySharePostQueryResult
    {
        public int SumOfReact { get; set; }
        public bool? IsReact { get; set; }
        public List<ReactSharePostDTO>? ListUserReact { get; set; }
        public List<ReactTypeCountDTO>? ListReact { get; set; }
    }
}