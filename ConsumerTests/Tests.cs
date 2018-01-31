using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet.Mocks.MockHttpService.Models;

namespace ConsumerTests
{
    [TestClass]
    public class Tests : PactSetup
    {
        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            CreatePactBuilder();
            SetPactRelationship("Consuming Service", "Provider Api");
            SetPactVersion("1.0.0");
            SetMockServerPort(9222);
            CreateMockService();
            CreateApiTestClient();
        }

        [TestCleanup]
        public void VerifyAndClearInteractions()
        {
            MockProviderService.VerifyInteractions();
            MockProviderService.ClearInteractions();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }

        [TestMethod]
        public void GetThing_WhenTheThingExists_ReturnTheThing()
        {
            MockProviderService.Given("There is a thing")
                .UponReceiving("A GET request to retrieve the thing")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/api/values/",
                    Headers = new Dictionary<string, object>
                    {
                        {"Accept", "application/json"}
                    }
                    //Query = ""
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body =
                        new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                        {
                            value1 = "value1",
                            value2 = "value2"
                        }
                }); //NOTE: WillRespondWith call must come last as it will register the interaction

            var response = ApiTestClient.MakeRequest(HttpMethod.Get, "http://localhost:9222/api/values");

            var content = ApiTestClient.HttpResponseToString(response);

            Assert.IsNotNull(content);
        }
    }
}