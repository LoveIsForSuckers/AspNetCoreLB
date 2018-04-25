using AspNetCoreSolution.Models.Api.CommonData;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Weapon : BaseModel
    {
        public string title;
        public string preview;

        public string gameImage;
        public string shotImage;

        public int baseAttack;
        public int attackDelta;
        public int attackSpeed;

        public Currency price;
    }
}
