using BookStore.CORE.DTOs;
using BookStore.CORE.Models;

namespace BookStore.CORE.Repositories;
public interface IAuthRepository
{
    Task<AuthModel> RegisterAsync(RegisterDTO dto);
    Task<AuthModel> Login(LoginDTO loginDTO);
}