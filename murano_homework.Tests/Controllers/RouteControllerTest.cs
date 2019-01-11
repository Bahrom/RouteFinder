using Microsoft.VisualStudio.TestTools.UnitTesting;
using murano_homework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace murano_homework.Tests.Controllers
{
    [TestClass]
   public class RouteControllerTest
    {
        [TestMethod]
        public void GetRoutes()
        {
            var c = new RouteController();
            c.GetRoute("AER", "CEE", System.Threading.CancellationToken.None);


        }
    }
}
