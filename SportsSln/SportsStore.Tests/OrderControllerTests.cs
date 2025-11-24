using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;
namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            OrderController controller = new OrderController(mock.Object, cart);

            // Act
            IActionResult result = controller.Checkout(new Order());

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Checkout");
            Assert.False(viewResult.ViewData.ModelState.IsValid);
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController controller = new OrderController(mock.Object, cart);
            controller.ModelState.AddModelError("error", "error");

            // Act
            IActionResult result = controller.Checkout(new Order());
            
            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Checkout");
            Assert.False(viewResult.ViewData.ModelState.IsValid);
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController controller = new OrderController(mock.Object, cart);

            // Act
            IActionResult result = controller.Checkout(new Order());

            // Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Completed", redirectResult.ActionName);
        }
    }
}