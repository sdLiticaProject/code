using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace sdLitica.Exceptions.Abstractions
{
    /// <summary>
    /// model that represent api exception
    /// </summary>
    public abstract class BaseExceptionModel : Exception
    {
        /// <summary>
        /// entity id
        /// </summary>
        public string EntityId { get; set; }
        
        /// <summary>
        /// http status code
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }
        
        /// <summary>
        /// exception detailed code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// base contructor
        /// </summary>
        public BaseExceptionModel()
        { }

        /// <summary>
        /// contructor with message param
        /// </summary>
        /// <param name="message">message of exception</param>
        public BaseExceptionModel(string message)
            : base(message)
        { }

        /// <summary>
        /// contructor with message and inner exception param
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BaseExceptionModel(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// json format of exception model
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return (new JObject(
                       new JProperty(nameof(Code), Code),
                       new JProperty(nameof(EntityId), EntityId),
                       new JProperty(nameof(Message), Message)
                   )).ToString();
        }
    }
}
