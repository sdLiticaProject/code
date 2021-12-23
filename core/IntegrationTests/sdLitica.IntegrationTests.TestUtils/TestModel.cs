using Newtonsoft.Json;

namespace sdLitica.IntegrationTests.TestUtils
{
    public class TestModel
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}