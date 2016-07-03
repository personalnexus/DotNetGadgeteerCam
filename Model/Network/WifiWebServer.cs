using System;
using Microsoft.SPOT;
using Gadgeteer.Networking;
using Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GHIElectronics.NETMF.Net;
using DotNetGadgeteerCam.Model.Pictures;
using DotNetGadgeteerCam.Model.Status;
using DotNetGadgeteerCam.Secret;

namespace DotNetGadgeteerCam.Model.Network
{
    public class WifiWebServer: WebServerBase
    {
        public WifiWebServer(WiFi_RS21 wifiInterface, IPictureTaker pictureTaker, IPictureArchive pictureArchive, IStatusChecker statusChecker)
            : base(wifiInterface, pictureTaker, pictureArchive, statusChecker, "Wifi Web Server")
        {
            _wifiInterface = wifiInterface;
        }

        private WiFi_RS21 _wifiInterface;

        public override void Connect()
        {
            base.Connect();
            if (!IsReady)
            {
                try
                {
                    int tryCount = 3;
                    WiFi_RS21.WiFiNetworkInfo wifiNetworkInfo;
                    do
                    {
                        wifiNetworkInfo = _wifiInterface.Search(Codes.SSID);
                    } 
                    while (--tryCount > 0 && wifiNetworkInfo == null);
                    if (wifiNetworkInfo == null)
                    {
                         Log("Did not find the wifi network " + Codes.SSID);
                    }
                    else
                    {
                        _wifiInterface.Join(wifiNetworkInfo, Codes.Password);
                        IsReady = true;
                    }
                }
                catch (WiFi.WiFiException wifiException)
                {
                    Log("Error while joining wifi network, code: " + wifiException.errorCode + ", message: " + wifiException.Message);
                }
            }
        }
    }
}
