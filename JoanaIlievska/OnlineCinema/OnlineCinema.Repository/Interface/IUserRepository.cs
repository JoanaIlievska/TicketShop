using System;
using System.Collections.Generic;
using System.Text;
using OnlineCinema.Domain.Identity;

namespace OnlineCinema.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<OnlineCinemaApplicationUser> GetAll();
        OnlineCinemaApplicationUser Get(string id);
        void Insert(OnlineCinemaApplicationUser entity);
        void Update(OnlineCinemaApplicationUser entity);
        void Delete(OnlineCinemaApplicationUser entity);
    }
}