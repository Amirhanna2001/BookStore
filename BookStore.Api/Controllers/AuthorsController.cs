using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using BookStore.CORE;
using Microsoft.AspNetCore.Mvc;
using BookStore.CORE.DTOs;
using BookStore.CORE.Consts;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IImageProcesses _imageProcesses;

    public AuthorsController(IUnitOfWork unitOfWork, IConfiguration config, IImageProcesses imageProcesses)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _imageProcesses = imageProcesses;
    }
    [HttpGet]
    public ActionResult<Author> GetAuthors(int pageNumber)
        => Ok(_unitOfWork.Authors.GetAll(Pagination.PageSize, --pageNumber));
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetById(int id)
    {
        Author author = await _unitOfWork.Authors.GetByIdAsync(id);
        if (author == null)
            return NotFound($"No Author found by id = {id}");

        return Ok(author);
    }
    [HttpPost]
    public async Task<ActionResult<Author>> Create([FromForm] AuthorDTO dto)
    {
        if (dto == null)
            return BadRequest();

        if (dto.Image == null)
            return BadRequest("Please attach an image");

        Author author = new();

        if (_imageProcesses.IsAvailableExtension(dto.Image))
            author.ImageUrl = await _imageProcesses.StoreImage(dto.Image, _config["ImageStorage:Author"]);
        else
            return BadRequest("Only allowed extensions (.PNG & .JPG)");

        author.Name = dto.Name;
        _unitOfWork.Authors.Add(author);
        _unitOfWork.SaveChanges();

        return Ok(author);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Author>> Edit(int id, [FromForm] AuthorDTO dto)
    {
        string folderPath = _config["ImageStorage:Author"];
        Author author = await _unitOfWork.Authors.GetByIdAsync(id);
        if (author == null)
            return NotFound($"No Author found by id = {id}");

        if (dto.Image != null)
        {
            string oldImagePath = author.ImageUrl;
            if (_imageProcesses.IsAvailableExtension(dto.Image))
                author.ImageUrl = await _imageProcesses.StoreImage(dto.Image, folderPath);
            else
                return BadRequest("Only allowed extensions (.PNG & .JPG)");

            _imageProcesses.DeleteImage(Path.Combine(folderPath, oldImagePath));
        }
        author.Name = dto.Name;
        _unitOfWork.Authors.Update(author);
        _unitOfWork.SaveChanges();
        return Ok(author);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Author>> Delete(int id)
    {
        Author author = await _unitOfWork.Authors.GetByIdAsync(id);
        if (author == null)
            return NotFound($"No Author found by id = {id}");

        _unitOfWork.Authors.Delete(author, Path.Combine(_config["ImageStorage:Author"], author.ImageUrl));
        _unitOfWork.SaveChanges();
        return Ok(author);
    }
}