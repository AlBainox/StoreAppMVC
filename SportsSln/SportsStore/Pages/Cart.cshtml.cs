using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Infrastructure;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        IStoreRepository repository;      

        public CartModel(IStoreRepository repo)
        {
           repository = repo;
        }

        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            ReturnUrl = returnUrl ?? "/";
        }
        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("Cart", Cart);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}