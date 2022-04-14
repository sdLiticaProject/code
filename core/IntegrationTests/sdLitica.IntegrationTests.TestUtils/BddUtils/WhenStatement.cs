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

		public T GetGivenData<T>(string additionalKey = null)
		{
			try
			{
				return (T) _givenData[GetDictionaryKey(typeof(T), additionalKey)];
			}
			catch
			{
				throw new KeyNotFoundException(
					$"Could not find '{GetDictionaryKey(typeof(T), additionalKey)}' key in 'Given' data");
			}
		}

		public T GetResultData<T>(string additionalKey = null)
		{
			try
			{
				return (T) _thenData[GetDictionaryKey(typeof(T), additionalKey)];
			}
			catch
			{
				throw new KeyNotFoundException(
					$"Could not find '{GetDictionaryKey(typeof(T), additionalKey)}' key in 'Result' data");
			}
		}

		public void AddResultData<T>(T data, string additionalKey = null)
		{
			var key = GetDictionaryKey(typeof(T), additionalKey);
			if (_thenData.ContainsKey(key))
			{
				_thenData.Remove(key);
			}

			_thenData.Add(key, data);
		}
	}
}
