using Desktop_Frontend.DSOs;
using System.Net.Http;
using System.Net.Http.Headers;

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
        public async Task<List<Ingredient>> GetAllIngredients()
        {
            // Returning an empty list as a placeholder
            return await Task.FromResult(new List<Ingredient>());
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
                success = await ValidateHTTPResponse(response);

            }
            catch (Exception)
            {
               success = false;
            }


            return success;
        }

        private async Task<bool> ValidateHTTPResponse(HttpResponseMessage response)
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
    }
}
