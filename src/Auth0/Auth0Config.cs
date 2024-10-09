using DotNetEnv;
using System.IO;
using System.Runtime.CompilerServices;

namespace Desktop_Frontend.Auth0
{
    public class Auth0Config
    {
        public string Domain;
        public string ClientId;
        public string CallbackUrl;
        public string ApiIdentifier;
        public bool ConfigValid;
        public string Audience;

        public Auth0Config()
        {
            LoadEnvVars();

            ConfigValid = ValidateEnvVars();

        }

        private void LoadEnvVars()
        {
            string sourceDirectory = GetSourceFileDirectory();
            string envFilePath = Path.Combine(sourceDirectory, "AUTH0.env");


            if (File.Exists(envFilePath))
            {
                using (var stream = File.OpenRead(envFilePath))
                {
                    Env.Load(stream);  // Load the .env file into the environment
                }
            }


            Domain = Env.GetString("AUTH0_DOMAIN");
            ClientId = Env.GetString("AUTH0_CLIENT_ID");
            CallbackUrl = Env.GetString("AUTH0_CALLBACK_URL");
            ApiIdentifier = Env.GetString("AUTH0_API_IDENTIFIER");
            Audience = Env.GetString("AUTH0_AUDIENCE");
        }

        private bool ValidateEnvVars()
        {
            return !(string.IsNullOrEmpty(Domain) || string.IsNullOrEmpty(ClientId)
               || string.IsNullOrEmpty(CallbackUrl) || string.IsNullOrEmpty(ApiIdentifier)
               || string.IsNullOrEmpty(Audience));

        }

        private string GetSourceFileDirectory([CallerFilePath] string sourceFilePath = "")
        {
            return Path.GetDirectoryName(sourceFilePath);
        }
    }
}
