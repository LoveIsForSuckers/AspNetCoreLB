using AspNetCoreSolution.Models.Api.CommonData;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class Weapon : BaseModel
    {
        [Required]
        [StringLength(36)]
        public string Title { get; set; }

        public string Preview { get; set; }
        public string GameImage { get; set; }
        public string ShotImage { get; set; }

        [Range(0, 9000)]
        public int BaseAttack { get; set; }
        [Range(0, 9000)]
        public int AttackDelta { get; set; }
        [Range(0, 100)]
        public float AttackSpeed { get; set; }

        [Required]
        public Currency Price { get; set; } = new Currency();
    }
}
