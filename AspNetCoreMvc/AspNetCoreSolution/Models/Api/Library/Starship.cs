using AspNetCoreSolution.Models.Api.CommonData;
using AspNetCoreSolution.Models.Api.UserGame;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Starship : BaseModel
    {
        public string title;
        public string preview;

        public string gameImage;

        public StarshipStats baseStats;
        public Currency price;
    }
}
