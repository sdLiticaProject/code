using System.Collections.Generic;
using Serilog;

namespace sdLitica.IntegrationTests.TestUtils.BddUtils
{
    public class WhenStatement : BddStatement
    {
        private IDictionary<string, object> _givenData;
        private IDictionary<string, object> _thenData = new Dictionary<string, object>();

        public WhenStatement(ILogger logger, IDictionary<string, object> givenData) : base(logger)
        {
            _givenData = givenData;
        }

        public ThenStatement Then => new ThenStatement(Logger, _givenData, _thenData);

        public T GetGivenData<T>(string additionalKey = null) where T : class
        {
            return _givenData[GetDictionaryKey(typeof(T), additionalKey)] as T;
        }
        
        public void AddResultData<T>(T data, string additionalKey = null) where T : class
        {
            _thenData.Add(GetDictionaryKey(typeof(T), additionalKey), data);
        }
    }
}