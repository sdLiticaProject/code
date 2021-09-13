using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;
using Serilog;

namespace sdLitica.IntegrationTests.Tests
{
    /// <summary>
    /// This class represents a base test class for all other integration test
    /// classes
    /// </summary>
    [TestFixture]
    public class BaseApiTest
    {
        protected BaseTestConfiguration Configuration;

        protected ILogger Logger;

        // Methods to deal with tests configuration from file
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("testsettings.json", optional: true)
                .AddUserSecrets("e3dfcccf-0cb3-423a-b302-e3e92e95c128")
                .AddEnvironmentVariables()
                .Build();
        }

        public static BaseTestConfiguration GetApplicationConfiguration(string outputPath)
        {
            var configuration = new BaseTestConfiguration();

            var iConfig = GetIConfigurationRoot(outputPath);

            iConfig
                .GetSection("Server")
                .Bind(configuration);

            return configuration;
        }

        [OneTimeSetUp]
        public void Init()
        {
            Configuration = GetApplicationConfiguration(TestContext.CurrentContext.TestDirectory);
            Assert.NotNull(Configuration.RootUrl, "Server URL for tests executions is not set");
            Assert.NotNull(Configuration.UserName, "Default user name for tests executions is not set");
            Assert.NotNull(Configuration.Password, "Default password for tests executions is not set");

            Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();
            HttpResponseMessageExtension.Init(Logger);
        }
    }
}