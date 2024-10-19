using DotNetEnv;
using System.IO;
using System.Runtime.CompilerServices;

namespace Desktop_Frontend.Auth0
{
    /// <summary>
    /// This class is responsible for reading the Auth0 environment variables
    /// </summary>  
    public class Auth0Config
    {
        public string? Domain;
        public string? ClientId;
        public string? CallbackUrl;
        public string? ApiIdentifier;

        /// <summary>
        /// bool to check if the Auth0 configuration is valid
        /// </summary>
        public bool ConfigValid;
        public string? Audience;

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth0Config"/> class.
        /// </summary>       
        public Auth0Config()
        {
            LoadEnvVars();

            ConfigValid = ValidateEnvVars();

        }

        /// <summary>
        /// Reads the .env file and sets the instance variables accordingly
        /// </summary>
        private void LoadEnvVars()
        {
            string? sourceDirectory = GetSourceFileDirectory();
            
            if(sourceDirectory != null)
            {
                string envFilePath = Path.Combine(sourceDirectory, "AUTH0.env");


                if (File.Exists(envFilePath))
                {
                    using (var stream = File.OpenRead(envFilePath))
                    {
                        Env.Load(stream);  
                    }
                }


                Domain = Env.GetString("AUTH0_DOMAIN");
                ClientId = Env.GetString("AUTH0_CLIENT_ID");
                CallbackUrl = Env.GetString("AUTH0_CALLBACK_URL");
                ApiIdentifier = Env.GetString("AUTH0_API_IDENTIFIER");
                Audience = Env.GetString("AUTH0_AUDIENCE");
            }

        }

        /// <summary>
        /// Checks if .env file is read correctly and variables assigned.
        /// </summary>
        /// <returns>Returns true if .env file read properly, false otherwise.</returns>
        private bool ValidateEnvVars()
        {
            return !(string.IsNullOrEmpty(Domain) || string.IsNullOrEmpty(ClientId)
               || string.IsNullOrEmpty(CallbackUrl) || string.IsNullOrEmpty(ApiIdentifier)
               || string.IsNullOrEmpty(Audience));

        }

        /// <summary>
        /// Gets the directory of the source file where this method is called from.
        /// </summary>
        /// <param name="sourceFilePath">
        /// The full path of the source file calling this method. 
        /// This parameter is automatically provided by the compiler using the <see cref="CallerFilePathAttribute"/> and should not be explicitly passed.
        /// </param>
        /// <returns>The directory of the source file.</returns>
        private string? GetSourceFileDirectory([CallerFilePath] string sourceFilePath = "")
        {
            return Path.GetDirectoryName(sourceFilePath);
        }
    }
}
