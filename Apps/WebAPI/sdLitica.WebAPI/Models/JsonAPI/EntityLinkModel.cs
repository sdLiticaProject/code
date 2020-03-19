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

namespace sdLitica.WebAPI.Entities.Common
{
    /// <summary>
    /// This model represents a link item that should be
    /// added to each entity returned by REST API call
    /// </summary>
    public class EntityLinkModel
    {
        /// <summary>
        /// Type of the link to be created
        /// </summary>
        public enum LinkType
        {
            /// <summary>
            /// Link type self
            /// </summary>
            Self,
            /// <summary>
            /// Link type canonical
            /// </summary>
            Canonical
        }


        /// <summary>
        /// Constructor to create a new entity link
        /// </summary>
        /// <param name="linkType">Type of the link to be created</param>
        /// <param name="linkAddress">Link address</param>
        public EntityLinkModel(LinkType linkType, string linkAddress)
        {
            _Type = linkType;
            Href = linkAddress;
        }

        /// <summary>
        /// Type of the link, i.e. "self" or "canonical"
        /// </summary>
        public string Type { get { return _Type.ToString(); } }
        private LinkType _Type { get; }

        /// <summary>
        /// Link address
        /// </summary>
        public string Href { get; }
    }
}
