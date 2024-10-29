using AutoMapper;
using BookStore.CORE;
using BookStore.CORE.Consts;
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
    private readonly IConfiguration _config;
    private readonly IImageProcesses _imageProcesses;

    public CategoriesController(IUnitOfWork unitOfWork, IConfiguration config, IImageProcesses imageProcesses)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _imageProcesses = imageProcesses;
    }
    [HttpGet]
    public ActionResult<Category> GetCategories(int pageNumber)
        => Ok( _unitOfWork.Categories.GetAll(Pagination.PageSize,--pageNumber));
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>>GetById(byte id)
    {
        Category cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if(cat == null) 
            return NotFound($"No category found by id = {id}");

        return Ok(cat);
    }
    [HttpPost]
    public async Task<ActionResult<Category>>Create([FromForm]CategoryDTO dto)
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
    public async Task<ActionResult<Category>>Edit(byte id, [FromForm]CategoryDTO dto)
    {
        string folderPath = _config["ImageStorage:Category"];
        Category cat = await _unitOfWork.Categories.GetByIdAsync(id);
        if(cat == null)
            return NotFound($"No category found by id = {id}");

        if (dto.Image != null)
        {
            string oldImagePath = cat.ImageUrl;
            if (_imageProcesses.IsAvailableExtension(dto.Image))
                cat.ImageUrl = await _imageProcesses.StoreImage(dto.Image, folderPath);
            else
                return BadRequest("Only allowed extensions (.PNG & .JPG)");

            _imageProcesses.DeleteImage(Path.Combine(folderPath,oldImagePath));
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