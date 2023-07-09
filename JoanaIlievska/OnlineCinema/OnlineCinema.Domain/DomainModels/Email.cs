using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCinema.Domain.DomainModels
{
    public class Email : BaseEntity
    {
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Boolean Status { get; set; }


    }
}
