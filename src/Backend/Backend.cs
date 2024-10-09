using Desktop_Frontend.DSOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;

namespace Desktop_Frontend.Backend
{
    public class Backend : IBackend
    {
        private BackendConfig config;

        private HttpClient HttpClient;

        public Backend() : this(new BackendConfig()) { }

        public Backend(BackendConfig config)
        {
            this.config = config;

            HttpClient = new HttpClient();
        }

        // Implementation of GetAllIngredients, returning an empty list for now
        public async Task<List<Ingredient>> GetAllIngredients(IUser user)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            //Create request
            string url = config.BackendUrl + config.All_Ing_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, url); ;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);
                ValidateAllIngResponse(response);
                FillAllIngList(response, ingredients);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to fetch all ingredients");
            }


            return ingredients;
        }

        public async Task<bool> CreateUser(IUser user)
        {
            bool success = false;

            //Create request
            string url = config.BackendUrl + config.Create_User_Endpoint;
            string accessToken = user.GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Post, url);;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //Send request and get response
            try
            {
                HttpResponseMessage response = await HttpClient.SendAsync(request);

                success = await ValidateUserCreation(response);

            }
            catch (Exception)
            {
               success = false;
            }


            return success;
        }

        private async Task<bool> ValidateUserCreation(HttpResponseMessage response)
        {
            bool valid = false;

            if(response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.Contains("Item created successfully") ||
                    responseBody.Contains("Item already exists."))
                {
                    valid = true;
                }
            }

            return valid;
        }

        private async void ValidateAllIngResponse(HttpResponseMessage response)
        {
           if (!response.IsSuccessStatusCode)
           {
                throw new Exception();
           }
        }

        private async void FillAllIngList(HttpResponseMessage response, List<Ingredient> allIng)
        {
            // Read the response content as a string
            string body = await response.Content.ReadAsStringAsync();

            // Parse the JSON response
            var jsonBody = JsonDocument.Parse(body);

            // Check if the root element contains "result"
            if (jsonBody.RootElement.TryGetProperty("result", out JsonElement resultArray))
            {
                // Enumerate through each ingredient in the "result" array
                foreach (JsonElement item in resultArray.EnumerateArray())
                {
                    // Extract the name and type from the JSON object
                    string name = item.GetProperty("name").GetString();
                    string ingType = item.GetProperty("type").GetString(); ;

                    // Create an Ingredient object and add it to the list
                    allIng.Add(new Ingredient(name, ingType));
                }
            }
        }

    }
}
