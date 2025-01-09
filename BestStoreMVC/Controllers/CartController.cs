using BestStoreMVC.Models;
using BestStoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BestStoreMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly decimal shippingFee;

        public CartController(ApplicationDbContext context, IConfiguration configuration
            , UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.configuration = configuration;
            this.userManager = userManager;
            shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
        }
        public IActionResult Index()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            decimal subtotal = CartHelper.GetSubTotal(cartItems);

            ViewBag.CartItems = cartItems;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal + shippingFee;
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(CheckoutDto checkOut)
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            decimal subtotal = CartHelper.GetSubTotal(cartItems);

            ViewBag.CartItems = cartItems;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal + shippingFee;

            if (!ModelState.IsValid)
            {
                return View(checkOut);
            }

            if(cartItems.Count == 0)
            {
                ViewBag.ErrorMessage = "Your cart is empty";
                return View(checkOut);
            }


            TempData["DeliveryAddress"] = checkOut.DeliveryAddress;
            TempData["PaymentMethod"] = checkOut.PaymentMethod;

            if(checkOut.PaymentMethod == "paypal" || checkOut.PaymentMethod == "credit_card")
            {
                return RedirectToAction("Index", "Checkout");
            }


            return RedirectToAction("Confirm");

        }

        public IActionResult Confirm()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            decimal total = CartHelper.GetSubTotal(cartItems) + shippingFee;
            int cartSize = 0;
            foreach(var item in cartItems)
            {
                cartSize += item.Quantity;
            }

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();
            if(cartSize ==0 || deliveryAddress.Length ==0 || paymentMethod.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DeliveryAddress = deliveryAddress;
            ViewBag.PaymentMethod = paymentMethod;
            ViewBag.Total = total;
            ViewBag.CartSize = cartSize;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Confirm(int any)
        {
            var cartItems = CartHelper.GetCartItems(Request,Response, context);

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();
            if (cartItems.Count == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var appUser = await userManager.GetUserAsync(User);
            if (appUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //save the order
            var order = new Order
            {
                ClientId = appUser.Id,
                Items = cartItems,
                ShippingFee = shippingFee,
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymentMethod,
                PaymentStatus = "pending",
                PaymentDetails = "",
                OrderStatus = "created",
                CreatedAt = DateTime.Now,
            };

            context.Orders.Add(order);
            context.SaveChanges();

            //delete the shopping cart cookie
            Response.Cookies.Delete("shopping_cart");
            ViewBag.SuccessMessage = "Order created successfully";
            return View();
        }
    }
}
