using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.v1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankingController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok();
    }
}