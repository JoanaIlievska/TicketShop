using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Repository.Interface;
using OnlineCinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Services.Implementation
{
    public class BackgroundSender : IBackgroundEmailSender
    {

        private readonly IEmailService _emailService;
        private readonly IRepository<Email> _mailRepository;

        public BackgroundSender(IEmailService emailService, IRepository<Email> mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }
        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.Status).ToList());
        }
    }

}    

