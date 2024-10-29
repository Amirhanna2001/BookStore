using BookStore.CORE.DTOs;
using BookStore.CORE.Models;

namespace BookStore.CORE.Repositories;
public interface IAuthRepository
{
    public Task<AuthModel> RegisterAsync(RegisterDTO dto);
}