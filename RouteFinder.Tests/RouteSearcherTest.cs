using Microsoft.VisualStudio.TestTools.UnitTesting;
using RouteFinder.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace RouteFinder.Tests
{
    [TestClass]
   public class RouteSearcherTest
    {
        [TestMethod]
        public async Task GetAirline()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            await route_searcher.GetAirlineAsync("Y7");
            await route_searcher.GetAirlineAsync("None");
        }

        [TestMethod]
        public async Task GetAirport()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            await route_searcher.GetAirportAsync("AER");
            await route_searcher.GetAirportAsync("None");
        }

        [TestMethod]
        public async Task GetRoutes()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            await route_searcher.GetRoutesAsync("AER");
            await route_searcher.GetRoutesAsync("None");
        }

        [TestMethod]
        public void SearchFirstRoute()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            route_searcher.SearchRoute("AER", "CEE", SearchResultType.FirstOne);

        }

        [TestMethod]
        public void SearchShortesRoute()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            route_searcher.SearchRoute("AER", "CEE", SearchResultType.ShortestOne);
        }

        [TestMethod]
        public void SearchAllRoutes()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            route_searcher.SearchRoute("AER", "CEE", SearchResultType.AllPossibleRoutes);
        }

        [TestMethod]
        public void SearchNoneExistRoute()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            route_searcher.SearchRoute("AER", "None", SearchResultType.AllPossibleRoutes);
        }

    }
}
