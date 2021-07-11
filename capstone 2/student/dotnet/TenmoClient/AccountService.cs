using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;


namespace TenmoClient
{
    public class AccountService
    {
        private readonly string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        private ApiUser user = new ApiUser();
        private List<ApiTransfer> apiTransfers = new List<ApiTransfer>();
        private List<int> userAccounts = new List<int>();

        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }
       
        public decimal GetBalance()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/balance");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<decimal> response = client.Get<decimal>(request);

            return response.Data;
        }

        public List<ApiTransfer> GetPastTransfer()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/transfers");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<ApiTransfer>> response = client.Get<List<ApiTransfer>>(request);

            return apiTransfers = response.Data;

        }

        public List<int> GetAccountIdsForTransfer()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account/transfer");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<int>> response = client.Get<List<int>>(request);
            return userAccounts = response.Data;
        }

        public ApiTransfer CreateTransfer( int accountTo, decimal amount)
        {
            ApiTransfer apiTransfer = new ApiTransfer();
            apiTransfer.AccountTo = accountTo;
            apiTransfer.Amount = amount;
            
            RestRequest request = new RestRequest(API_BASE_URL + "account/transfer");
            request.AddJsonBody(apiTransfer);
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<ApiTransfer> response = client.Post<ApiTransfer>(request);

            if(response.ResponseStatus!=ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                RestRequest putRequest = new RestRequest(API_BASE_URL + "account/transfer");
                putRequest.AddJsonBody(apiTransfer);
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
                IRestResponse<ApiTransfer> putResponse = client.Put<ApiTransfer>(putRequest);

                return response.Data;
            }
            return null;
        }


        public void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception ("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("The User is not authorized");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new Exception("The User is not allowed at this resource");
                }
                else
                {

                    Console.WriteLine( response.StatusDescription ); 
                }
            }
        }
       
    }
}
