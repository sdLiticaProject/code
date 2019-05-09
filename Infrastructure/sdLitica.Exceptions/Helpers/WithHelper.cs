using sdLitica.Exceptions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Exceptions.Helpers
{
    /// <summary>
    /// Helper that added to objects like sticky interfaces
    /// </summary>
    public static class WithHelper
    {
        /// <summary>
        /// add to exception entity id
        /// </summary>
        /// <typeparam name="T">child class of BaseExceptionModel</typeparam>
        /// <param name="item">the current object</param>
        /// <param name="entityId">entity id</param>
        /// <returns>this exception with entity id</returns>
        public static T WithId<T> (this T exception, string entityId) where T : BaseExceptionModel
        {
            exception.EntityId = entityId;
            return exception;
        }
        /// <summary>
        /// add to exception code
        /// </summary>
        /// <typeparam name="T">child class of BaseExceptionModel</typeparam>
        /// <param name="item">the current object</param>
        /// <param name="code">code</param>
        /// <returns>this exception with code</returns>
        public static T WithCode<T>(this T exception, string code) where T : BaseExceptionModel
        {
            exception.Code = code;
            return exception;
        }
    }
}
