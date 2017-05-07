using Harris.CelestialADB.ApiData;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.Desktop.WebService
{
    public static class AltiumDbApi
    {
        //const string BaseUrl = "http://localhost:64446/";
        const string BaseUrl = "https://altiumservices.azurewebsites.net/";

        public static string Token { get; set; }
        static string LastError { get; set; }

        static AltiumDbApi()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
            {
                Token = Properties.Settings.Default.AccessToken;
            }
        }

        public static async Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            var client = GenerateClient(request);

            var response = await client.ExecuteTaskAsync<T>(request);
            CheckError(response);

            return response.Data;
        }

        public static T Execute<T>(RestRequest request) where T : new()
        {
            var client = GenerateClient(request);

            var response = client.Execute<T>(request);
            CheckError(response);

            return response.Data;
        }

        public static async Task<string> ExecuteAsync(RestRequest request)
        {
            var client = GenerateClient(request);

            var response = await client.ExecuteTaskAsync(request);
            CheckError(response);

            return response.Content;
        }

        public static string Execute(RestRequest request)
        {
            var client = GenerateClient(request);

            var response = client.Execute(request);
            CheckError(response);

            return response.Content;
        }

        public static void CheckError(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var wsException = new ApplicationException(message, response.ErrorException);
                throw wsException;
            }
        }

        public static RestClient GenerateClient(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(BaseUrl);

            if (!string.IsNullOrEmpty(Token))
            {
                request.AddHeader("Authorization", String.Format("Bearer {0}", Token));
            }

            return client;
        }


        public static async Task<UserRegistrationResponse> AccountRegister(UserRegistrationRequest user)
        {
            LastError = "";

            var request = new RestRequest(Method.POST);
            request.Resource = "api/Account/Register";
            request.AddJsonBody(user);

            return await ExecuteAsync<UserRegistrationResponse>(request);
        }

        public static DatabaseStats GetDatabaseStats()
        {
            LastError = "";

            var request = new RestRequest();
            request.Resource = "api/Altium/DatabaseStats";

            return Execute<DatabaseStats>(request);
        }
        
        public static async Task<ApiResponse> CheckAccountActivated()
        {
            LastError = "";

            var request = new RestRequest(Method.GET);
            request.Resource = "api/Account/CheckAccountActivated";

            return await ExecuteAsync<ApiResponse>(request);
        }

        public static async Task<ApiResponse> ResendActivationEmail(string email)
        {
            LastError = "";

            var request = new RestRequest(Method.GET);
            request.Resource = "api/Account/ResendActivationEmail";
            request.AddParameter("email", email);

            return await ExecuteAsync<ApiResponse>(request);
        }

        public static async Task<ApiResponse> ActivateAccount(AccountActivation account)
        {
            LastError = "";

            var request = new RestRequest(Method.POST);
            request.Resource = "api/Account/ActivateAccount";
            request.AddJsonBody(account);

            return await ExecuteAsync<ApiResponse>(request);
        }

        public static async Task<TokenResponse> Login(string user, string pass)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "Token";
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", user);
            request.AddParameter("password", pass);
            
            var token = await ExecuteAsync<TokenResponse>(request);

            if (!string.IsNullOrEmpty(token.access_token))
            {
                Token = token.access_token;
                Properties.Settings.Default.AccessToken = Token;
                Properties.Settings.Default.Username = user;
                Properties.Settings.Default.Save();
            }

            return token;
        }

        public static bool CheckTokenValid()
        {
            LastError = "";

            var request = new RestRequest();
            request.Resource = "api/Account/CheckTokenValid";

            return (Execute<ApiResponse>(request)).Success;
        }

        public static async Task<String> GetUsersNameAsync()
        {
            LastError = "";

            var request = new RestRequest();
            request.Resource = "api/Account/GetUsersName";

            return (await ExecuteAsync<ApiResponse>(request)).Message;
        }

        public static string GetUsersName()
        {
            LastError = "";

            var request = new RestRequest();
            request.Resource = "api/Account/GetUsersName";

            return (Execute<ApiResponse>(request)).Message;
        }

        public static async Task<ApiResponse> CheckFirewallRule()
        {
            LastError = "";

            var request = new RestRequest(Method.GET);
            request.Resource = "api/Altium/CheckFirewallRule";

            return await ExecuteAsync<ApiResponse>(request);
        }

        public static async Task<ApiResponse> UpdateFirewallRule()
        {
            LastError = "";

            var request = new RestRequest(Method.GET);
            request.Resource = "api/Altium/UpdateFirewallRule";

            return await ExecuteAsync<ApiResponse>(request);
        }
        
        public static async Task<ApiResponse> GetIpAddress()
        {
            LastError = "";

            var request = new RestRequest(Method.GET);
            request.Resource = "api/Altium/IpAddress";

            return await ExecuteAsync<ApiResponse>(request);
        }

        public static async Task<List<GenericViewDefinition>> DatabaseViewDefinitions()
        {
            LastError = "";

            var request = new RestRequest();
            request.Resource = "api/Altium/DatabaseViewDefinitions";

            return await ExecuteAsync<List<GenericViewDefinition>>(request);
        }
        
        public static async Task<GenericViewDefinition> DatabaseViewDefinition(string viewName)
        {
            LastError = "";

            var request = new RestRequest(Method.GET);
            request.Resource = "api/Altium/DatabaseViewDefinitions";
            request.AddParameter("viewName", viewName);

            return await ExecuteAsync<GenericViewDefinition>(request);
        }

        public static async Task<List<string>> DatabaseViewList()
        {
            LastError = "";

            var request = new RestRequest();
            request.Resource = "api/Altium/DatabaseViewList";

            return await ExecuteAsync<List<string>>(request);
        }
    }
}
