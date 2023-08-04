using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        string filterKey = "subject";
        string filterValue = "Please delete the specified account form AD";
        // Set up the HttpClient with the base URL
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://servicedeskplus.uk")
        };

        // Set the bearer token in the Authorization header
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "1000.4ea56cf6a5fea44fcf4293d989c46e73.027cfe38fb84f99c129fede3eeb3d24e");

        // Make the GET request to "/api/v3/requests"
        HttpResponseMessage response = await httpClient.GetAsync("/api/v3/requests");

        // Check if the request was successful (HTTP status code 200-299)
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string
            string content = await response.Content.ReadAsStringAsync();

            // Parse the response content as a JSON object
            var jsonResponse = JObject.Parse(content);

            // Check if the "requests" property exists in the JSON response
            if (jsonResponse.TryGetValue("requests", out JToken requestsToken))
            {
                // Convert the "requests" property to a list of JToken
                var data = requestsToken.ToObject<List<JToken>>();

                // Filter the data based on the filter criteria
                var filteredData = new List<JToken>();
                foreach (var item in data)
                {
                    if (item is JObject jObject && jObject.ContainsKey(filterKey) && jObject[filterKey].ToString() == filterValue)
                    {
                        filteredData.Add(item);
                    }
                }

                // Convert the filtered data back to JSON
                var filteredJson = JsonConvert.SerializeObject(filteredData);

                Console.WriteLine(filteredJson);
            }
            else
            {
                Console.WriteLine("No requests found in the response.");
            }
        }
        else
        {
            // Handle the case when the request was not successful
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
        }
    }
}