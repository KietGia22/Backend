using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyDiary.API.Factories;
using MyDiary.API.Models;
using MyDiary.Application.Auth.Models;
using MyDiary.Application.Contracts.Identity;
using MyDiary.Application.CurrentUser.Dtos;
using MyDiary.Application.CurrentUser.Queries.GetCurrentUser;
using MyDiary.Application.Diary.Dtos;
using System.Net;

namespace MyDiary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    private readonly IMediator _mediator;

    public AuthController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(APIResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request) {
        var result = await _authService.Login(request);
        return Ok(APIResponseFactory.Create<AuthResponse>(HttpStatusCode.OK, true, result));
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(APIResponse<RegistrationResponse>), StatusCodes.Status200OK)]

    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var result = await _authService.Register(request);
        return Ok(APIResponseFactory.Create<RegistrationResponse>(HttpStatusCode.OK, true, result));
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(APIResponse<AuthResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshToken(refreshToken);
        return Ok(APIResponseFactory.Create<AuthResponse>(HttpStatusCode.OK, true, result));
    }

    [HttpGet("GetCurrentUser")]
    [ProducesResponseType(typeof(APIResponse<UserDto>), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var result = await _mediator.Send(new GetCurrentUserQuery());
        return Ok(APIResponseFactory.Create<UserDto>(HttpStatusCode.OK, true, result));
    }
    
}