using BookStore.CORE;
using BookStore.CORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>>GetById(byte id)
    {
        return Ok( _unitOfWork.Categories.GetById(id));
    }
}
