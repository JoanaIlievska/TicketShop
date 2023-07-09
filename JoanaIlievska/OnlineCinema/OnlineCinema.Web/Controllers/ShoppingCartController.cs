using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using OnlineCinema.Domain.DTO;
using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Repository;
using OnlineCinema.Services.Interface;
using GemBox.Document;
using System.IO;
using System.Text;
using Stripe;

namespace OnlineCinema.Web.Controllers
{
    public class ShoppingCartController : Controller
    {


        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.deleteProductFromSoppingCart(userId, id);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
        }

        public Boolean Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.order(userId);

            return result;
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._shoppingCartService.getShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Плаќање на карти за филм",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.Order();

                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult Faktura()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.getShoppingCartInfo(userId);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.DOCX");

            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", new Guid().ToString());
            document.Content.Replace("{{UserName}}", User.Identity.Name);

            StringBuilder sb = new StringBuilder();

            var total = 0.0;

            foreach (var item in result.Products)
            {
                total += item.Quantity * item.Product.ProductPrice;
                sb.AppendLine(item.Product.ProductName + " со количина: " + item.Quantity + " и цена : " + item.Product.ProductPrice + "ден.");
            }

            document.Content.Replace("{{ProductList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", total.ToString() + "ден") ;

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");


        }


    }
}
