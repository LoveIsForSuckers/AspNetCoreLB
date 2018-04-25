using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api
{
    public class CustomClaimTypes
    {
        public const string CanEditLibrary = "CanEditLibrary";
        public const string CanUseApi = "CanUseApi";
        public const string CanGetEveryonesData = "CanGetEveryonesData";
        public const string OwnsUserGame = "OwnsUserGame";
    }
}
