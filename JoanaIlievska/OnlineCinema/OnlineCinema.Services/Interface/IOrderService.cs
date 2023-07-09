using OnlineCinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCinema.Services.Interface
{
    public interface IOrderService
    {
        public List<Order> getAllOrders();
        public Order getOrderDetails(Guid? Id);
    }
}
