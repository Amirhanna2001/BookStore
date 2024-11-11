using BookStore.CORE.DTOs;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AccountsController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        AuthModel result = await _authRepository.RegisterAsync(dto);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        AuthModel result = await _authRepository.Login(dto);

        if (!result.IsAuthenticated)
            return BadRequest(result.Message);

        return Ok(result);
    }

}