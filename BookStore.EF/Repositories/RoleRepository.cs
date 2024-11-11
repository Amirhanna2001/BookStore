using BookStore.CORE.DTOs;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.EF.Repositories;
public class RoleRepository : IRoleRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<RoleReturnModel> CreateRole(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName});
        if(!result.Succeeded)
            return ErrorResult(result);

        return new RoleReturnModel() {IsSuccessfully = true, Message = "Role created successfully" };
    }

    public async Task<RoleReturnModel> Delete(string roleName)
    {
        IdentityRole role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
            return new RoleReturnModel() { Message =$"Role: {roleName} is not found" };

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
            return ErrorResult(result);

        return new RoleReturnModel() { IsSuccessfully = true, Message = "Role deleted successfully" };
    }

   

    public async Task<List<string>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        List<string> result = new ();
        foreach(var role in roles) 
            result.Add(role.Name);
        return result;
    }
        
    public async Task<RoleReturnModel> Update(UpdateRoleDto dto)
    {
        IdentityRole role = await _roleManager.FindByNameAsync(dto.OldName);
        if (role is null)
            return new RoleReturnModel() { Message = $"Role: {dto.OldName} not found" };

        role.Name = dto.NewName;
        var result = await _roleManager.UpdateAsync(role);
        if(!result.Succeeded)
            return ErrorResult(result);

        return new RoleReturnModel() {IsSuccessfully = true, Message = "Role updated" };
    }
    public async Task<RoleReturnModel> AddUserToRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return new RoleReturnModel() { Message = $"No Users found by this id {userId}" };

        var roleRes = await _roleManager.FindByNameAsync(role);
        if (roleRes is null)
            return new RoleReturnModel() { Message = $"Role {role} is not found" };

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            return new RoleReturnModel() { Message = string.Join(", ", result.Errors.Select(error => error.Description)) };

        return new RoleReturnModel() { Message = $"User: {user.FirstName} is added to role: {role}" };
    }

    private static RoleReturnModel ErrorResult(IdentityResult result)
    {
        return new RoleReturnModel() { Message =  string.Join(", ", result.Errors.Select(error => error.Description)) };
    }
}