using RestSharp;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ZeroFloApp.Services.Auth
{
    public class BearerTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public class AuthService
    {
        public async Task<string?> GetBearerToken()
        {
            var options = new RestClientOptions("https://au-0000.sandbox.auth.assemblypay.com/tokens");
            var client = new RestClient(options);
            var request = new RestRequest("", Method.Post);

            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var jsonBody = new
            {
                client_id = "7661lq8jmnu7p6s5boa11ml43g",
                client_secret = "qukuj8dsqblnfp4nannakvhiaiveudbnbthfjnr5h1bnu6fk61l",
                grant_type = "client_credentials",
                scope = "im-au-08/f3d66fa0-2098-013d-847a-0a58a9feac03:40bf3613-b1a8-424d-bca4-d9897b7e0d75:3"
            };

            request.AddJsonBody(jsonBody);

            try
            {
                var response = await client.PostAsync(request);

                if (response.IsSuccessful)
                {
                    var tokenResponse = JsonConvert.DeserializeObject<BearerTokenResponse>(response.Content);
                    return tokenResponse?.AccessToken;
                }
                else
                {
                    Console.WriteLine("Error obtaining bearer token:");
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Response: {response.Content}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Network error while obtaining bearer token:");
                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error while obtaining bearer token:");
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
