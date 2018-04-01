using AspNetCore.Identity.MongoDbCore.Models;
using System;

namespace AspNetCoreSolution.Models.IdentityModels
{
    public class ApplicationRole : MongoIdentityRole<int>
    {
        public ApplicationRole() : base()
        {

        }

        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }
}
