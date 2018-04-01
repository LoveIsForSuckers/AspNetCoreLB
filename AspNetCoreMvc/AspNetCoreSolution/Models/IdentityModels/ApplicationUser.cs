using AspNetCore.Identity.MongoDbCore.Models;
using System;

namespace AspNetCoreSolution.Models.IdentityModels
{
    public class ApplicationUser : MongoIdentityUser<int>
    {
        public ApplicationUser() :base()
        {

        }

        public ApplicationUser(string userName, string email) : base (userName, email)
        {

        }
    }
}
