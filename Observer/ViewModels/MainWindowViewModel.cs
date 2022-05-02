using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Observer.Core;

namespace Observer.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<string> serverMessages;

        public ObservableCollection<string> ServerMessages
        {
            get { return serverMessages; }
            set
            {
                if (value != serverMessages)
                {
                    serverMessages = value;
                }

                OnPropertyChanged();
            }
        }

        bool isInvokingNewStreetlightInProgress = false;

        private string serverAddress = "http://localhost:5000";

        public string ServerAddress
        {
            get { return serverAddress; }
            set
            {
                if (value != serverAddress)
                {
                    serverAddress = value;
                }

                OnPropertyChanged();
            }
        }

        private int streetlightSecondsLeft;

        public int StreetlightSecondsLeft
        {
            get { return streetlightSecondsLeft; }
            set {
                if (value != streetlightSecondsLeft)
                {
                    streetlightSecondsLeft = value;
                }

                OnPropertyChanged();
            }
        }

        private bool isLightGreen;

        public bool IsLightGreen
        {
            get { return isLightGreen; }
            set { 
                if (value != isLightGreen)
                {
                    isLightGreen = value;
                }
                OnPropertyChanged();
            }
        }

        private byte? leftDigit;

        public byte? LeftDigit
        {
            get { return leftDigit; }
            set { leftDigit = value;
                OnPropertyChanged();
            }
        }

        private byte? rightDigit;

        public byte? RightDigit
        {
            get { return rightDigit; }
            set { rightDigit = value;
                OnPropertyChanged();
            }
        }

        private string subtype;

        public string Subtype
        {
            get { return subtype; }
            set
            {
                subtype = value;
                OnPropertyChanged();
            }
        }

        private int initialNumber;

        public int InitialNumber
        {
            get { return initialNumber; }
            set
            {
                initialNumber = value;
                OnPropertyChanged();
            }
        }
        
        public RelayCommand ConnectToAnalisysServerCommand { get; set; }

        public RelayCommand OrderToClearAnalisysServerCommand { get; set; }
        
        public RelayCommand InvokeNewStreetlightCommand { get; set; }

        public MainWindowViewModel()
        {
            ServerMessages = new ObservableCollection<string>();
            InvokeNewStreetlightCommand = new RelayCommand(async o => { await ExecuteInvokeNewStreetlight(); },
                o => CanExecuteInvokeNewStreetlight());

            ConnectToAnalisysServerCommand = new RelayCommand(async o => { await ExecuteConnectToAnalisysServer(); });
            OrderToClearAnalisysServerCommand = new RelayCommand(async o => { await ExecuteOrderToClearAnalisysServer(); });
        }
        private bool CanExecuteInvokeNewStreetlight()
        {
            if (isInvokingNewStreetlightInProgress)
            {
                return false;
            }
            return true;
        }
        private async Task ExecuteInvokeNewStreetlight()
        {
            lock (this)
            {
                isInvokingNewStreetlightInProgress = true;
            }
            StreetLightManager.StreetlightManager.InvokeNewStreetlight();
            UpdateUI();
            lock (this)
            {
                isInvokingNewStreetlightInProgress = false;
            }
        }

        private async Task ExecuteConnectToAnalisysServer()
        {
            

            ServerMessages.Add("777");
                await Task.Run(
                             () =>
                             {
                                 Thread.Sleep(500);
                             });
                ServerMessages.Add("666");
            await Task.Run(
                 () =>
                 {
                     Thread.Sleep(100);
                 });


        }

        private async Task ExecuteOrderToClearAnalisysServer()
        {
            await Task.Run(
                 () =>
                 {
                     RestSharpManager.RestSharpManager.Current.ClearServerData(ServerAddress);

                 });
            //ServerMessages.Clear();


        }
        

        internal void UpdateUI()
        {
            StreetlightSecondsLeft = StreetLightManager.StreetlightManager.SecondsLeft;
            IsLightGreen = StreetLightManager.StreetlightManager.LightIsGreen;

            if (StreetLightManager.StreetlightManager.DigitsSegmentsScheme.Count > 1)
            {
                LeftDigit = StreetLightManager.StreetlightManager.DigitsSegmentsScheme[0];
                RightDigit = StreetLightManager.StreetlightManager.DigitsSegmentsScheme[1];
            }
            else if (StreetLightManager.StreetlightManager.DigitsSegmentsScheme.Count > 0)
            {
                LeftDigit = null;
                RightDigit = StreetLightManager.StreetlightManager.DigitsSegmentsScheme[0];
            }
            else
            {
                LeftDigit = null;
                RightDigit = null;
            }

            Subtype = StreetLightManager.StreetlightManager.GetSubtypeOfStreetlight();
            InitialNumber = StreetLightManager.StreetlightManager.GetInitialNumberOfStreetlight();
        }

        public void UpdateDigitsSegments()
        {
            UpdateUI();
        }

        public void UpdateColor()
        {
            UpdateUI();
        }

        public void AddMessage(string message)
        {
            ServerMessages.Add(message);
        }


    }
}
