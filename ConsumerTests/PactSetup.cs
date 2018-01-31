using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

namespace ConsumerTests
{
    [TestClass]
    public class PactSetup
    {
        public static IPactBuilder PactBuilder { get; private set; }
        public static IMockProviderService MockProviderService { get; private set; }
        public static ProviderServiceRequest Request;
        public static ApiClient ApiTestClient;
        public static int MockServerPort { get; set; }
        public const string PactDirectory = @"C:\PactFiles";
        private static string Consumer { get; set; }
        private static string Provider { get; set; }
        public static string PactFilename { get; set; }
        private static string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";
        public static string PactVersion { get; set; }

        public static void PactTestInit(TestContext context)
        {

        }

        public static void CreatePactBuilder()
        {
            PactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = PactDirectory,
                LogDir = PactDirectory
            });
        }

        /// <summary>
        /// The actual version of the pact as specified by
        /// the expected interactions defined
        /// within the test methods of the derived class
        /// </summary>
        public static void SetPactVersion(string version)
        {
            PactVersion = version;
        }

        public static void SetPactRelationship(string cosumer, string provider)
        {
            Provider = provider.Replace(" ", "_").ToLower();
            Consumer = cosumer.Replace(" ", "_").ToLower();

            PactFilename = $"{Consumer}-{Provider}";

            PactBuilder
                .ServiceConsumer(cosumer)
                .HasPactWith(provider);
        }

        public static void SetMockServerPort(int port)
        {
            MockServerPort = port;
        }

        public static void CreateMockService()
        {
            MockProviderService = PactBuilder.MockService(MockServerPort);

            MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings());
        }

        public static void CreateRequest()
        {
            Request = new ProviderServiceRequest();
        }

        public static void CreateApiTestClient()
        {
            ApiTestClient = new ApiClient(MockProviderServiceBaseUri);
        }
    }
}