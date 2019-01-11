using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace murano_homework.Core
{
    /// <summary>
    /// Result type for SearchRoute method
    /// </summary>
    public enum SearchResultType
    {
        /// <summary>
        /// First found route
        /// </summary>
        FirstOne,
        /// <summary>
        /// Shortest route
        /// </summary>
        ShortestOne,
        /// <summary>
        /// All found routes
        /// </summary>
        AllPossibleRoutes,

    }
}