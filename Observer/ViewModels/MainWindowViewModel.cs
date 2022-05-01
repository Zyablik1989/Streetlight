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

                if (StreetLightManager.StreetLightManager.DigitsSegmentsScheme.Count > 1)
                {
                    LeftDigit = StreetLightManager.StreetLightManager.DigitsSegmentsScheme[0];
                    RightDigit = StreetLightManager.StreetLightManager.DigitsSegmentsScheme[1];
                }
                else if (StreetLightManager.StreetLightManager.DigitsSegmentsScheme.Count > 0)
                {
                    LeftDigit = null;
                    RightDigit = StreetLightManager.StreetLightManager.DigitsSegmentsScheme[0]; 
                }
                else
                {
                    LeftDigit = null;
                    RightDigit = null;
                }
                

              

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


        public MainWindowViewModel()
        {
                       
        }



        public void UpdateDigitsSegments()
        {
            StreetlightSecondsLeft = StreetLightManager.StreetLightManager.SecondsLeft;
        }

        public void UpdateColor()
        {
            IsLightGreen = StreetLightManager.StreetLightManager.LightIsGreen;
        }


    }
}
