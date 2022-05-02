using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Observer.Core;

namespace Observer.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        bool isInvokingNewStreetlightInProgress = false;

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

        public RelayCommand InvokeNewStreetlightCommand { get; set; }

        public MainWindowViewModel()
        {
            InvokeNewStreetlightCommand = new RelayCommand(async o => { await ExecuteInvokeNewStreetlight(); },
                o => CanExecuteInvokeNewStreetlight());
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

        private bool CanExecuteInvokeNewStreetlight()
        {
            if (isInvokingNewStreetlightInProgress)
            {
                return false;
            }
            return true;
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


    }
}
