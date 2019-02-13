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

namespace sdLitica.Attributes.Swagger
{
    /// <summary>
    /// This attribute allows to hide resource handler / method
    /// from Swagger documentation
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HideInDocsAttribute : Attribute
    {
    }
}
