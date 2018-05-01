using AspNetCoreSolution.Models.Api.CommonData;
using System.Collections.Generic;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Upgrade : BaseModel
    {
        public string Title { get; set; }
        public string Preview { get; set; }

        public IList<UpgradeLevel> Levels { get; set; }
    }

    public class UpgradeLevel
    {
        public StarshipStats Stats { get; set; } = new StarshipStats();
        public Currency Price { get; set; } = new Currency();
    }
}
