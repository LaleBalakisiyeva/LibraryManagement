using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.DTOs.Book
{
    public class BookUpdateDto
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
    }
}
