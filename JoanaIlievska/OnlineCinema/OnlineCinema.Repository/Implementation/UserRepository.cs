using Microsoft.EntityFrameworkCore;
using OnlineCinema.Domain.Identity;
using OnlineCinema.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCinema.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<OnlineCinemaApplicationUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<OnlineCinemaApplicationUser>();
        }
        public IEnumerable<OnlineCinemaApplicationUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public OnlineCinemaApplicationUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.TicketsInShoppingCarts")
               .Include("UserCart.TicketsInShoppingCarts.Product")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(OnlineCinemaApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(OnlineCinemaApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(OnlineCinemaApplicationUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
