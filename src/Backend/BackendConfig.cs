using DotNetEnv;
using System.IO;
using System.Runtime.CompilerServices;

namespace Desktop_Frontend.Backend
{
    public class BackendConfig
    {
        public string BackendUrl;
        public string Create_User_Endpoint;
        public string All_Ing_Endpoint;
        public bool ConfigValid;

        public BackendConfig()
        {
            LoadEnvVars();
            ConfigValid = ValidateEnvVars();
        }

        private void LoadEnvVars()
        {

            string sourceDirectory = GetSourceFileDirectory();
            string envFilePath = Path.Combine(sourceDirectory, "BACKEND.env");


            if (File.Exists(envFilePath))
            {
                using (var stream = File.OpenRead(envFilePath))
                {
                    Env.Load(stream);  // Load the .env file into the environment
                }
            }


            //Env.TraversePath().Load("BACKEND.env");
            BackendUrl = Env.GetString("BACKEND_URL");
            Create_User_Endpoint = Env.GetString("CREATE_USER");
            All_Ing_Endpoint = Env.GetString("ALL_INGREDIENTS");
        }

        private bool ValidateEnvVars()
        {
            return !(string.IsNullOrEmpty(BackendUrl) || string.IsNullOrEmpty(Create_User_Endpoint)
               || string.IsNullOrEmpty(All_Ing_Endpoint));
        }

        private string GetSourceFileDirectory([CallerFilePath] string sourceFilePath = "")
        {
            return Path.GetDirectoryName(sourceFilePath);
        }
    }
}
