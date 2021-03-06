using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

using StreetlightExchange;
using System.Linq;

namespace RestSharpManager
{
    public class RestSharpManager
    {
        private static bool _isConnectionInProcess;

        public static Action<string> ExternalMessage;

        public static ObserverJobsEnum Job = ObserverJobsEnum.Idle;
        public static string ConnectionString;
        public static string SequenceCode;

        private static RestSharpManager _instance;
        public static RestSharpManager Current => _instance ?? (_instance = new RestSharpManager());

        private static readonly RestClient restClient = new RestClient();
        private static RestRequest restRequest = new RestRequest();

        public static BackgroundWorker bw = new BackgroundWorker();
        private static bool ValidateResponse(IRestResponse response)
        {
            if (response == null) return false;

            if (response.StatusCode != HttpStatusCode.OK) return false;

            return true;
        }

        public RestSharpManager()
        {
            bw = new BackgroundWorker();
            bw.DoWork += CountDown;
            bw.RunWorkerAsync();
        }

        private static void CountDown(object sender, DoWorkEventArgs e)
        {
            Current.WaitTimeout();
        }

        public void ClearServerData(string ServerAddress)
        {
            ConnectionString = ServerAddress;
            Job = ObserverJobsEnum.ClearServerData;
            ExternalMessage?.Invoke("Job started: Clear server data");
        }

        public void SendStreetlightData(string ServerAddress)
        {
            ConnectionString = ServerAddress;
            Job = ObserverJobsEnum.GetSequenceCode;
            ExternalMessage?.Invoke("Job started: Send street light data");
        }

        public void Cancel()
        {
            if (Job != ObserverJobsEnum.Idle) {
                Job = ObserverJobsEnum.Cancel;
                ExternalMessage?.Invoke("Job started: Cancel");
            }
        }

        public async Task RetrieveSequence(string ServerAddress)
        {
            ServerAddress = ServerAddress.TrimEnd('/');
            restRequest = new RestRequest(Method.POST);
            restClient.BaseUrl = new Uri(ServerAddress + @"/sequence/create");
            ExternalMessage?.Invoke($"RETRIEVING CODE FOR STREET LIGHT DATA SEQUENCE");
            ExternalMessage?.Invoke($"Trying to connect to :{restClient.BaseUrl}");

            var restResponse = await restClient.ExecuteAsync(restRequest);

            if (!ValidateResponse(restResponse))
            {
                ExternalMessage?.Invoke("UNSUCCESSFUL RETREIVING");
                ExternalMessage?.Invoke($"Server Answer ({restResponse.StatusCode}) {restResponse.ErrorMessage}");
                return;
            }
            try
            {
                //Deserializing JSON
                var response = JsonConvert.DeserializeObject<SequenceDto>(restResponse.Content);
                if (response != null)
                {
                    if (response.status == Status.Ok)
                    { 
                        SequenceCode = response.response.sequence;
                        ExternalMessage?.Invoke($"SUCCESSFUL RETREIVING: sequence code — {response.response.sequence}");
                    }
                    else
                    {
                        ExternalMessage?.Invoke($"UNSUCCESSFUL RETREIVING");
                        Job = ObserverJobsEnum.Cancel;
                    }
                    
                }
            }
            catch (Exception e)
            {
                ExternalMessage?.Invoke(e.Message);
            }
        }

        public async Task SendCurrentSegmentsScheme(string ServerAddress)
        {
            ServerAddress = ServerAddress.TrimEnd('/');
            restRequest = new RestRequest(Method.POST);
            restClient.BaseUrl = new Uri(ServerAddress + @"/observation/add");
            ExternalMessage?.Invoke($"SENDING  STREET LIGHT DATA SEQUENCE");
            ExternalMessage?.Invoke($"Trying to connect to :{restClient.BaseUrl}");

            var requestText = JsonConvert.SerializeObject(new ObservationRequest()
            {
                sequence = SequenceCode,
                observation = new Observation()
                {

                }
            });
            restRequest.AddJsonBody(requestText);

            var restResponse = await restClient.ExecuteAsync(restRequest);

            if (!ValidateResponse(restResponse))
            {
                ExternalMessage?.Invoke("UNSUCCESSFUL SENDING");
                ExternalMessage?.Invoke($"Server Answer ({restResponse.StatusCode}) {restResponse.ErrorMessage}");
                Job = ObserverJobsEnum.Cancel;
                return;
            }
            try
            {
                //Deserializing JSON
                var response = JsonConvert.DeserializeObject<ObservationDto>(restResponse.Content);
                if (response != null)
                {
                    if (response.status == Status.Ok)
                    {
                        var numbers = string.Join(", ",response.response?.start?.Select(_=>_.ToString()));
                        var missing = string.Join(", ", response.response?.missing?.ToList());
                        
                        ExternalMessage?.Invoke($"SUCCESSFUL SENDING: presumed numbers — {numbers}");
                        ExternalMessage?.Invoke($"SUCCESSFUL SENDING: missing segments — {missing}");
                    }
                    else
                    {
                        ExternalMessage?.Invoke($"UNSUCCESSFUL SENDING");
                        ExternalMessage?.Invoke($"message — {response.msg}");
                        Job = ObserverJobsEnum.Cancel;
                    }
                }
            }
            catch (Exception e)
            {
                ExternalMessage?.Invoke(e.Message);
                Job = ObserverJobsEnum.Cancel;
            }
        }

        private async Task ClearServerDataJob(string ServerAddress)
        {
            ServerAddress = ServerAddress.TrimEnd('/');
            restRequest = new RestRequest(Method.GET);
            restClient.BaseUrl = new Uri(ServerAddress + @"/clear");
            ExternalMessage?.Invoke($"CLEARING SERVER DATA");
            ExternalMessage?.Invoke($"Trying to connect to :{restClient.BaseUrl}");

            var restResponse = await restClient.ExecuteAsync(restRequest);
            
            if (!ValidateResponse(restResponse))
            {
                ExternalMessage?.Invoke("UNSUCCESSFUL RETREIVING");
                ExternalMessage?.Invoke($"Server Answer ({restResponse.StatusCode}) {restResponse.ErrorMessage}");
                return;
            }
            try
            {
                //Deserializing JSON
                var response = JsonConvert.DeserializeObject<ClearDto>(restResponse.Content);
                if (response != null)
                {
                    ExternalMessage?.Invoke($"CLEAR SUCCESSFUL: status — {response.status}, response — {response.response}");
                }
            }
            catch (Exception e)
            {
                ExternalMessage?.Invoke(e.Message);
            }
        }


        private async Task WaitTimeout()
        {
            while (true)
            {
                if (_isConnectionInProcess)
                {
                    if (Job == ObserverJobsEnum.Cancel)
                    {
                        ExternalMessage?.Invoke("CANCELING PENDING THE CONNECTION COMPLETE");
                    }
                    await Task.Run(() => { Thread.Sleep(1000); });
                    continue;
                }

                if (Job == ObserverJobsEnum.Cancel)
                {
                    Job = ObserverJobsEnum.Idle;
                    ExternalMessage?.Invoke("CANCELED.");
                }

                await Task.Run(() => { Thread.Sleep(500); });

                switch (Job)
                {
                    case ObserverJobsEnum.GetSequenceCode:
                        Job = ObserverJobsEnum.SendSegmentsScheme;
                        lock (Current) { _isConnectionInProcess = true; }
                        await RetrieveSequence(ConnectionString).ConfigureAwait(false);
                        lock (Current) { _isConnectionInProcess = false; }
                        //
                        break;
                    case ObserverJobsEnum.SendSegmentsScheme:
                        lock (Current) { _isConnectionInProcess = true; }
                        await SendCurrentSegmentsScheme(ConnectionString).ConfigureAwait(false);
                        lock (Current) { _isConnectionInProcess = false; }
                        break;
                    case ObserverJobsEnum.ClearServerData:
                        Job = ObserverJobsEnum.Idle;
                        lock (Current) { _isConnectionInProcess = true; }
                        await ClearServerDataJob(ConnectionString).ConfigureAwait(false);
                        lock (Current) { _isConnectionInProcess = false; }
                        break;
                }

                await Task.Run(() => { Thread.Sleep(500); });
            }
        }

    }
}