using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using AppleStore.Enums;
using AppleStore.InputModels;
using AppleStore.Models;
using AppleStore.Models.Context;
using AppleStore.Services;
using AppleStore.Services.Contracts;
using AppleStore.Tests.Infrastructure;
using Moq;
using NUnit.Framework;

namespace AppleStore.Tests.ServicesTests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<DbSet<Cart>> cartMockSet;
        private Mock<DbSet<ApplicationUser>> userMockSet;
        private Mock<AppleStoreDbContext> appleMockContext;
        private Mock<DbSet<PurchasedApples>> purhcasedMockSet;
        private Mock<DbSet<Apple>> applesMockSet;
        private Mock<DbSet<Discount>> discounstMockSet;

        private ICartService service;
        private Cart testCart;
        private ApplicationUser testUser;
        private PurchasedApples testPurchase;
        private Apple testApple;
        private Discount testDiscount;

        [SetUp]
        public void Setup()
        {
            this.appleMockContext = new Mock<AppleStoreDbContext>();

            this.testUser = new ApplicationUser() { Id = "1user"};
            this.testApple = new Apple() { Name = "test Apple", Id = "1apple", Type = AppleType.Bitter, Description = "testDesc", Price = 23, ImageUrl = "testImg/"};
            this.testPurchase = new PurchasedApples() { AppleId = "1apple", CartId = "1cart", Apple = testApple, Cart = testCart, Count = 2, Id = "1purchasedTest"};
            this.testDiscount = new Discount() { Apple = testApple, AppleId = testApple.Id, NewPrice = 230, Pairs = 2};

            var users = new List<ApplicationUser> { testUser }
                .AsQueryable();
            var purchasedTests = new List<PurchasedApples> { testPurchase }
                .AsQueryable();
            var applesTests = new List<Apple> { testApple }
                .AsQueryable();
            var discountsTests = new List<Discount> { testDiscount }
                .AsQueryable();

            this.testCart = new Cart() { Total = 10, Id = "1cart", UserId = "1user", PurchasedApplesList = purchasedTests.ToList() };

            var carts = new List<Cart> { testCart }
                .AsQueryable();

            this.cartMockSet = TestHelpers.GetDbSetMock(carts);
            this.userMockSet = TestHelpers.GetDbSetMock(users);
            this.purhcasedMockSet = TestHelpers.GetDbSetMock(purchasedTests);
            this.applesMockSet = TestHelpers.GetDbSetMock(applesTests);
            this.discounstMockSet = TestHelpers.GetDbSetMock(discountsTests);

            this.appleMockContext
                .Setup(x => x.Carts)
                .Returns(cartMockSet.Object);
            this.appleMockContext
                .Setup(c => c.Users)
                .Returns(this.userMockSet.Object);
            this.appleMockContext
                .Setup(c => c.PurchasedApples)
                .Returns(this.purhcasedMockSet.Object);
            this.appleMockContext
                .Setup(c => c.Apples)
                .Returns(this.applesMockSet.Object);
            this.appleMockContext
                .Setup(c => c.Discounts)
                .Returns(this.discounstMockSet.Object);

            //this.service = new CartService(this.appleMockContext.Object);
        }



        [Test]
        public void GetAllPurchased_ShouldReturn_CartListAllPurchasedFormModelWithCorrectData()
        {
            const int expectedCount = 1;

            var result = this.service.GetAllPurchased(this.testUser.Id);

            Assert.That(result, Is.InstanceOf<CartList_All_PurchasedInputModel>());
            Assert.That(result.AllPurchased, Is.InstanceOf<ICollection<CartListPurchasedAppleFormModel>>());
            Assert.That(expectedCount, Is.EqualTo(result.AllPurchased.Count()));
        }

        [Test]
        public void GetAllPurchased_ShouldNotReturn_WithWrongUserIdData()
        {
            Assert.That(() => this.service.GetAllPurchased(this.testUser.Id + "test"),
                Throws.InstanceOf<KeyNotFoundException>());
        }
        
        [Test]
        public void Create_ShouldCreateANewCartInTheDbRelatedToUser()
        {
            this.service.Create(this.testUser.Id);
            
            this.cartMockSet.Verify(m=> m.Add(It.IsAny<Cart>()), Times.Once());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void CreateWithWrongUserId_ShouldNotCreateANewCart_AndThrowExc()
        {
            Assert.That(() => this.service.Create(this.testUser.Id + "test"), 
                Throws.InstanceOf<KeyNotFoundException>());

            this.cartMockSet.Verify(m => m.Add(It.IsAny<Cart>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void Remove_ShouldRemoveThePurchasedAppleInCart_WithCorrectData()
        {
            var purchasedAppleInCart = this.purhcasedMockSet.Object.FirstOrDefault();
            if (purchasedAppleInCart is null)
                throw new KeyNotFoundException(nameof(purchasedAppleInCart));

            this.service.Remove(purchasedAppleInCart.Id);

            this.purhcasedMockSet.Verify(m => m.Remove(It.IsAny<PurchasedApples>()), Times.Once());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void RemoveWithWrongPurchasedAppleId_ShouldNotRemove_AndThrowExc()
        {
            var purchasedAppleInCart = this.purhcasedMockSet.Object.FirstOrDefault();
            if (purchasedAppleInCart is null)
                throw new KeyNotFoundException(nameof(purchasedAppleInCart));
            
            Assert.That(() => this.service.Remove(purchasedAppleInCart.Id+"test"),
                Throws.InstanceOf<KeyNotFoundException>());

            this.purhcasedMockSet.Verify(m => m.Remove(It.IsAny<PurchasedApples>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void AddApple_ShouldAddANewPurchasedAppleInTheDbRelatedToTheCart()
        {
            var expectedCount = 2;
            var purchasedAppleTest2 = new AddAppleInputModel()
            {
                UserId = testUser.Id,
                AppleId = this.testApple.Id,
                Count = 7
            };

            this.service.AddApple(purchasedAppleTest2);

            var total = this.testCart.PurchasedApplesList.Count();
            var result = this.testCart.PurchasedApplesList.FirstOrDefault(pa=> pa.Count== purchasedAppleTest2.Count);

            Assert.That(expectedCount, Is.EqualTo(total));
            Assert.That(result, Is.Not.EqualTo(null));
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void AddAppleWithWrongUserId_ShouldNotAddANewPurchasedApple_AndThrowExc()
        {
            var purchasedAppleTest3 = new AddAppleInputModel()
            {
                UserId = testUser.Id+"test",
                AppleId = this.testApple.Id,
                Count = 3
            };

            Assert.That(() => this.service.AddApple(purchasedAppleTest3),
                Throws.InstanceOf<KeyNotFoundException>());

            this.purhcasedMockSet.Verify(m => m.Add(It.IsAny<PurchasedApples>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void AddAppleWithWrongAppleId_ShouldNotAddANewPurchasedApple_AndThrowExc()
        {
            var purchasedAppleTest3 = new AddAppleInputModel()
            {
                UserId = testUser.Id,
                AppleId = this.testApple.Id + "test",
                Count = 4
            };

            Assert.That(() => this.service.AddApple(purchasedAppleTest3),
                Throws.InstanceOf<KeyNotFoundException>());

            this.purhcasedMockSet.Verify(m => m.Add(It.IsAny<PurchasedApples>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }
    }
}
