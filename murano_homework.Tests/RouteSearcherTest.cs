﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using murano_homework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace murano_homework.Tests
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
        public void SearchRoute()
        {
            RouteSearcher route_searcher = new RouteSearcher(System.Threading.CancellationToken.None, 10);
            route_searcher.SearchRoute("AER", "CEE", SearchResultType.FirstOne);
            route_searcher.SearchRoute("AER", "CEE", SearchResultType.AllPossibleRoutes);
            route_searcher.SearchRoute("AER", "CEE", SearchResultType.ShortestOne);
            route_searcher.SearchRoute("None", "None", SearchResultType.FirstOne);
        }

    }
}
