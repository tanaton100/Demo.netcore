
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Demo.Models;
using Demo.Repository;
using Demo.Services;
using Moq;
using NUnit.Framework;

namespace UnitTest.Services
{
    [TestFixture]
    class ProductServiceTest
    {
        private AutoMock _autoMock;
        private IProductService _productService;

        [SetUp]
        public void Setup()
        {
            _autoMock = AutoMock.GetLoose();
            _productService = _autoMock.Create<ProductService>();
        }

        [TearDown]
        public void TearDown()
        {
            _autoMock.Dispose();
        }

        [Test]
        public void Given_WhenGetAll_ThenReturnDetail()
        {
            var expected = GetProduct().Count();
            _autoMock.Mock<IProductRepository>().Setup(method => method.GetAll()).Returns(GetProduct());
            var result = _productService.GetAll();
            Assert.NotNull(result);
            Assert.AreEqual(result.Count(), expected);
        }

        [Test]
        public void Given_WhenGetById_ThenReturnDetail()
        {
            var productId = 1;
            _autoMock.Mock<IProductRepository>().Setup(method => method.FindById(It.IsAny<int>())).Returns((int id)=>GetProduct().FirstOrDefault(_ =>_.Id==id));
            var result = _productService.FindById(productId);
            var expected = GetProduct().FirstOrDefault(_ =>_.Id == productId);
            Assert.NotNull(result);
            Assert.AreEqual(result.Name, expected?.Name);
        }


        private IEnumerable<Products> GetProduct()
        {
            yield return new Products { Id = 1,Name = "P1",Price = 222};
            yield return new Products { Id = 2, Name = "P2", Price = 322 };
            yield return new Products { Id = 3, Name = "P4", Price = 422 };
        }
    }
}
