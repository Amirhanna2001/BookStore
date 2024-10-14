using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using BookStore.CORE;
using Microsoft.AspNetCore.Mvc;
using BookStore.CORE.Consts;
using BookStore.CORE.DTOs;
using AutoMapper;

namespace BookStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IImageProcesses _imageProcesses;
    private readonly IMapper _mapper;

    public BooksController(IUnitOfWork unitOfWork, IConfiguration config, IImageProcesses imageProcesses, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _imageProcesses = imageProcesses;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<Book>> GetBooks(int pageNumber)
    {

        var books = _unitOfWork.Books.GetAll(Pagination.PageSize, --pageNumber,["Author","Category"]);
        if (books == null || !books.Any())
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<ReturnBookDTO>>(books));
    }
        
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        Book Book = await _unitOfWork.Books.FindAsync(b=>b.Id == id, ["Author", "Category"]);
        if (Book == null)
            return NotFound($"No Book found by id = {id}");

        return Ok(_mapper.Map<ReturnBookDTO>(Book));
    }
    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromForm] BookDTO dto)
    {
        if (dto == null)
            return BadRequest();

        if (dto.Image == null)
            return BadRequest("Please attach an image");

        string imagePath = "";
        if (_imageProcesses.IsAvailableExtension(dto.Image))
            imagePath = await _imageProcesses.StoreImage(dto.Image, _config["ImageStorage:Book"]);
        else
            return BadRequest("Only allowed extensions (.PNG & .JPG)");

        if (_unitOfWork.Categories.GetById(dto.CategoryId) == null)
            return BadRequest($"No categories found by id = {dto.CategoryId}");

        if (_unitOfWork.Authors.GetById(dto.AuthorId) == null)
            return BadRequest($"No authors found by id = {dto.AuthorId}");

        Book book = _mapper.Map<Book>(dto);
        book.ImageUrl = imagePath;
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

        string imagePath = book.ImageUrl;
        if (dto.Image != null)
        {
            string oldImagePath = book.ImageUrl;
            if (_imageProcesses.IsAvailableExtension(dto.Image))
                imagePath = await _imageProcesses.StoreImage(dto.Image, folderPath);
            else
                return BadRequest("Only allowed extensions (.PNG & .JPG)");

            _imageProcesses.DeleteImage(Path.Combine(folderPath, oldImagePath));
        }
        if (_unitOfWork.Categories.GetById(dto.CategoryId) == null)
            return BadRequest($"No categories found by id = {dto.CategoryId}");

        if (_unitOfWork.Authors.GetById(dto.AuthorId) == null)
            return BadRequest($"No authors found by id = {dto.AuthorId}");

        book = _mapper.Map<Book>(dto);
        book.ImageUrl = imagePath;

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
    [HttpGet("GetBooksByCategory")]
    public async Task<ActionResult<Book>>  GetAllBooksFilteredByCategory(byte catId,int pageNum)
    {
       Category category = await _unitOfWork.Categories.GetByIdAsync(catId);
        if (category == null) 
            return NotFound("Category you want is not exists ");

        return Ok(_unitOfWork.Books.GetAll(b=>b.CategoryId == catId,Pagination.PageSize,--pageNum, ["Author"]));
    }
    [HttpGet("GetBooksByAuthor")]
    public async Task<ActionResult<Book>>  GetAllBooksFilteredByAuthor(int authorId,int pageNum)
    {
       Author author = await _unitOfWork.Authors.GetByIdAsync(authorId);
        if (author == null) 
            return NotFound("Author you want is not exists ");

        return Ok(_unitOfWork.Books.GetAll(b=>b.CategoryId == authorId,Pagination.PageSize,--pageNum, [ "Category"]));
    }
       
}