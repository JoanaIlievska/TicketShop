using Nest;
using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Domain.DTO;
using OnlineCinema.Repository.Implementation;
using OnlineCinema.Services.Interface;
using OnlineCinema.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OnlineCinema.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly OnlineCinema.Repository.Interface.IRepository<Ticket> _productRepository;
        private readonly OnlineCinema.Repository.Interface.IRepository<TicketsInShoppingCarts> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;


        public ProductService(OnlineCinema.Repository.Interface.IRepository<Ticket> productRepository, IUserRepository userRepository, OnlineCinema.Repository.Interface.IRepository<TicketsInShoppingCarts> productInShoppingCartRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;



        }

        public OnlineCinema.Repository.Interface.IRepository<Ticket> ProductRepository => _productRepository;

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item.ProductId != null && userShoppingCard != null)
            {
                var product = this.GetDetailsForProduct(item.ProductId);
                //{896c1325-a1bb-4595-92d8-08da077402fc}

                if (product != null)
                {
                    TicketsInShoppingCarts itemToAdd = new TicketsInShoppingCarts
                    {
                        Id = Guid.NewGuid(),
                        Product = product,
                        ProductId = product.Id,
                        UserCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };

                    var existing = userShoppingCard.TicketsInShoppingCarts.Where(z => z.ShoppingCartId == userShoppingCard.Id && z.ProductId == itemToAdd.ProductId).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.Quantity += itemToAdd.Quantity;
                        this._productInShoppingCartRepository.Update(existing);

                    }
                    else
                    {
                        this._productInShoppingCartRepository.Insert(itemToAdd);
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewProduct(Ticket p)
        {
            this.ProductRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = this.GetDetailsForProduct(id);
            this.ProductRepository.Delete(product);
        }

        public List<Ticket> GetAllProducts()
        {
            return this.ProductRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForProduct(Guid? id)
        {
            return this.ProductRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForProduct(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedProduct = product,
                ProductId = product.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdeteExistingProduct(Ticket p)
        {
            this.ProductRepository.Update(p);
        }
    }
}
