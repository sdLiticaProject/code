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
 *  along with Insight Project.  If not, see <http://www.gnu.org/licenses/>.
 * *************************************************************************/

using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.CommonApiServices.ApiVersion.JsonAPI;
using sdLitica.CommonApiServices.ApiVersion.JsonAPI.Pages;

namespace sdLitica.WebAPI.Controllers
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class ApiVersionsController : Controller
    {
        /// <summary>
        /// This REST API handler return the list of currently available API versions
        /// </summary>
        /// <returns>List of currently available API versions</returns>
        [Route("api")]
        [HttpGet]
        public ApiEntityListPage<ApiVersionModel> Get()
        {
            ApiVersionModel v1 = new ApiVersionModel("v1", true, true);

            ApiEntityListPage<ApiVersionModel> result =
                new ApiEntityListPage<ApiVersionModel>(new List<ApiVersionModel> {v1},
                    HttpContext.Request.Path.ToString());

            return result;
        }
    }
}
