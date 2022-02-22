using System.Threading.Tasks;
using System.Web.Mvc;
using AppleStore.Contracts.Services;
using AppleStore.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppleStore.Web.Controllers;


namespace AppleStore.Tests.ControllersTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private IAppleData db;
        private HomeController controller;
        public HomeControllerTests()
        {
            this.db = new InMemoryAppleData();
            this.controller = new HomeController(this.db);
        }

        [TestMethod]
        public void IndexReturnsView()
        {
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ContactsReturnsView()
        {
            var result = controller.Contact() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
