using AspNetCoreSolution.Models.Api;
using AspNetCoreSolution.Models.Api.UserGame;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UserGameController : Controller
    {
        private const string MODEL_NAME = "UserGame";

        private IUserGameRepository _repo;

        public UserGameController(IUserGameRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return Json(SimpleResponse.Content(await _repo.GetUserGames()));
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            var userGame = await _repo.GetUserGame(id);
            return Json(SimpleResponse.Content(userGame));
        }
        
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]UserGame value)
        {
            if (value == null)
                return Json(SimpleResponse.Error("Invalid value"));

            int id = value.Id;
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
