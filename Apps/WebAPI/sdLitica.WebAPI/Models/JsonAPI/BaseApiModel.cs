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

namespace sdLitica.WebAPI.Entities.Common
{
    /// <summary>
    /// Base class for all entities returned by REST API.
    /// This class contains all commonalities for this kind of
    /// entities
    /// </summary>
    public interface BaseApiModel
    {

        /// <summary>
        /// Get the API prefix to access current entity from the REST API.
        /// </summary>
        /// <returns></returns>
        string GetApiUrlPrefix();

        /// <summary>
        /// Entity links
        /// </summary>
        List<EntityLinkModel> Links { get; set; }
    }
}
