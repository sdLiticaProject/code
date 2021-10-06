using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.BddUtils
{
    public class ThenStatement : BddStatement
    {
        private IDictionary<string, object> _givenData;
        private IDictionary<string, object> _thenData;

        public ThenStatement(ILogger logger, IDictionary<string, object> givenData,
            IDictionary<string, object> thenData) : base(logger)
        {
            _givenData = givenData;
            _thenData = thenData;
        }

        public T GetGivenData<T>(string additionalKey = null) where T : class
        {
            try
            {
                return _givenData[GetDictionaryKey(typeof(T), additionalKey)] as T;
            }
            catch
            {
                throw new KeyNotFoundException(
                    $"Could not find '{GetDictionaryKey(typeof(T), additionalKey)}' key in 'Given' data");
            }
        }

        public T GetResultData<T>(string additionalKey = null) where T : class
        {
            try
            {
                return _thenData[GetDictionaryKey(typeof(T), additionalKey)] as T;
            }
            catch
            {
                throw new KeyNotFoundException(
                    $"Could not find '{GetDictionaryKey(typeof(T), additionalKey)}' key in 'Result' data");
            }
        }

        public ThenStatement ResponseHasCode(HttpStatusCode code)
        {
            GetStatementLogger()
                .Information($"[{{ContextStatement}}] Expecting last response to have code '{code}'",
                    GetType().Name);

            GetResultData<HttpResponseMessage>(BddKeyConstants.LastHttpResponse).AssertError(code);
            return this;
        }
    }
}
