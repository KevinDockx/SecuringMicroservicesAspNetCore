using GloboTicket.Web.Extensions;
using GloboTicket.Web.Models;
using GloboTicket.Web.Models.Api;
using GloboTicket.Web.Models.View;
using GloboTicket.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GloboTicket.Web.Controllers
{
    public class ShoppingBasketController : Controller
    {
        private readonly IShoppingBasketService basketService;
        private readonly Settings settings;

        public ShoppingBasketController(IShoppingBasketService basketService, 
            Settings settings)
        {
            this.basketService = basketService;
            this.settings = settings; 
        }

        public async Task<IActionResult> Index()
        {
            var basketViewModel = await CreateBasketViewModel();

            return View(basketViewModel);
        }

        private async Task<BasketViewModel> CreateBasketViewModel()
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            Basket basket = await basketService.GetBasket(basketId);

            var basketLines = await basketService.GetLinesForBasket(basketId);

            var lineViewModels = basketLines.Select(bl => new BasketLineViewModel
            {
                LineId = bl.BasketLineId,
                EventId = bl.EventId,
                EventName = bl.Event.Name,
                Date = bl.Event.Date,
                Price = bl.Price,
                Quantity = bl.TicketAmount
            });


            var basketViewModel = new BasketViewModel
            {
                BasketLines = lineViewModels.ToList()
            };
             
            basketViewModel.ShoppingCartTotal = basketViewModel.BasketLines.Sum(bl => bl.Price * bl.Quantity);

            return basketViewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLine(BasketLineForCreation basketLine)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            var newLine = await basketService.AddToBasket(basketId, basketLine);
            Response.Cookies.Append(settings.BasketIdCookieName, newLine.BasketId.ToString());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLine(BasketLineForUpdate basketLineUpdate)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.UpdateLine(basketId, basketLineUpdate);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveLine(Guid lineId)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.RemoveLine(basketId, lineId);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(BasketCheckoutViewModel basketCheckoutViewModel)
        {
            try
            {
                var basketId = Request.Cookies.GetCurrentBasketId(settings);
                if (ModelState.IsValid)
                {
                    var basketForCheckout = new BasketForCheckout
                    {
                        FirstName = basketCheckoutViewModel.FirstName,
                        LastName = basketCheckoutViewModel.LastName,
                        Email = basketCheckoutViewModel.Email,
                        Address = basketCheckoutViewModel.Address,
                        ZipCode = basketCheckoutViewModel.ZipCode,
                        City = basketCheckoutViewModel.City,
                        Country = basketCheckoutViewModel.Country,
                        CardNumber = basketCheckoutViewModel.CardNumber,
                        CardName = basketCheckoutViewModel.CardName,
                        CardExpiration = basketCheckoutViewModel.CardExpiration,
                        CvvCode = basketCheckoutViewModel.CvvCode,
                        BasketId = basketId,
                        UserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value)
                    };

                    await basketService.Checkout(basketCheckoutViewModel.BasketId, basketForCheckout);

                    return RedirectToAction("CheckoutComplete");
                }

                return View(basketCheckoutViewModel);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View(basketCheckoutViewModel);
            }
        } 

        public IActionResult CheckoutComplete()
        {
            return View();
        }
    }
}
