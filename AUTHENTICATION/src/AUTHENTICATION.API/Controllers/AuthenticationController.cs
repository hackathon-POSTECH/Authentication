using AUTHENTICATION.API.request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AUTHENTICATION.API.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserRequest request)
    {
        try
        {
            await _mediator.Send(request.ToCommand());
            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request)
    {
        try
        {
            var result = await _mediator.Send(request.ToCommand());
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
