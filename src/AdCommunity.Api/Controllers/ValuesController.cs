using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ValuesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var users= _dbContext.Users.ToList();
            return Ok(users);
        }
    }
}
