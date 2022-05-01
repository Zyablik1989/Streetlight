using System;
using System.ComponentModel;
using System.Net;
using RestSharp;

namespace ObserverWithRestClient
{
    internal class RestSharpManager
    {

        public static RestSharpManager Current { get; private set; }

        public string Url { get; set; }
        public string TokenForAPI { get; set; }
        public int Timeout { get; set; } = 30;
        //public string RequestBody;
        public HttpStatusCode LastErrorCode { get; set; }

        private static RestClient restClient = new RestClient();
        private static RestRequest restRequest = new RestRequest();

        public enum ApiActions
        {
            SendDisplayStatus = 0,
        }

        public enum Route
        {
            SendSale = 0
        }

        public bool ApiAction(ApiActions action)
        {
            bool result = true;

            switch (action)
            {
                case ApiActions.SendDisplayStatus: 
                    result = SendDisplayStatus();
                    break;
                default:
                    Console.WriteLine($"Unknown API action: \"{0}\"", action.ToString());
                    return false;
            }

            return result;
        }

        public static void Start()
        {
            Current = new RestSharpManager();

        }

        private bool SendDisplayStatus()
        {
            
            try
            {
                if (string.IsNullOrEmpty(TokenForAPI))
                {
                    Console.WriteLine("There is no Token for request. Upload is impossible");
                    return false;
                }

                //if (string.IsNullOrEmpty(RequestBody))
                {
                    Console.WriteLine("There is no Body for request. Upload is impossible"); 
                    return false;
                }

                //var response = RestAction(Method.Put, Route.SendSale);
                //if (!ValidateResponse(response)) return false;
                
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private static bool ValidateResponse(RestResponse response)
        {
            if (response == null)
            {
                Console.WriteLine("Unable send a request with current route");
                return false;
            }

            if (response.StatusCode != HttpStatusCode.Created)
            {
                Console.WriteLine($"An error occured while requesting: ({0}) {1}",
                    (int)response.StatusCode,
                    response.StatusCode);

                if (!string.IsNullOrEmpty(response.StatusDescription))
                    Console.WriteLine(response.StatusDescription);

                if (!string.IsNullOrEmpty(response.ErrorMessage))
                    Console.WriteLine(response.ErrorMessage);

                Current.LastErrorCode = response.StatusCode;
                return false;
            }

            return true;
        }

        //public RestResponse RestAction(Method method, Route route)
        //{
        //    restRequest = new RestRequest(method.ToString());

        //    //restClient. = new Uri(Url);

        //    restRequest.AddHeader("Authorization", "Basic " + TokenForAPI);
        //    restRequest.AddHeader("Content-Type", "application/json");
        //    restRequest.AddHeader("Accept", "application/json");

        //    //restRequest.AddParameter("application/json", RequestBody, ParameterType.RequestBody); 

        //    restRequest.Timeout = Timeout * 1000;

        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //    //RestResponse restResponse = restClient.Execute(restRequest);
            
        //    //if ((int)restResponse.StatusCode == 201)
        //        //Console.WriteLine($"Request '{1}' with a method {0}, was completed with result:({2}) {3}", 
        //        //    method,
        //        //    route,
        //        //(int)restResponse.StatusCode,
        //        //restResponse.StatusCode  );
        //    //else
        //    //    Console.WriteLine($"Request '{1}' with a method {0}, was completed with result:({2}) {3}", 
        //    //        method,
        //    //        route,
        //    //        (int)restResponse.StatusCode,
        //    //        restResponse.StatusCode);
        //    //return restResponse;
        //}



    }
}