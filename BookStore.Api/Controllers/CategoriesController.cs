using AutoMapper;
using BookStore.CORE;
using BookStore.CORE.DTOs;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly IImageProcesses _imageProcesses;

    public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, IImageProcesses imageProcesses)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _config = config;
        _imageProcesses = imageProcesses;
    }
    [HttpGet]
    public async Task<ActionResult<Category>> GetCategories(int pageNumber)
    {
        int pageSize =int.Parse(_config["Pagination:PageSize"]);
        return Ok( _unitOfWork.Categories.GetAll(pageSize,--pageNumber));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>>GetById(byte id)
    {
        Category cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if(cat == null) 
            return NotFound($"No category found by id = {id}");

        return Ok(cat);
    }
    [HttpPost]
    public async Task<ActionResult<Category>>Create(CreateCategoryDTO dto)
    {
        if (dto == null)
            return BadRequest();

        if(dto.Image == null)
            return BadRequest("Please attach an image");

        Category cat = new();

        if (_imageProcesses.IsAvailableExtension(dto.Image))
            cat.ImageUrl = await _imageProcesses.StoreImage(dto.Image, _config["ImageStorage:Category"]);
        else
            return BadRequest("Only allowed extensions (.PNG & .JPG)");
        cat.Name = dto.Name;
        _unitOfWork.Categories.Add(cat);
        _unitOfWork.SaveChanges();

        return Ok(cat);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Category>>Edit(byte id, [FromForm]CreateCategoryDTO dto)
    {
        Category cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if(cat == null)
            return NotFound($"No category found by id = {id}");

        if (dto.Image != null)
        {
            string path = cat.ImageUrl;
            if (_imageProcesses.IsAvailableExtension(dto.Image))
                cat.ImageUrl = await _imageProcesses.StoreImage(dto.Image, _config["ImageStorage:Category"]);
            else
                return BadRequest("Only allowed extensions (.PNG & .JPG)");

            _imageProcesses.DeleteImage(path);
        }
        cat.Name = dto.Name;
        _unitOfWork.Categories.Update(cat);
        _unitOfWork.SaveChanges();
        return Ok(cat);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Category>> Delete(byte id)
    {
        Category cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if (cat == null)
            return NotFound($"No category found by id = {id}");

        _unitOfWork.Categories.Delete(cat,Path.Combine( _config["ImageStorage:Category"],cat.ImageUrl));
        _unitOfWork.SaveChanges();
        return Ok(cat);

    }
}