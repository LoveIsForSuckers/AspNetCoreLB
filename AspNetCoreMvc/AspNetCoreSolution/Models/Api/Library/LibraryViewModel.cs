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
    }
}
