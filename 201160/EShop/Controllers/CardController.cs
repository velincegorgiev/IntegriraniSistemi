
using EShop.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class CardController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public CardController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }
        public IActionResult DeleteFromCard(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result=this._shoppingCartService.deleteProductFromSoppingCart(id, userId);

            if (result)
            {
                return RedirectToAction("Index", "Card");
            }
            else
            {
                return RedirectToAction("Index", "Card");
            }
        }
        public IActionResult Order()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.order(userId);
            if(result)
            {
                return RedirectToAction("Index", "Card");
            }
            else
            {
                return RedirectToAction("Index", "Card");
            }
        }
    }
}
