using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace sdLitica.Helpers
{
    /// <summary>
    /// class for reflection operations
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// get property name from expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns>property name</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
    }
}
