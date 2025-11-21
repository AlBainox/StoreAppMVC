namespace SportsStoreTests
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Moq;
    using SportsStore.Components;
    using SportsStore.Models;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class NavigationMenuViewComponentsTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange - create the mock repository
            Mock<IStoreRepository> mock = new();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }).AsQueryable<Product>());

            // Arrange - create the view component
            NavigationMenuViewComponent target =
                new(mock.Object);

            // Act = get the set of categories
            string[] results =
                ((IEnumerable<string>?)(target.Invoke()
                as ViewViewComponentResult)?.ViewData?.Model
                ?? Enumerable.Empty<string>()).ToArray();

            // Assert
            Assert.Equal(new string[] { "Apples", "Oranges", "Plums" }, results);
        }
        [Fact]
        public void Indicates_Selected_Category()
        {
            // Arrange - create the mock repository
            Mock<IStoreRepository> mock = new();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }).AsQueryable<Product>());

            // Arrange - create the view component
            NavigationMenuViewComponent target =
                new(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }

            };

            // Arrange - define the category to be selected 
            string categoryToSelect = "Apples";          
            target.RouteData.Values["category"] = categoryToSelect;

            // Act
            string? result = (string?)(target.Invoke()
                as ViewViewComponentResult)?.ViewData?["SelectedCategory"];

            // Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}