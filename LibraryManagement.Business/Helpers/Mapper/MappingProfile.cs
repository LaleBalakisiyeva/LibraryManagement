using AutoMapper;
using LibraryManagement.Business.DTOs.Author;
using LibraryManagement.Business.DTOs.Book;
using LibraryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryManagement.Business.Helpers.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : null));
            CreateMap<BookCreateDto, Book>();
            CreateMap<BookUpdateDto, Book>();

            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<AuthorCreateDto, Author>();
            CreateMap<AuthorUpdateDto, Author>();
        }
    }
}
