using BookStore.CORE.DTOs;
using BookStore.CORE.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.CORE.Repositories;
public interface IRoleRepository
{
    Task<RoleReturnModel> CreateRole(string roleName);
    Task<List<string>> GetAllRolesAsync();
    Task<RoleReturnModel> Update(UpdateRoleDto dto);
    Task<RoleReturnModel> Delete(string roleName);
    Task<RoleReturnModel> AddUserToRole(string userId, string role);

}