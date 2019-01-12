using RouteFinder.Core;
using RouteFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace RouteFinder.Controllers
{
    public class RouteController : ApiController
    {
        public List<List<Route>> GetRoute(string srcAirport, string destAirport, CancellationToken cancellationToken)
        {
            try
            {
                RouteSearcher route_searcher = new RouteSearcher(cancellationToken, 10);
                return route_searcher.SearchRoute(srcAirport, destAirport, SearchResultType.FirstOne);
            }
            catch(Exception ex)
            {
                HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                throw new HttpResponseException(response);
            }
        }
    }
}
