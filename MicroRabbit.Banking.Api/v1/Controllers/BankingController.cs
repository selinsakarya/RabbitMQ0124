using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.v1.Controllers;

[Route("api/v1[controller]")]
[ApiController]
public class BankingController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok();
    }
}