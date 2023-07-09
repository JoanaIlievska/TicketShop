using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Domain.DTO;
using OnlineCinema.Services.Interface;


namespace OnlineCinema.Web.Controllers
{
    public class TicketsController : Controller
    {
        
       
            private readonly IProductService _productService;
            private readonly ILogger<TicketsController> _logger;

            public TicketsController(ILogger<TicketsController> logger, IProductService productService)
            {
                _logger = logger;
                _productService = productService;
            }

        // GET: Products
        public IActionResult Index()
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;
                DateTime currentDateTimeWithoutSeconds = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, currentDateTime.Minute, 0);

                var validTickets = this._productService.GetAllProducts()
                    .Where(ticket => ticket.movietime >= currentDateTimeWithoutSeconds)
                    .ToList();

                return View(validTickets);
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }

            // GET: Products/Details/5
            public IActionResult Details(Guid? id)
            {
                _logger.LogInformation("User Request -> Get Details For Product");
                if (id == null)
                {
                    return NotFound();
                }

                var product = this._productService.GetDetailsForProduct(id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }

            // GET: Products/Create
            public IActionResult Create()
            {
                _logger.LogInformation("User Request -> Get create form for Product!");
                return View();
            }

            // POST: Products/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create([Bind("Id,ProductName,ProductImage,ProductDescription,ProductPrice,ProductRating")] Ticket product)
            {
                _logger.LogInformation("User Request -> Inser Product in DataBase!");
                if (ModelState.IsValid)
                {
                    product.Id = Guid.NewGuid();
                    this._productService.CreateNewProduct(product);
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }

            // GET: Products/Edit/5
            public IActionResult Edit(Guid? id)
            {
                _logger.LogInformation("User Request -> Get edit form for Product!");
                if (id == null)
                {
                    return NotFound();
                }

                var product = this._productService.GetDetailsForProduct(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }

            // POST: Products/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Guid id, [Bind("Id,ProductName,ProductImage,ProductDescription,ProductPrice,ProductRating")] Ticket product)
            {
                _logger.LogInformation("User Request -> Update Product in DataBase!");

                if (id != product.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        this._productService.UpdeteExistingProduct(product);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(product.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }

            // GET: Products/Delete/5
            public IActionResult Delete(Guid? id)
            {
                _logger.LogInformation("User Request -> Get delete form for Product!");

                if (id == null)
                {
                    return NotFound();
                }

                var product = this._productService.GetDetailsForProduct(id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }

            // POST: Products/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(Guid id)
            {
                _logger.LogInformation("User Request -> Delete Product in DataBase!");

                this._productService.DeleteProduct(id);
                return RedirectToAction(nameof(Index));
            }


            public IActionResult AddToShoppingCart(Guid id)
            {
                var result = this._productService.GetShoppingCartInfo(id);

                return View(result);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult AddToShoppingCart(AddToShoppingCartDto model)
            {

                _logger.LogInformation("User Request -> Add Product in ShoppingCart and save changes in database!");


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = this._productService.AddToShoppingCart(model, userId);

                if (result)
                {
                    return RedirectToAction("Index", "Tickets");
                }
                return View(model);
            }
            private bool ProductExists(Guid id)
            {
                return this._productService.GetDetailsForProduct(id) != null;
            }
        }
    }
