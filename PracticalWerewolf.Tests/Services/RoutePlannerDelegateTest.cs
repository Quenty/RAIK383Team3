using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalWerewolf.Controllers;
using PracticalWerewolf.Helpers;
using System.Device.Location;

namespace PracticalWerewolf.Tests.Services
{
    [TestClass]
    public class RoutePlannerDelegateTest
    {

        private static string email = "jesseelzhang@gmail.com";

        [TestMethod]
        public void TestMethod1()
        {
            var user = ServiceTestUtils.CreateUser(email);
            user.ContractorInfo.Truck.Location = LocationHelper.CreatePoint(43.5318683, -96.7271978);

            //CivicAddressDb pickUpAddress = new CivicAddressDb(){ RawInputAddress = };

            //var order = ServiceTestUtils.CreateOrder(user, Models.Orders.OrderStatus.Queued, )

        }






        


    }
}
