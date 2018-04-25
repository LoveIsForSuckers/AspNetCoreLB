using AspNetCoreSolution.Models.Api.CommonData;

namespace AspNetCoreSolution.Models.Api.UserGame
{
    public class UserGame : BaseModel
    {
        public string Name { get; set; }
        public int LevelId { get; set; }
        public Currency Currency { get; set; }
        public UserStarship Starship { get; set; }
    }
}
