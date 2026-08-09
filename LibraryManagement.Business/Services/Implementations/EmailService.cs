using LibraryManagement.Business.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Implementations
{
    public  class EmailService :IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendWelcomeEmailAsync(string userEmail)
        {
            _logger.LogInformation("Asynchronous process started: Sending notification to {Email}...", userEmail);

            await Task.Delay(5000);

            _logger.LogInformation("Successful! Congratulations email sent in the background: {Email}", userEmail);
        }
    }
}
