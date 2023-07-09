using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Services.Interface
{
    public interface IShoppingCartService
    {
        CardDto getShoppingCartInfo(string userId);
        bool deleteProductFromSoppingCart(Guid id, string userId);
        bool order(string userId);
    }
}
