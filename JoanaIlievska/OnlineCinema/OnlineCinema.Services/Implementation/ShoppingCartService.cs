using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Domain.DTO;
using OnlineCinema.Repository.Interface;
using OnlineCinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace OnlineCinema.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Email> _mailRepository;
        private readonly IRepository<TicketInOrder> _productInOrderRepository;
        private readonly IRepository<Order> _orderRepository;


        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> productInOrderRepository, IRepository<Email> mailRepository)
        {
            _mailRepository = mailRepository;
            _productInOrderRepository = productInOrderRepository;
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
         
        }


        public bool deleteProductFromSoppingCart(string userId, Guid productId)
        {
            if (!string.IsNullOrEmpty(userId) && productId != null)
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.TicketsInShoppingCarts.Where(z => z.ProductId.Equals(productId)).FirstOrDefault();

                userShoppingCart.TicketsInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);

                var userCard = loggedInUser.UserCart;

                var allProducts = userCard.TicketsInShoppingCarts.ToList();

                var allProductPrices = allProducts.Select(z => new
                {
                    ProductPrice = z.Product.ProductPrice,
                    Quantity = z.Quantity
                }).ToList();

                double totalPrice = 0.0;

                foreach (var item in allProductPrices)
                {
                    totalPrice += item.Quantity * item.ProductPrice;
                }

                var reuslt = new ShoppingCartDto
                {
                    Products = allProducts,
                    TotalPrice = totalPrice
                };

                return reuslt;
            }
            return new ShoppingCartDto();
        }

        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userCard = loggedInUser.UserCart;

                Email mail = new Email();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Успешно направена нарачка!";
                mail.Status = false;


                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<TicketInOrder> productInOrders = new List<TicketInOrder>();

                var result = userCard.TicketsInShoppingCarts.Select(z => new TicketInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.Product.Id,
                    OrderedProduct = z.Product,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder content = new StringBuilder();

                var totalPrice = 0.0;

                content.AppendLine("Вашата нарачка се процесира. Нарачката ги содржи следниве карти: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var currentItem = result[i - 1];
                    totalPrice += currentItem.Quantity * currentItem.OrderedProduct.ProductPrice;
                    content.AppendLine(i.ToString() + ". " + currentItem.OrderedProduct.ProductName + " со : " + currentItem.Quantity + " карти со цена од" + currentItem.OrderedProduct.ProductPrice +"ден.");
                }
                content.AppendLine();
                content.AppendLine("Вкупна цена: " + totalPrice.ToString());

                mail.Content = content.ToString();


                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.TicketsInShoppingCarts.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
            }

            return false;
        }
    }
}
