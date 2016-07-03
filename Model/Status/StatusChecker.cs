using System;
using Microsoft.SPOT;
using System.Collections;
using Gadgeteer.Modules.GHIElectronics;

namespace DotNetGadgeteerCam.Model.Status
{
    public class StatusChecker: IStatusChecker
    {
        public StatusChecker(MulticolorLed led)
        {
            _led = led;
            _led.TurnWhite();
            _statusProviders = new ArrayList();
        }

        private ArrayList _statusProviders;

        public IEnumerable StatusProviders { get { return _statusProviders; } }

        private MulticolorLed _led;

        //
        // IStatusChecker
        //

        public void AddStatusProvider(IStatusProvider statusProvider)
        {
            _statusProviders.Add(statusProvider);
        }

        public bool Check()
        {
            bool result = true;
            foreach (IStatusProvider statusProvider in StatusProviders)
            {
                if (!statusProvider.IsReady)
                {
                    result = false;
                }
                Log("Status Check: " + statusProvider.Name + " is " + (statusProvider.IsReady ? "Ready" : "Not Ready"));
            }
            if (result)
            {
                _led.TurnGreen();
            }
            else
            {
                _led.TurnRed();
            }
            return result;
        }

        public void Log(string message)
        {
            Debug.Print(DateTime.Now.ToString("hh:MM:ss : ") + message);
        }
    }
}
