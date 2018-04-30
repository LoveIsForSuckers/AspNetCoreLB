using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api.Library
{
    public class LibraryViewModel
    {
        public IEnumerable<Starship> starships;
        public IEnumerable<Weapon> weapons;
        public IEnumerable<Upgrade> upgrades;
        public int version;

        public void ResolveVersion()
        {
            int result = 0;

            foreach (var starship in starships)
                result += starship.Version;
            foreach (var weapon in weapons)
                result += weapon.Version;
            foreach (var upgrade in upgrades)
                result += upgrade.Version;
        }
    }
}
