namespace ES.API.Requests.ViewModels
{
    public class Auth0LoginModel
    {
        public Auth0LoginModel(
            string clientId, string clientSecret, string audience,
            string username, string password)
        {
            this.client_id = clientId;
            this.client_secret = clientSecret;
            this.audience = audience;
            this.grant_type = "client_credentials";
            this.username = username;
            this.password = password;
        }

        public string client_id { get; private set; }
        public string client_secret { get; private set; }
        public string audience { get; private set; }
        public string grant_type { get; private set; }
        public string username { get; private set; }
        public string password { get; private set; }
    }
}