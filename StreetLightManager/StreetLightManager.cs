using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Streetlight;

namespace StreetLightManager
{
    public class StreetlightManager
    {
        private static readonly object Sync = new object();
        protected static bool? lightIsGreen;
        protected static AbstractStreetlight streetlight;
        public static Action ColorChanged, DigitsChanged;
        public static BackgroundWorker bw = new BackgroundWorker();

        public static bool LightIsGreen
        {
            get => lightIsGreen ?? false;
            protected set => lightIsGreen = value;
        }

        public static List<byte> DigitsSegmentsScheme = new List<byte>();

        public static int GetInitialNumberOfStreetlight()
        {
            return streetlight?.InitialNumber ?? 0;
        }

        public static string GetSubtypeOfStreetlight()
        {
            if (streetlight == null) { return null; }

            switch (streetlight.GetType()) 
            {
                default: return "Default";
            }

        }

        public static int SecondsLeft { get; set; } = 1;

        public static StreetlightManager Current { get; private set; }
        public static void Start()
        {
            Current = new StreetlightManager();

            bw = new BackgroundWorker();
            bw.DoWork += CountDown;
            bw.RunWorkerAsync();
            InvokeNewStreetlight();
        }

        public static void InvokeNewStreetlight()
        {
            if (Current == null)
            {
                return;
            }

            lock (Sync)
            { 
                streetlight = StreetlightFactory.CreateStreetlight(); 
            }
            

        }

        private static void CountDown(object sender, DoWorkEventArgs e)
        {
            Task result = WaitTimeoutBeforeNextRequest();
        }


        private static async Task WaitTimeoutBeforeNextRequest()
        {
            while (true)
            {
                if (SecondsLeft <= 1)
                {
                    lock (Sync)
                    {
                        SecondsLeft = streetlight?.InitialNumber ?? 1;
                        LightIsGreen = !LightIsGreen;
                    }

                    ColorChanged?.Invoke();
                }
                else
                {
                        SecondsLeft--;
                }



                DigitsSegmentsScheme = streetlight?.ResolveNumberIntoDigitScheme(SecondsLeft);

                if (!(streetlight is StreetlightIdleWhileRed) || LightIsGreen)
                {
                    DigitsChanged?.Invoke();
                }

                

                await Task.Run(
                             () =>
                             {
                                 Thread.Sleep(1000);
                             });
            }
        }



    }
}
