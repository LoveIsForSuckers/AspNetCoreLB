namespace AspNetCoreSolution.Models.Api.UserGame
{
    public class UserGame : BaseModel
    {
        public string Name { get; set; }
        public int LevelId { get; set; }
        public UserCurrency currency { get; set; }
    }
}
