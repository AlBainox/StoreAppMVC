using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
         [BindNever]
         public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        [Required(ErrorMessage = "Please enter your name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please enter your city")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Please enter your state")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Please enter your zip code")]
        public string? Zip { get; set; }

        [Required(ErrorMessage = "Please enter your country")]
        public string? Country { get; set; }       

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string? Email { get; set; }
        public bool GiftWrap { get; set; }       
    }
}