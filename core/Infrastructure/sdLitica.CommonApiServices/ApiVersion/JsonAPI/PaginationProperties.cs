/* *************************************************************************
 * This file is part of project "Insight Project".
 *
 *  Insight Project is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Insight Project is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
 * *************************************************************************/

using System;

namespace sdLitica.CommonApiServices.ApiVersion.JsonAPI
{
    /// <summary>
    /// This class represent pagination properties for 
    /// List entities pages in REST API responses
    /// </summary>
    public class PaginationProperties
    {
        /// <summary>
        /// Default value for offset in collection navigation
        /// </summary>
        public static int DEFAULT_OFFSET = 0;
        /// <summary>
        /// Default value for page size in collection navigation
        /// </summary>
        public static int DEFAULT_PAGE_SIZE = 20;
        /// <summary>
        /// Default value for hasMore in collection navigation
        /// </summary>
        public static bool DEFAULT_HAS_MORE = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="offset">collection navigation offset</param>
        /// <param name="pageSize">collection navigation page size</param>
        /// <param name="count">actual number of items on the page</param>
        /// <param name="hasMore">flag showing that collection has more elements to show</param>
        public PaginationProperties(int count = 0, bool hasMore = false, int offset = 0, int pageSize = 20)
        {
            Offset = offset;
            PageSize = pageSize;
            Count = count;
            HasMore = hasMore;
            if (Count > pageSize)
            {
                throw new ArgumentException("Count can be greater than offset");
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public PaginationProperties()
        {
            Offset = DEFAULT_OFFSET;
            PageSize = DEFAULT_PAGE_SIZE;
            HasMore = DEFAULT_HAS_MORE;
            Count = 0;
        }
        
        /// <summary>
        /// Current page offset in the corresponding collection
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Current page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Actual number of entities on the page
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Flag showing that collection has more elements to return
        /// </summary>
        public bool HasMore { get; set; }
    }
}
