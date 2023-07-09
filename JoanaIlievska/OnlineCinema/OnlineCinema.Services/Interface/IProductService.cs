using OnlineCinema.Domain.DomainModels;
using OnlineCinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCinema.Services.Interface
{
    public interface IProductService
    {

        List<Ticket> GetAllProducts();
        Ticket GetDetailsForProduct(Guid? id);
        void CreateNewProduct(Ticket p);
        void UpdeteExistingProduct(Ticket p);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteProduct(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
