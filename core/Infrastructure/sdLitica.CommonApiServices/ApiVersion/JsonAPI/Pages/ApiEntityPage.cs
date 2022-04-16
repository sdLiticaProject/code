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

namespace sdLitica.CommonApiServices.ApiVersion.JsonAPI.Pages
{
    /// <summary>
    /// This class represents an output REST API page representing
    /// one single entity
    /// </summary>
    public class ApiEntityPage<T> : ApiBasePage<T> where T : BaseApiModel
    {
        /// <summary>
        /// Constructor for the rsponse page containing particular object
        /// </summary>
        /// <param name="entity">Entity to put into the page</param>
        /// <param name="basePagePath">Url at which current response page was generated</param>
        public ApiEntityPage(T entity, string basePagePath)
            : base(basePagePath)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity representing page content
        /// </summary>
        public T Entity { get; }

        private void setupEntityLinks()
        {
            Entity.Links = getEntityLinks(Entity);
        }
    }
}
