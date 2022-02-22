using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using AppleStore.Enums;
using AppleStore.InputModels;
using AppleStore.Models;
using AppleStore.Models.Context;
using AppleStore.Models.Repositories;
using AppleStore.Services;
using AppleStore.Services.AutoMapperConfig;
using AppleStore.Services.Contracts;
using AppleStore.Tests.Infrastructure;
using AppleStore.ViewModels;

using AutoMapper;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace AppleStore.Tests.ServicesTests
{
    [TestFixture]
    public class AppleServiceTests
    {
        private Mock<DbSet<Apple>> appleMockSet;
        private Mock<DbSet<Discount>> discountMockSet;
        private Mock<AppleStoreDbContext> appleMockContext;
        private EfRepository<Apple> appleEfRepository;
        private EfRepository<Discount> discountEfRepository;
        private EfRepository<PurchasedApples> purchasedEfRepository;
        private SqlAppleData data;
        private IAppleService service;
        private Apple testApple;
        private Discount testDiscount;

        [SetUp]
        public void Setup()
        {
            this.appleMockContext = new Mock<AppleStoreDbContext>();
            
            this.testApple = new Apple() { Name = "test Apple", Id = "1apple" };
            this.testDiscount = new Discount() { AppleId = "1apple", Id = "1discount", Pairs = 2, NewPrice = 1800, Apple = this.testApple };

            var apples = new List<Apple> { testApple }.AsQueryable();
            var discounts = new List<Discount> { testDiscount }.AsQueryable();

            this.appleMockSet = TestHelpers.GetDbSetMock(apples);
            this.discountMockSet = TestHelpers.GetDbSetMock(discounts);

            SetupDbSets();

            this.appleEfRepository = new EfRepository<Apple>(this.appleMockContext.Object);
            this.discountEfRepository = new EfRepository<Discount>(this.appleMockContext.Object);
            this.purchasedEfRepository = new EfRepository<PurchasedApples>(this.appleMockContext.Object);
            this.data = new SqlAppleData(this.appleEfRepository, this.discountEfRepository);
            this.service = new AppleService(this.data, purchasedEfRepository);

            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        [TearDown]
        public void Dispose()
        {
            Mapper.Reset();
        }

        [Test]
        public void GetAll_ShouldReturnAListOfAppleViewModel()
        {
            const int expectedCount = 1;

            var result = this.service.GetAll();

            Assert.That(result, Is.InstanceOf<IEnumerable<AppleViewModel>>());
            Assert.That(expectedCount, Is.EqualTo(result.Count()));
        }

        [Test]
        [TestCase("1apple", "1user")]
        public void Get_ShouldReturnTheAppleViewModel(string id, string userId)
        {
            var expectedModel = new AppleViewModel() { Name = "test Apple", Id = id };

            var result = this.service.Get(id, userId);

            Assert.That(expectedModel.Id, Is.EqualTo(result.Id));
        }

        [Test]
        [TestCase("2apple", "1user")]
        public void GetWithNonExistingId_ShouldThrowKeyNotFoundException(string id, string userId)
        {
            try
            {
                this.service.Get(id, userId);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is KeyNotFoundException);
            }
        }

        [Test]
        public void Add_ShouldAddCorrectly()
        {
            var formModel = new AppleInputModel()
            {
                Name = "test2 Apple", 
                Id = "2apple", 
                Type = AppleType.Bitter, 
                Description = "test Desc",
                ImageUrl = "test Img", Price = 444
            };

            this.service.Add(formModel);

            this.appleMockSet.Verify(m => m.Add(It.IsAny<Apple>()), Times.Once());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [Test]
        public void Add_ShouldNotAdd_WithInvalidName()
        {
            var formModel = new AppleInputModel() { Name = "", Id = "2apple", Type = AppleType.Bitter, Description = "test Desc", ImageUrl = "test Img", Price = 444 };

            this.service.Add(formModel);

            this.appleMockSet.Verify(m => m.Add(It.IsAny<Apple>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void Add_ShouldNotAdd_WithInvalidImageUrl()
        {
            var formModel = new AppleInputModel() { Name = "apple2", Id = "2apple", Type = AppleType.Bitter, Description = "test Desc", ImageUrl = "", Price = 444 };

            this.service.Add(formModel);

            this.appleMockSet.Verify(m => m.Add(It.IsAny<Apple>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void Add_ShouldNotAdd_WithDescriptionLengthAboveTheLimit()
        {
            var descriptionAboveLimit = TestHelpers.RandomString(665);

            var formModel = new AppleInputModel() { Name = "apple2", Id = "2apple", Type = AppleType.Bitter, Description = descriptionAboveLimit, ImageUrl = "", Price = 444 };

            this.service.Add(formModel);

            this.appleMockSet.Verify(m => m.Add(It.IsAny<Apple>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void Edit_ShouldEditCorrectly()
        {
            var editData = new Mock<SqlAppleData>(this.appleMockContext.Object);
            this.service = new AppleService(editData.Object, purchasedEfRepository);

            var formModel = new AppleInputModel()
            {
                Name = "test2 Apple",
                Id = "1apple", 
                Type = AppleType.Bitter, 
                Description = "test Desc",
                ImageUrl = "test Img", 
                Price = 444
            };

            editData
                .Setup(e => e.Get(formModel.Id))
                .Returns(this.testApple);

            editData
                .Protected().Setup<bool>("Modify", this.testApple, formModel)
                .Returns(true);

            this.service.Edit(formModel);
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void Edit_ShouldNotEdit_WithInvalidName()
        {
            var editData = new Mock<SqlAppleData>(this.appleMockContext.Object);
            this.service = new AppleService(editData.Object, purchasedEfRepository);

            var formModel = new AppleInputModel()
            {
                Name = "",
                Id = "2apple",
                Type = AppleType.Bitter,
                Description = "test Desc",
                ImageUrl = "test Img",
                Price = 444
            };

            editData
                .Setup(e => e.Get(formModel.Id))
                .Returns(this.testApple);

            editData
                .Protected().Setup<bool>("Modify", this.testApple, formModel)
                .Returns(true);

            this.service.Edit(formModel);

            editData.Protected().Verify("Modify", Times.Never(), ItExpr.IsAny<Apple>(), ItExpr.IsAny<AppleInputModel>());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void Edit_ShouldNotEdit_WithInvalidImageUrl()
        {
            var editData = new Mock<SqlAppleData>(this.appleMockContext.Object);
            this.service = new AppleService(editData.Object, purchasedEfRepository);

            var formModel = new AppleInputModel()
            {
                Name = "apple2", 
                Id = "2apple", 
                Type = AppleType.Bitter, 
                Description = "test Desc", 
                ImageUrl = "",
                Price = 444
            };

            this.service.Add(formModel);

            editData.Protected().Verify("Modify", Times.Never(), ItExpr.IsAny<Apple>(), ItExpr.IsAny<AppleInputModel>());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [Test]
        public void Edit_ShouldNotAdd_WithDescriptionLengthAboveTheLimit()
        {
            var descriptionAboveLimit = TestHelpers.RandomString(665);

            var formModel = new AppleInputModel() { Name = "apple2", Id = "2apple", Type = AppleType.Bitter, Description = descriptionAboveLimit, ImageUrl = "", Price = 444 };

            this.service.Add(formModel);

            this.appleMockSet.Verify(m => m.Add(It.IsAny<Apple>()), Times.Never());
            this.appleMockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        private void SetupDbSets()
        {
            this.appleMockSet
                .Setup(ds => ds.AsNoTracking())
                .Returns(appleMockSet.Object);

            this.discountMockSet
            .Setup(ds => ds.AsNoTracking())
            .Returns(discountMockSet.Object);

            this.appleMockContext
                .Setup(x => x.Apples)
                .Returns(appleMockSet.Object);

            this.appleMockContext
                .Setup(x => x.Discounts)
                .Returns(discountMockSet.Object);

            this.appleMockContext
               .Setup(x => x.Set<Apple>())
               .Returns(appleMockSet.Object);

            this.appleMockSet
               .Setup(x => x.Find(this.testApple.Id))
               .Returns(this.testApple);

            this.appleMockContext
               .Setup(x => x.Set<Discount>())
               .Returns(discountMockSet.Object);
        }
    }
}
