using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using sdLitica.Exceptions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;


namespace sdLitica.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionValidationFilter : IActionFilter
    {
        /// <summary>
        /// multiple allowed
        /// </summary>
        public bool AllowMultiple => true;

        /// <summary>
        /// action executed method
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        /// <summary>
        /// if user sent incorrect model, method gather all needed rules and throw an error
        /// </summary>
        /// <param name="actionContext"></param>
        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            ModelStateDictionary modelState = actionContext.ModelState;

            if (!modelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var state in modelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                throw new InvalidRequestException(string.Join(' ',errors));
            }
        }

    }
}
