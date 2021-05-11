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
    /// This class represents REST API entities 
    /// describing REST API versions
    /// </summary>
    public class ApiVersionModel : BaseApiModel
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="version">Verstion id string</param>
        /// <param name="isActive">is version currently active/supported</param>
        /// <param name="isLatest">is version latest</param>
        public ApiVersionModel(string version, bool isActive, bool isLatest)
        {
            Version = version;
            IsActive = isActive;
            IsLatest = isLatest;
        }

        /// <summary>
        /// Version identity string, i.e. 'v1'
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Flag, showing that this version is active and maintained
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Flag, showing that this version is the latest available
        /// </summary>
        public bool IsLatest { get; }


        /// <summary>
        /// Entity links
        /// </summary>
        public List<EntityLinkModel> Links { get; set; }

        /// <summary>
        /// Get the API prefix to access current entity from the REST API.
        /// </summary>
        /// <returns></returns>
        public string getApiUrlPrefix()
        {
            return Version;
        }
    }
}
