using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SportsStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            // Arrange - create a mock repository
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                p1, p2
            }).AsQueryable<Product>());

            // Arrange - create a Cart
            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            // Arrange - create the a mock page context and session
            Mock<ISession> mockHttpContext = new Mock<ISession>();
            byte[] cartData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cart));
            mockHttpContext.Setup(s => s.TryGetValue(It.IsAny<string>(), out cartData));
        
            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(c => c.Session).Returns(mockHttpContext.Object);
            
            // Action
            CartModel cartModel = new CartModel(mock.Object)
            {
                PageContext = new PageContext(new ActionContext()
                {
                    HttpContext = mockContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new PageActionDescriptor()
                })
            };
            cartModel.OnGet("myUrl");

            // Assert
            Assert.Equal(2, cartModel.Cart?.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }
        [Fact]
        public void Can_Update_Cart()
        {
            // Arrange - create a mock repository
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                p1, p2
            }).AsQueryable<Product>());

            Cart cart = new Cart();

            // Arrange - create a mock page context and session
            Mock<ISession> mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, val) =>
            {
                cart = JsonSerializer.Deserialize<Cart>(Encoding.UTF8.GetString(val))!;
            });

            Mock<HttpContext> mockContext = new Mock<HttpContext>();
            mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);

            // Action
            CartModel cartModel = new CartModel(mock.Object)
            {
                PageContext = new PageContext(new ActionContext()
                {
                    HttpContext = mockContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new PageActionDescriptor()
                })
            };
            cartModel.OnPost(1, "myUrl");

            // Assert
            Assert.Single(cart.Lines);
            Assert.Equal(p1, cart.Lines.First().Product);
            Assert.Equal(1, cart.Lines.First().Quantity);
        }
    }
}