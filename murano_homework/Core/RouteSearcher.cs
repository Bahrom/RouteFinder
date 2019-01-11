using murano_homework.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace murano_homework.Core
{
    public class RouteSearcher
    {
        HttpClient _client;  // Client for making request to API
        CancellationToken _cancellationToken; // For canceling request
        TaskFactory factory; // For Controling tasks
        int _retryCount = 0; // Retries for calls to API
        /// <summary>
        /// New RouteSearcher
        /// </summary>
        /// <param name="cancellationToken">Token for canceling search</param>
        /// <param name="maxDegreeOfParallelism">How many threads to use?</param>
        /// <param name="connectionLimit">How many connections to use to remote service </param>
        /// <param name="retryCount">How many times retry to attempt connect to remote service</param>
        public RouteSearcher(CancellationToken cancellationToken, int maxDegreeOfParallelism = 5, int connectionLimit = 5, int retryCount = 3)
        {
            _cancellationToken = cancellationToken;
            _retryCount = retryCount;
            // Create a scheduler with limited threads. 
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(maxDegreeOfParallelism);

            // Create a TaskFactory with scheduler. 
            factory = new TaskFactory(lcts);

            //Create a client
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://homework.appulate.com");

            // Limiting connections to API server
            ServicePointManager.DefaultConnectionLimit = connectionLimit;
            // Limiting connections to API URL
            ServicePoint servicePoint = ServicePointManager.FindServicePoint(_client.BaseAddress);


        }

        /// <summary>
        /// Gets Airline information from remote service
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public async Task<Airline> GetAirlineAsync(string alias)
        {
            if (alias == null)
                throw new ArgumentNullException("alias");

            List<Airline> airlines = null; // Result
            int retry_count = _retryCount; // Initialing retries count
            while (retry_count-- > 0)
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync($"/api/Airline/{alias}", _cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        airlines = await response.Content.ReadAsAsync<List<Airline>>(_cancellationToken);
                        break;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        break;
                }
                catch (HttpRequestException) { }
            }

            if (airlines?.Count > 0)
                return airlines?[0];
            return null;
        }

        /// <summary>
        /// Gets information about Airport
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public async Task<Airport> GetAirportAsync(string alias)
        {
            Airport airport = null; // Result
            int retry_count = _retryCount; // Initialing retries count
            while (retry_count-- > 0)
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync($"/api/Airport/search?pattern={alias}", _cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        var airports = await response.Content.ReadAsAsync<List<Airport>>(_cancellationToken);
                        airport = airports?.Where(a => a.alias == alias).FirstOrDefault();
                        break;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        break;
                }
                catch (HttpRequestException) { }

            }

            return airport;
        }

        /// <summary>
        /// Get routes information for a given airport
        /// </summary>
        /// <param name="airport"></param>
        /// <returns></returns>
        public async Task<List<Route>> GetRoutesAsync(string airport)
        {
            List<Route> routes = null; // Result
            int retry_count = _retryCount; // Initialing retries count
            while (retry_count-- > 0)
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync($"/api/Route/outgoing?airport={airport}", _cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        routes = response.Content.ReadAsAsync<List<Route>>(_cancellationToken).Result;
                        break;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        break;
                }
                catch (HttpRequestException) { }
            }

            return routes;
        }

        /// <summary>
        /// Searches routes between srcAirport and destAirport.      
        /// </summary>
        /// <param name="srcAirport">From</param>
        /// <param name="destAirport">To</param>
        /// <param name="result_type">Result type</param>
        public List<List<Route>> SearchRoute(string srcAirport, string destAirport, SearchResultType result_type)
        {
            if (srcAirport == null)
                throw new ArgumentNullException("sourceAirport");
            if (destAirport == null)
                throw new ArgumentNullException("destAirport");

            HashSet<string> _expanded_airports = new HashSet<string>(); // Visited airports
            List<List<Route>> found_routes = new List<List<Route>>(); // Found routes
            Object _objLock = new object(); // Object for sync
            ConcurrentBag<Exception> errors = new ConcurrentBag<Exception>(); // Occured errors in tasks
            using (var mre = new ManualResetEvent(false))
            {
                int count = 0; // Working tasks count

                Action<List<Route>, string> processAirport = null; // Action to find routes


                processAirport = (pathToCurrentRoute, _srcAirport) => // pathToCurrentRoute -> Routes to _srcAirport
                {
                    lock (_objLock)
                    {
                        if (_expanded_airports.Contains(_srcAirport)) // Is airport already visited
                            return;

                        _expanded_airports.Add(_srcAirport); // Check airport to visited
                    }

                    if (result_type == SearchResultType.FirstOne) // Will return the first found Route?
                        if (found_routes.Count > 0)
                            return;

                    Interlocked.Increment(ref count); // New task adding

                    factory.StartNew(async () => // Task started
                    {
                        try
                        {
                            List<Route> dest_routes = await GetRoutesAsync(_srcAirport); // Get routes of airport from remote Service
                            if (dest_routes != null && dest_routes.Count > 0) // Is result not empty?
                            {
                                foreach (var route in dest_routes)
                                {
                                    if (route.destAirport == destAirport) // Is destination airport found?
                                    {
                                        var airline = await GetAirlineAsync(route.airline); // Get airline information from remote service
                                        if (airline.active == true) // Is airline active?
                                        {
                                            List<Route> pathToRoute = pathToCurrentRoute.ToList();
                                            pathToRoute.Add(route); // Combine route with previouse routes
                                            found_routes.Add(pathToRoute); // Adding route to result
                                            return;
                                        }
                                    }
                                }
                                // Route not found in _srcAirport, continue searching another airports who has route from _srcAirport
                                foreach (var route in dest_routes)
                                {
                                    if (route.destAirport != destAirport) // Airline of route to destAirport isn't actived
                                    {
                                        List<Route> pathToRoute = pathToCurrentRoute.ToList();
                                        pathToRoute.Add(route); // Combine route with previouse routes
                                        processAirport(pathToRoute, route.destAirport); // New task for searching 
                                    }
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            errors.Add(ex);
                        }
                        finally
                        {
                            if (Interlocked.Decrement(ref count) == 0) // Is all tasks completed?
                                mre.Set(); // Signal to end
                        }
                    });
                };

                processAirport(new List<Route>(), srcAirport); // Start searching

                mre.WaitOne(); // Wait all tasks to complete

                if (errors.Count > 0)
                    throw errors.First();

                if (found_routes.Count == 0) // Route not found
                    return new List<List<Route>>(); // Empty result

                var sorted_routes = found_routes.OrderBy(r => r.Count).ToList(); // Sort by shortes route

                if (result_type == SearchResultType.AllPossibleRoutes) // Will return all found routes?
                    return sorted_routes;
                else // Return shortest Route
                {
                    List<List<Route>> result = new List<List<Route>>();
                    result.Add(found_routes[0]);
                    return result;
                }

            }

        }
    }
}