
using Newtonsoft.Json;
using System.Text.Json;
using TechTalk.SpecFlow;
using TFLCodingChallenge.DTOs;

namespace TFLCodingChallenge.Specs.StepDefinitions
{
    [Binding]
    public class RoadStatusStepDefinition
    {
        private HttpClient httpClient;
        private HttpResponseMessage response;
        private Settings config;
        private string roadId;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var json = File.ReadAllText("specflow.json");
            config = JsonConvert.DeserializeObject<Settings>(json);
        }

        [Given(@"I make an HTTP GET request to TflRoad Api by passing roadid ""(.*)""")]
        public void GivenIMakeAnHTTPGETRequestToTflRoadApiByPassingRoadId(string roadId)
        {
            this.roadId = roadId;
            var url = $"{config.BaseUrl}{this.roadId}?app_id={config.App_Id}&app_key={config.App_Key}";

            // Create an HttpClient and make the HTTP GET request
            httpClient = new HttpClient();
            response = httpClient.GetAsync(url).Result;
        }

        [When(@"the http status code is ""([^""]*)""")]
        public void WhenTheHttpStatusCodeIs(string statusCode)
        {
            Assert.AreEqual(statusCode, response.StatusCode.ToString());
        }

        [Then(@"the response should be in JSON format")]
        public void ThenTheResponseShouldBeInJSONFormat()
        {
            // Ensure that the response has the content type set to JSON
            Assert.IsTrue(response.Content.Headers.ContentType.MediaType.Equals("application/json"));
        }

        [Then(@"the response should contain the following information:")]
        public void ThenTheResponseShouldContainTheFollowingInformation(Table table)
        {
            // Parse the JSON response into an object (You can use a JSON parsing library)
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var roads = System.Text.Json.JsonSerializer.Deserialize<List<RoadDetails>>(jsonResponse, options);

            foreach (var row in table.Rows)
            {
                var key = row["PropertyName"];
                var expectedValue = row["ExpectedValue"];

                var actualValue = roads[0].GetValueByKey(key);

                Assert.AreEqual(expectedValue, actualValue);
            }
        }

        [Then(@"the response should return an informative error ""([^""]*)""")]
        public void ThenTheResponseShouldReturnAnInformativeError(string errorMessage)
        {
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var responseObject = System.Text.Json.JsonSerializer.Deserialize<ResponseDto>(jsonResponse, options);

            Assert.AreEqual(errorMessage, responseObject.Message);
        }

    }

    // Define a class to represent the response JSON structure
    public class RoadDetails
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string StatusSeverity { get; set; }
        public string StatusSeverityDescription { get; set; }

        public string GetValueByKey(string key)
        {
            // Implement logic to retrieve the value by key
            switch (key)
            {
                case "id":
                    return Id;
                case "displayName":
                    return DisplayName;
                case "statusSeverity":
                    return StatusSeverity;
                case "statusSeverityDescription":
                    return StatusSeverityDescription;
                default:
                    return null;
            }
        }
    }

    public class Settings
    {
        public string BaseUrl { get; set; }
        public string App_Key { get; set; }
        public string App_Id { get; set; }
    }
}
