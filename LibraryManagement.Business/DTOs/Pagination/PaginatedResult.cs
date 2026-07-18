using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.DTOs.Pagination
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; } 
        public int TotalCount { get; set; }      
        public int PageNumber { get; set; }      
        public int PageSize { get; set; }                
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
