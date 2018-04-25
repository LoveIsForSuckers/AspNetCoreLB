using AspNetCoreSolution.Models.Api;
using AspNetCoreSolution.Models.Api.UserGame;
using AspNetCoreSolution.Models.IdentityModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace AspNetCoreSolution.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = CustomClaimTypes.CanUseApi)]
    [Route("api/[controller]")]
    public class UserGameController : Controller
    {
        private const string MODEL_NAME = "UserGame";

        private IUserGameRepository _repo;

        public UserGameController(IUserGameRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Policy = CustomClaimTypes.CanGetEveryonesData)]
        [HttpGet("{skip}")]
        public async Task<JsonResult> Get([FromBody]int skip, [FromBody]int limit = 50)
        {
            return Json(SimpleResponse.Content(await _repo.GetUserGames(skip, limit)));
        }

        [Authorize(Policy = CustomClaimTypes.CanGetEveryonesData)]
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            var userGame = await _repo.GetUserGame(id);
            return Json(SimpleResponse.Content(userGame));
        }
        
        [Authorize(Policy = CustomClaimTypes.OwnsUserGame)]
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var claims = User.Claims;
            var claim = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.OwnsUserGame);

            int id = int.Parse(claim.Value);
            
            var userGame = await _repo.GetUserGame(id);
            return Json(SimpleResponse.Content(userGame));
        }

        [Authorize(Policy = CustomClaimTypes.OwnsUserGame)]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]UserGame value)
        {
            if (value == null)
                return Json(SimpleResponse.Error("Invalid value"));

            var claims = User.Claims;
            var claim = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.OwnsUserGame);

            int id = int.Parse(claim.Value);
            if (id != value.Id)
                return Json(SimpleResponse.Error("Invalid value"));

            var userGame = await _repo.GetUserGame(id);

            if (userGame != null)
            {
                var result = await _repo.ReplaceUserGame(id, value);
                return Json(SimpleResponse.Success());
            }
            else
            {
                await _repo.AddUserGame(value);
                return Json(SimpleResponse.Success());
            }
        }
    }
}
