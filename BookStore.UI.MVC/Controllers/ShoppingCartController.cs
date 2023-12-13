using Microsoft.AspNetCore.Mvc;
using BookStore.DATA.EF.Models;//context
using BookStore.UI.MVC.Models;//CartItemViewModel
using Microsoft.AspNetCore.Identity;//UserManager
using Newtonsoft.Json;//To manage Session variables
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace BookStore.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Steps to Implement Session Based Shopping Cart
        /*
         * 1) Register Session in program.cs (builder.Services.AddSession... && app.UseSession())
         * 2) Create the CartItemViewModel class in [ProjName].UI.MVC/Models folder
         * 3) Add the 'Add To Cart' button in the Index and/or Details view of your Products
         * 4) Create the ShoppingCartController (empty controller -> named ShoppingCartController)
         *      - add using statements
         *          - using GadgetStore.DATA.EF.Models;
         *          - using Microsoft.AspNetCore.Identity;
         *          - using GadgetStore.UI.MVC.Models;
         *          - using Newtonsoft.Json;
         *      - Add props for the GadgetStoreContext && UserManager
         *      - Create a constructor for the controller - assign values to context && usermanager
         *      - Code the AddToCart() action
         *      - Code the Index() action
         *      - Code the Index View
         *          - Start with the basic table structure
         *          - Show the items that are easily accessible (like the properties from the model)
         *          - Calculate/show the lineTotal
         *          - Add the RemoveFromCart <a>
         *      - Code the RemoveFromCart() action
         *          - verify the button for RemoveFromCart in the Index view is coded with the controller/action/id
         *      - Add UpdateCart <form> to the Index View
         *      - Code the UpdateCart() action
         *      - Add Submit Order button to Index View
         *      - Code SubmitOrder() action
         * */
        #endregion

        private readonly BookStoreContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(BookStoreContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //retrieve the cart contents from session
            var sessionCart = HttpContext.Session.GetString("cart");

            //create the shell for the C# version of the cart
            Dictionary<int, CartItemViewModel>? shoppingCart;

            //check to see if the cart exists
            if (string.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new();
                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }
            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            #region Session Notes
            /*
             * Session is a tool available on the server-side that can store information for a user while they are actively using your site.
             * Typically the session lasts for 20 minutes (this can be adjusted in Program.cs).
             * Once the 20 minutes is up, the session variable is disposed.
             * 
             * Values that we can store in Session are limited to: string, int
             * - Because of this we have to get creative when trying to store complex objects (like CartItemViewModel).
             * To keep the info separated into properties we will convert the C# object to a JSON string.
             * */
            #endregion

            var sessionCart = HttpContext.Session.GetString("cart");

            //Empty shell for a LOCAL shopping cart variable
            Dictionary<int, CartItemViewModel> shoppingCart;

            if (string.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            //add newly selected items to the cart
            Book? book = _context.Books.Find(id);

            //Initialize a cart item so we can add to the cart.
            CartItemViewModel item = new(1, book);

            if (shoppingCart.ContainsKey(book.BookId))
            {
                //update the quantity
                shoppingCart[book.BookId].Qty++;
            }
            else
            {
                shoppingCart.Add(book.BookId, item);
            }

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            //Dry : var shoppingCart = GetCart();
            //retrieve the cart from session
            var jsonCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(jsonCart);

            shoppingCart.Remove(id);

            //Check if there are any other items in the cart. If not, remove the cart from session.
            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateCart(int bookId, int qty)
        {
            var jsonCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(jsonCart);

            //update the qty for our specific dictionary key.
            //if quantity is 0, remove the item from the cart.
            if (qty <= 0)
            {
                return RedirectToAction("RemoveFromCart", bookId);
            }
            else
            {
                shoppingCart[bookId].Qty = qty;
                jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");
        }

        [Authorize]

        public async Task<IActionResult> SubmitOrder()
        {
            #region Planning out Order Submission
            // BIG PICTURE PLAN
            // Create Order object -> then save to the DB
            // - OrderDate
            // - UserId
            // - ShipToName, ShipCity, ShipState, ShipZip --> this info needs to be pulled from the UserDetails record.
            // Alternatively, use a checkout screen and a Create Orders template.
            // Add the record to _context
            // Save DB changes

            // Create OrderProducts object for each item in the cart
            // - ProductId -> available from cart
            // - OrderId -> from Order object
            // - Qty -> available from cart
            // - ProductPrice -> available from cart
            // Add the record to _context
            // Save DB changes
            #endregion
            // Retrieve the current user's ID
            var buyerId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            // Retrieve the UserDetails for that user
            var ud = _context.Buyers.Find(buyerId);
            if (ud == null)
            {
                var newUd = new Buyer()
                {
                    BuyerId = buyerId,
                    BuyerFname = "Default",
                    BuyerLname = "Name",
                };
                _context.Add(newUd);
                ud = newUd;
            }

            // Create the order object and assign values (either from user details or from your checkout form submission.)
            Order o = new()
            {
                OrderDate = DateTime.Now,
                BuyerId = buyerId,
                ShipCity = ud?.City ?? "Not Given",
                ShipToName = ud?.FullName ?? "Not Given",
                ShipState = ud?.State ?? "NO",
                ShipPostalCode = ud?.PostalCode ?? "[N/A]"
            };

            _context.Add(o);

            // Retrieve the session cart
            var jsonCart = HttpContext.Session.GetString("cart");

            // Check if the cart is null or empty
            if (string.IsNullOrEmpty(jsonCart))
            {
                // Handle the case when the cart is empty or not present
                // You may want to redirect to the cart page or display an error message
                return RedirectToAction("Index");
            }

            try
            {
                // Deserialize the session cart data
                var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(jsonCart);

                foreach (var item in shoppingCart.Values)
                {
                    // Create an OrderProduct object for each item in the cart
                    OrderBook ob = new OrderBook()
                    {
                        OrderId = o.OrderId,
                        BookId = item.Book.BookId,
                        BookPrice = item.Book.BookPrice,
                        Quantity = (short)item.Qty
                    };

                    o.OrderBooks.Add(ob);
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("cart");
                return RedirectToAction("Index", "Orders");
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, redirect to an error page, etc.)
                // For now, I'm just logging it to the console
                Console.WriteLine($"Error during order submission: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> SubmitOrder()
        //{
        //    #region Planning out Order Submission
        //    //BIG PICTURE PLAN
        //    //Create Order object -> then save to the DB
        //    //  - OrderDate
        //    //  - UserId
        //    //  - ShipToName, ShipCity, ShipState, ShipZip --> this info needs to be pulled from the UserDetails record.
        //    //  Alternatively, use a checkout screen and a Create Orders template.
        //    //Add the record to _context
        //    //Save DB changes

        //    //Create OrderProducts object for each item in the cart
        //    //  - ProductId -> available from cart
        //    //  - OrderId -> from Order object
        //    //  - Qty -> available from cart
        //    //  - ProductPrice -> available from cart
        //    //Add the record to _context
        //    //Save DB changes
        //    #endregion
        //    //retrieve the current user's ID
        //    var buyerId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

        //    //retrieve the UserDetails for that user
        //    var ud = _context.Buyers.Find(buyerId);
        //    if (ud == null)
        //    {
        //        var newUd = new Buyer()
        //        {
        //            BuyerId = buyerId,
        //            BuyerFname = "Default",
        //            BuyerLname = "Name",
        //        };
        //        _context.Add(newUd);
        //        ud = newUd;
        //    }

        //    //Create the order object and assign values (either from user details or from your checkout form submission.)
        //    Order o = new()
        //    {
        //        OrderDate = DateTime.Now,
        //        BuyerId = buyerId,
        //        ShipCity = ud?.City ?? "Not Given",
        //        ShipToName = ud?.FullName ?? "Not Given",
        //        ShipState = ud?.State ?? "NO",
        //        ShipPostalCode = ud?.PostalCode ?? "[N/A]"
        //    };

        //    _context.Add(o);

        //    //Retrieve the session cart
        //    var jsonCart = HttpContext.Session.GetString("cart");
        //    var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(jsonCart);

        //    foreach (var item in shoppingCart.Values)
        //    {
        //        //create an OrderProduct object for each item in the cart
        //        OrderBook ob = new OrderBook()
        //        {
        //            OrderId = o.OrderId,
        //            BookId = item.Book.BookId,
        //            BookPrice = item.Book.BookPrice,
        //            Quantity = (short)item.Qty
        //        };

        //        o.OrderBooks.Add(ob);
        //    }
        //    await _context.SaveChangesAsync();
        //    HttpContext.Session.Remove("cart");
        //    return RedirectToAction("Index", "Orders");
        //}

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_context.Buyers.Any(ud => ud.BuyerId == buyerId))
            {
                var newUd = new Buyer()
                {
                    BuyerId = buyerId,
                    BuyerFname = cvm.BuyerFname,
                    BuyerLname = cvm.BuyerLname,
                };
                _context.Add(newUd);
            }

            Order o = new()
            {
                OrderDate = DateTime.Now,
                BuyerId = buyerId,
                ShipCity = cvm.City,
                ShipToName = cvm.BuyerFname + " " + cvm.BuyerLname,
                ShipState = cvm.State,
                ShipPostalCode = cvm.PostalCode
            };

            _context.Orders.Add(o);

            //Retrieve the session cart
            var jsonCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(jsonCart);

            foreach (var item in shoppingCart.Values)
            {
                //create an OrderProduct object for each item in the cart
                OrderBook ob = new OrderBook()
                {
                    OrderId = o.OrderId,
                    BookId = item.Book.BookId,
                    BookPrice = item.Book.BookPrice,
                    Quantity = (short)item.Qty
                };

                o.OrderBooks.Add(ob);
            }
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("cart");
            return RedirectToAction("Index", "Orders");
        }
    }
}

