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

namespace sdLitica.WebAPI.Entities.Common.Pages
{
    /// <summary>
    /// This class represents base page for the
    /// response pages returned by REST API
    /// </summary>
    public class ApiBasePage<T> where T : BaseApiModel
    {
        /// <summary>
        /// List of links to orchestrate current response page
        /// </summary>
        public List<EntityLinkModel> Links { get; }

        private string pageBasePath;

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        public ApiBasePage(string basePagePath)
        {
            pageBasePath = basePagePath;
            Links = new List<EntityLinkModel>();
            Links.Add(new EntityLinkModel(EntityLinkModel.LinkType.Self,
                pageBasePath));
            Links.Add(new EntityLinkModel(EntityLinkModel.LinkType.Canonical,
                pageBasePath));

        }

        /// <summary>
        /// Create a set of links for the given entity
        /// </summary>
        /// <param name="entity">Entity to generate link for</param>
        /// <returns>List of links for the entity</returns>
        protected List<EntityLinkModel> getEntityLinks(T entity)
        {
            List<EntityLinkModel> result = new List<EntityLinkModel>();

            string currentContextPath =
                pageBasePath.TrimEnd('/');

            result.Add(new EntityLinkModel(EntityLinkModel.LinkType.Self,
                currentContextPath + "/" + entity.GetApiUrlPrefix()));
            result.Add(new EntityLinkModel(EntityLinkModel.LinkType.Canonical,
                currentContextPath + "/" + entity.GetApiUrlPrefix()));

            return result;
        }
    }
}
