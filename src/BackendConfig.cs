using DotNetEnv;

namespace Desktop_Frontend
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
            Env.TraversePath().Load("BACKEND.env");
            BackendUrl = Env.GetString("BACKEND_URL");
            Create_User_Endpoint = Env.GetString("CREATE_USER");
            All_Ing_Endpoint = Env.GetString("ALL_INGREDIENTS");
        }

        private bool ValidateEnvVars()
        {
            return !(string.IsNullOrEmpty(BackendUrl) || string.IsNullOrEmpty(Create_User_Endpoint)
               || string.IsNullOrEmpty(All_Ing_Endpoint));
        }

    }
}
