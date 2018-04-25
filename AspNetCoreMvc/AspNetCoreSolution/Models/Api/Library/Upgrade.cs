using AspNetCoreSolution.Models.Api.CommonData;
using System.Collections.Generic;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Upgrade : BaseModel
    {
        public string title;
        public string preview;

        public IEnumerable<UpgradeLevel> levels;
    }

    public class UpgradeLevel
    {
        public StarshipStats stats;
        public Currency price;
    }
}
