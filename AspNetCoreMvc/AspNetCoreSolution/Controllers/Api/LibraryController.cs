using AspNetCoreSolution.Models.Api;
using AspNetCoreSolution.Models.Api.Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = CustomClaimTypes.CanUseApi)]
    [Route("api/[controller]")]
    public class LibraryController : Controller
    {
        private ILibraryRepository _repo;

        public LibraryController(ILibraryRepository repo)
        {
            _repo = repo;
        }

        public async Task<JsonResult> Get()
        {
            var library = await _repo.GetLibrary();
            return Json(library);
        }
    }
}
