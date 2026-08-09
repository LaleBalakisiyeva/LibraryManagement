using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string userEmail);
    }
}
