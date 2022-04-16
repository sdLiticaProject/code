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

using System.Collections.Generic;

namespace sdLitica.CommonApiServices.ApiVersion.JsonAPI.Pages
{
    /// <summary>
    /// This class represents an output REST API page representing
    /// list of entities
    /// </summary>
    public class ApiEntityListPage<T> : ApiBasePage<T> where T : BaseApiModel
    {
        /// <summary>
        /// Constructor for the rsponse page containing list of objects
        /// </summary>
        /// <param name="entities">list of entities to put to the resulting page</param>
        /// <param name="basePagePath">Url at which current response page was generated</param>
        /// <param name="paginationProperties">Collection pagination properties</param>
        public ApiEntityListPage(List<T> entities, string basePagePath, PaginationProperties paginationProperties)
            : base(basePagePath)
        {
            Entities = entities;
            setupEntitiesLinks();
            Pagination = paginationProperties;
        }

        /// <summary>
        /// Constructor for the rsponse page containing list of objects
        /// </summary>
        /// <param name="entities">list of entities to put to the resulting page</param>
        /// <param name="basePagePath">Url at which current response page was generated</param>
        public ApiEntityListPage(List<T> entities, string basePagePath)
            : this(entities, basePagePath, new PaginationProperties(entities.Count))
        {
        }

        /// <summary>
        /// List of items representing content of the current response page
        /// </summary>
        public List<T> Entities { get; }

        /// <summary>
        /// Collection pagination properties
        /// </summary>
        public PaginationProperties Pagination { get; }

        private void setupEntitiesLinks()
        {
            Entities.ForEach(entitiy =>
                entitiy.Links = getEntityLinks(entitiy)
            );
        }
    }
}
