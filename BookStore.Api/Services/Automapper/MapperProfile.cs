using AutoMapper;
using BookStore.CORE.DTOs;
using BookStore.CORE.Models;

namespace BookStore.Api.Services;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Book,ReturnBookDTO>()
            .ForMember(b=>b.CategoryName , opt => opt.MapFrom(b=>b.Category.Name))
            .ForMember(b=>b.AuthorName , opt => opt.MapFrom(b=>b.Author.Name));
        CreateMap<BookDTO, Book>()
            .ForMember(b=>b.ImageUrl,opt=>opt.Ignore()).ReverseMap();
    }
}
