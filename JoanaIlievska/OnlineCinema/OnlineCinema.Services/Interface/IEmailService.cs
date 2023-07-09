using OnlineCinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Services.Interface
{
   
        public interface IEmailService
        {
            Task SendEmailAsync(List<Email> allMails);
        }
    
}
