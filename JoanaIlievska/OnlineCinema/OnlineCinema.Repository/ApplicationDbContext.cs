using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

 namespace OnlineCinema.Repository
{
    public class ApplicationDbContext : IdentityDbContext<OnlineCinemaApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Email> Emails{ get; set; }
        public virtual DbSet<Ticket> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketsInShoppingCarts> TicketsInShoppingCarts { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrder { get; set; }
        public virtual DbSet<Order> Order { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<Ticket>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInOrder>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketsInShoppingCarts>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketsInShoppingCarts>()
                 .HasOne(z => z.Product)
                 .WithMany(z => z.TicketsInShoppingCarts)
                 .HasForeignKey(z => z.ProductId);

            builder.Entity<TicketsInShoppingCarts>()
                .HasOne(z => z.UserCart)
                .WithMany(z => z.TicketsInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ShoppingCart>()
                .HasOne<OnlineCinemaApplicationUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);


            builder.Entity<TicketInOrder>()
                .HasOne(z => z.OrderedProduct)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<TicketInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.OrderId);

        }
    }
}
