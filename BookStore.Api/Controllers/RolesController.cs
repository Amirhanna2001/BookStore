using BookStore.CORE.DTOs;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RolesController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    [HttpPost]
    public async Task< IActionResult> Create(string name)
    {
        RoleReturnModel result = await _roleRepository.CreateRole(name);
        if (!result.IsSuccessfully)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllRoles()
        => Ok(await _roleRepository.GetAllRolesAsync());
    [HttpPut]
    public async Task<IActionResult> Update(UpdateRoleDto dto)
    {
        var result = await _roleRepository.Update(dto);
        if (!result.IsSuccessfully)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(string role)
    {
        var result = await _roleRepository.Delete(role);
        if (!result.IsSuccessfully)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
    [HttpPost("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(AddUserToRoleDto dto)
    {
        var result =  await _roleRepository.AddUserToRole(dto.UserId, dto.RoleName);
        if(!result.IsSuccessfully)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}