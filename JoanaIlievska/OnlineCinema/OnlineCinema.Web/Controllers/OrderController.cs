using Microsoft.AspNetCore.Mvc;
using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Services.Interface;
using System;
using System.Linq;
using System.Security.Claims;

namespace OnlineCinema.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public IActionResult Index()
        {


            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Replace this with your actual way of getting the logged-in user's ID

            // Filter orders based on the logged-in user's ID
            var userOrders = orderService.getAllOrders()
                .Where(order => order.UserId == loggedInUserId)
                .ToList();

            return View(userOrders);



        }

        public IActionResult Details(Guid id)
        {
            try
            {
                var order = orderService.getOrderDetails(id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }
            catch (Exception ex)
            {
                // Handle exception and return appropriate response
                return View("Error");
            }
        }
    }
}
