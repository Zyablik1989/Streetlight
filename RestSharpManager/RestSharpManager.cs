using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace RestSharpManager
{
    public class RestSharpManager
    {
        #region Enums

        /// <summary>
        ///    Common API methods
        /// </summary>
        public enum ApiActions
        {
            [Description("Retrieve Entries")] RetrieveEntries = 0
        }

        /// <summary>
        ///     Routes API
        /// </summary>
        public enum Route
        {
            RetrieveEntries = 0
        }

        #endregion

        /// <summary>
        ///    API request manager instance
        /// </summary>
        public static RestSharpManager Current { get; private set; }

        public string TokenForAPI { get; set; }
        public string BaseUrl { get; set; }
        public bool IsLastRequestWasSuccessful = false;
        
        private static readonly RestClient restClient = new RestClient();
        private static RestRequest restRequest = new RestRequest();

   
        #region Common Actions

        /// <summary>
        ///     Retrieving entries from API
        /// </summary>
        public List<Entry> RetrieveEntries()
        {
            var ListOfEntries = new List<Entry>();
            //REST request
            var response = RestAction(Method.GET, Route.RetrieveEntries);
            if (!ValidateResponse(response)) return ListOfEntries;

            try
            {
                //Deserializing JSON
                ListOfEntries = JsonConvert.DeserializeObject<List<Entry>>(response.Content);
            }
            catch (Exception e)
            {
            }
            IsLastRequestWasSuccessful = true;
            return ListOfEntries;
        }

        /// <summary>
        ///   Validating respond for an errors
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static bool ValidateResponse(IRestResponse response)
        {
            if (response == null) return false;

            if (response.StatusCode != HttpStatusCode.OK) return false;

            return true;
        }

        #endregion

        #region RestSharp

        /// <summary>
        ///    Direct API manipulation via RestSharp
        /// </summary>
        /// <param name="method"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public IRestResponse RestAction(Method method, Route route)
        {
            //New request each time.
            restRequest = new RestRequest(method);

            var routeString = string.Empty;

            switch (route)
            {
                //Setting target routing 
                case Route.RetrieveEntries:
                    routeString = "api/news/get?count=20&page=1";
                    break;
                default:

                    return null;
            }

            //Connection string
            restClient.BaseUrl = new Uri(BaseUrl + "/" + routeString);
            restClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            //Universal headers
            //restRequest.AddHeader("Authorization", "Basic " + TokenForAPI);
            restRequest.AddHeader("User_Agent", "News Application");
            restRequest.AddHeader("Accept", "application/json");

            restRequest.Timeout = 30 * 1000;

            //THE REQUEST
            var restResponse = restClient.Execute(restRequest);


            return restResponse;
        }

        #endregion

        #region New manager starting

        public static void Start()
        {
            Current = null;

            if (!ValidateExportSettings())
                return;

            Init();
        }

        private static bool ValidateExportSettings()
        {
            return true;
        }

        private static void Init()
        {
            //new manager initialization
            Current = new RestSharpManager();
            SetProxy();
        }

        /// <summary>
        ///   Setting system proxy by default
        /// </summary>
        public static void SetProxy()
        {
            try
            {
                restClient.Proxy = WebRequest.GetSystemWebProxy();
                restClient.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}