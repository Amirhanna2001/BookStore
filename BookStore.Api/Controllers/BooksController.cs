using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using BookStore.CORE;
using Microsoft.AspNetCore.Mvc;
using BookStore.CORE.Consts;
using BookStore.CORE.DTOs;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IImageProcesses _imageProcesses;

    public BooksController(IUnitOfWork unitOfWork, IConfiguration config, IImageProcesses imageProcesses)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _imageProcesses = imageProcesses;
    }
    [HttpGet]
    public async Task<ActionResult<Book>> GetBooks(int pageNumber)
        => Ok(_unitOfWork.Books.GetAll(Paggination.PageSize, --pageNumber,["Author","Category"]));
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        Book Book = await _unitOfWork.Books.FindAsync(b=>b.Id == id, ["Author", "Category"]);
        if (Book == null)
            return NotFound($"No Book found by id = {id}");

        return Ok(Book);
    }
    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromForm] BookDTO dto)
    {
        if (dto == null)
            return BadRequest();

        if (dto.Image == null)
            return BadRequest("Please attach an image");

        Book book = new();

        if (_imageProcesses.IsAvailableExtension(dto.Image))
            book.ImageUrl = await _imageProcesses.StoreImage(dto.Image, _config["ImageStorage:Book"]);
        else
            return BadRequest("Only allowed extensions (.PNG & .JPG)");

        if (_unitOfWork.Categories.GetById(dto.CategoryId) == null)
            return BadRequest($"No categories found by id = {dto.CategoryId}");

        if (_unitOfWork.Authors.GetById(dto.AuthorId) == null)
            return BadRequest($"No authors found by id = {dto.AuthorId}");

        book.Name = dto.Name;
        book.AuthorId = dto.AuthorId;
        book.CategoryId = dto.CategoryId;
        book.Quantity = dto.Quantity;
        book.Price = dto.Price;
        book.SalePercentage = dto.SalePercentage;

        _unitOfWork.Books.Add(book);
        _unitOfWork.SaveChanges();

        return Ok(book);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> Edit(int id, [FromForm] BookDTO dto)
    {
        string folderPath = _config["ImageStorage:Book"];
        Book book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null)
            return NotFound($"No Book found by id = {id}");

        if (dto.Image != null)
        {
            string oldImagePath = book.ImageUrl;
            if (_imageProcesses.IsAvailableExtension(dto.Image))
                book.ImageUrl = await _imageProcesses.StoreImage(dto.Image, folderPath);
            else
                return BadRequest("Only allowed extensions (.PNG & .JPG)");

            _imageProcesses.DeleteImage(Path.Combine(folderPath, oldImagePath));
        }
        if (_unitOfWork.Categories.GetById(dto.CategoryId) == null)
            return BadRequest($"No categories found by id = {dto.CategoryId}");

        if (_unitOfWork.Authors.GetById(dto.AuthorId) == null)
            return BadRequest($"No authors found by id = {dto.AuthorId}");

        book.Name = dto.Name;
        book.AuthorId = dto.AuthorId;
        book.CategoryId = dto.CategoryId;
        book.Quantity = dto.Quantity;
        book.Price = dto.Price;
        book.SalePercentage = dto.SalePercentage;

        _unitOfWork.Books.Update(book);
        _unitOfWork.SaveChanges();
        return Ok(book);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<Book>> Delete(byte id)
    {
        Book Book = await _unitOfWork.Books.GetByIdAsync(id);
        if (Book == null)
            return NotFound($"No Book found by id = {id}");

        _unitOfWork.Books.Delete(Book, Path.Combine(_config["ImageStorage:Book"], Book.ImageUrl));
        _unitOfWork.SaveChanges();
        return Ok(Book);
    }
}
