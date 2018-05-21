using AspNetCoreSolution.Models.Api.CommonData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Upgrade : BaseModel
    {
        public string Title { get; set; }
        public string Preview { get; set; }

        [Required]
        [MinLength(1)]
        public IList<UpgradeLevel> Levels { get; set; } = new List<UpgradeLevel>();
    }

    public class UpgradeLevel
    {
        public StarshipStats Stats { get; set; } = new StarshipStats();
        public Currency Price { get; set; } = new Currency();
    }
}
