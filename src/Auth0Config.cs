using System.IO;
using DotNetEnv;


namespace Desktop_Frontend
{
    public class Auth0Config
    {
        public string Domain;
        public string ClientId;
        public string CallbackUrl;
        public string ApiIdentifier; 

        public Auth0Config()
        {
            LoadEnvVars();

            ValidateEnvVars();

            // Check if values are null or empty
            if (string.IsNullOrEmpty(Domain) || string.IsNullOrEmpty(ClientId))
            {
                throw new InvalidOperationException("Auth0 configuration is missing. Check your .env file.");
            }
        }

        private void LoadEnvVars()
        {
            Env.TraversePath().Load();

            Domain = Env.GetString("AUTH0_DOMAIN");
            ClientId = Env.GetString("AUTH0_CLIENT_ID");
            CallbackUrl = Env.GetString("AUTH0_CALLBACK_URL");
            ApiIdentifier = Env.GetString("AUTH0_API_IDENTIFIER");
        }

        private void ValidateEnvVars()
        {
            if (string.IsNullOrEmpty(Domain) || string.IsNullOrEmpty(ClientId)
               || string.IsNullOrEmpty(CallbackUrl) || string.IsNullOrEmpty(ApiIdentifier))
            {
                throw new Exception("Auth0 configuration is missing. Missing environment variables.");
            }
        }
    }
}
