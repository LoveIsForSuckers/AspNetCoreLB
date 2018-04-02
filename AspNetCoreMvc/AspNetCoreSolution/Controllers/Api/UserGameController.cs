using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSolution.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSolution.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserGameController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            var dataSet = GetDebugDataset();
            return Json(new { users = dataSet });
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var dataSet = GetDebugDataset();
            var item = dataSet.Find(x => x.Id == id);
            return Json(item);
        }

        private List<UserGame> GetDebugDataset()
        {
            var dataSet = new List<UserGame>();
            dataSet.Add(new UserGame() { Id = 1, LevelId = 0, Name = "Petya", currency = new UserCurrency() { Soft = 0, Hard = 0 } });
            dataSet.Add(new UserGame() { Id = 2, LevelId = 3, Name = "Kendrick", currency = new UserCurrency() { Soft = 250, Hard = 0 } });
            return dataSet;
        }
        
        [HttpPost]
        public string Post([FromBody]UserGame value)
        {
            if (value != null)
                return "Success";
            else
                return "Fail!";
        }
    }
}
