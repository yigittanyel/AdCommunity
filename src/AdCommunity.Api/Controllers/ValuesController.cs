using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //private readonly IYtMediator _ytMediator;

        //public ValuesController(IYtMediator ytMediator)
        //{
        //    _ytMediator = ytMediator;
        //}

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var query=new GetUsersQuery();
        //    return Ok(_ytMediator.Send(query));
        //}
    }
}
