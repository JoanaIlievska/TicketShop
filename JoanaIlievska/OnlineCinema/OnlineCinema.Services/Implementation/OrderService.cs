using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Repository.Interface;
using OnlineCinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCinema.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public List<Order> getAllOrders()
        {
            return this._orderRepository.GetAll().ToList();
        }
        public Order getOrderDetails(Guid? Id)
        {
            return this._orderRepository.Get(Id);
        }
    }
}
