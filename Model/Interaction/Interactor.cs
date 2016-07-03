using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using DotNetGadgeteerCam.Model.Status;
using DotNetGadgeteerCam.Model.Pictures;
using DotNetGadgeteerCam.Model.Network;

namespace DotNetGadgeteerCam.Model.Interaction
{
    public class Interactor : StatusProviderBase
    {
        public Interactor(Button wifiButton, Button ethernetButton, IPictureTaker pictureTaker, IStatusChecker statusChecker, ServerConstructor wifiConstructor, ServerConstructor ethernetConstructor)
            : base(statusChecker, "Interaction Controls")
        {
            _pictureTaker = pictureTaker;
            wifiButton.ButtonPressed += (sender, state) => ProcessButtonPressed(sender, wifiConstructor);
            ethernetButton.ButtonPressed += (sender, state) => ProcessButtonPressed(sender, ethernetConstructor);
            IsReady = true;
        }

        private IPictureTaker _pictureTaker;
        private IServer _server;

        void ProcessButtonPressed(Button button, ServerConstructor serverConstructor)
        {
            Log("Debug button was pressed");
            if (_server == null)
            {
                button.TurnLEDOn();
                _server = serverConstructor();
            }
            _server.Connect();
            StatusChecker.Check();
            _pictureTaker.TakePicture();
        }
    }
}
