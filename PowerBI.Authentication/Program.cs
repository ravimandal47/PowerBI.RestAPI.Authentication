using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBI.Authentication
{
    class Program
    {
        //TODO - Replace {client id} with your client app ID. 
        //Format : e2xxxxx-e4xxx-44xx-9bxx-xxxxxxxxx
        private static string clientID = "{client-id}";

        //RedirectUri you used when you registered your app.
        //For a client app, a redirect uri gives AAD more details on the specific application that it will authenticate.
        private static string redirectUri = "https://login.live.com/oauth20_desktop.srf";

        //Resource Uri for Power BI API
        private static string resourceUri = "https://analysis.windows.net/powerbi/api";

        //OAuth2 authority Uri
        private static string authority = "https://login.windows.net/common/oauth2/authorize";

        private static AuthenticationContext authContext = null;
        private static string token = String.Empty;
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                string token = await AccessToken();
                Console.WriteLine(string.Format("Power BI Token :{0}",token));
                Console.ReadLine();
            }).Wait();
        }
        /// <summary>
        /// Use AuthenticationContext to get an access token
        /// </summary>
        /// <returns></returns>
        static async Task<string> AccessToken()
        {
            if (token == String.Empty)
            {
                //Get Azure access token
                // Create an instance of TokenCache to cache the access token
                TokenCache TC = new TokenCache();
                // Create an instance of AuthenticationContext to acquire an Azure access token
                authContext = new AuthenticationContext(authority, TC);
                // Call AcquireToken to get an Azure token from Azure Active Directory token issuance endpoint
                var authenticationResult = await authContext.AcquireTokenAsync(resourceUri, clientID, new Uri(redirectUri), new PlatformParameters(PromptBehavior.RefreshSession));
                token = authenticationResult.AccessToken;
            }
            else
            {
                //Get the token in the cache
                var authenticationResult = await authContext.AcquireTokenSilentAsync(resourceUri, clientID);
                token = authenticationResult.AccessToken;

            }

            return token;
        }
    }
}
