using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api.UserGame
{
    public struct UserStarship
    {
        public int starshipId;
        public int weaponId;
        public IEnumerable<UpgradeData> upgrades;
    }

    public struct UpgradeData
    {
        public int upgradeId;
        public int level;
    }
}
