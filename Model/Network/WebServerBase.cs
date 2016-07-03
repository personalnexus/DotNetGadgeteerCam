using System;
using Microsoft.SPOT;
using Gadgeteer.Modules;
using Gadgeteer.Networking;
using DotNetGadgeteerCam.Secret;
using DotNetGadgeteerCam.Model.Pictures;
using DotNetGadgeteerCam.Model.Status;
using System.Collections;
using Gadgeteer;
using MFToolkit.Net.Ntp;

namespace DotNetGadgeteerCam.Model.Network
{
    public abstract class WebServerBase : WebPageProducer, IServer
    {
        public WebServerBase(Module.NetworkModule networkInterface, IPictureTaker pictureTaker, IPictureArchive pictureArchive, IStatusChecker statusChecker, string name)
            : base(pictureTaker, pictureArchive, statusChecker, name)
        {
            _networkInterface = networkInterface;
            _networkInterface.NetworkUp += StartWebServer;
            _networkInterface.NetworkDown += StopWebServer;
            //_networkInterface.UseDHCP();
            _networkInterface.UseStaticIP(Codes.IpAddress, Codes.SubnetMask, Codes.GatewayAddress, new string[] {"8.8.8.8"});
        }

        private Module.NetworkModule _networkInterface;

        private void StartWebServer(object sender, Module.NetworkModule.NetworkState networkState)
        {
            if (!IsReady && networkState == Module.NetworkModule.NetworkState.Up)
            {
                //
                // Set the time every time the network connection is established
                //
                // DateTime UtcTime = NtpClient.GetNetworkTime("ptbtime1.ptb.de");
                // Microsoft.SPOT.Hardware.Utility.SetLocalTime(UtcTime);
                
                Log("Network is up, starting web server at " + _networkInterface.NetworkSettings.IPAddress + ":" + Codes.ListeningPort + "...");
                WebServer.StartLocalServer(_networkInterface.NetworkSettings.IPAddress, Codes.ListeningPort);
                SetupWebEvent(MainPageUrl, (m, r) => r.Respond(GetMainPage()));
                SetupWebEvent(ArchivePageUrl, (m, r) => r.Respond(GetArchivePage()));
                SetupWebEvent(StatusPageUrl, (m, r) => r.Respond(GetStatusPage()));
                SetupWebEvent(ArchivedPictureUrl, (m, r) => r.Respond(GetArchivedPicture(r.GetParameterValueFromURL(ArchivedPictureParameterName))));
                SetupWebEvent(CurrentPictureUrl, (m, r) => r.Respond(GetCurrentPicture()));
                Log("Web server started");
                IsReady = true;
            }
        }

        private void StopWebServer(object sender, Module.NetworkModule.NetworkState networkState)
        {
            if (IsReady && networkState == Module.NetworkModule.NetworkState.Down)
            {
                Log("Network is down, stopping web server...");
                foreach (WebEvent webEvent in _webEvents)
                {
                    WebServer.DisableWebEvent(webEvent);
                }
                _webEvents.Clear();
                WebServer.StopLocalServer();
                Log("Web server stopped");
                IsReady = false;
            }
        }

        //
        // IServer
        //

        public virtual void Connect()
        {
        }

        //
        // Web events
        //

        private delegate void WebEventReceiver(WebServer.HttpMethod method, Responder responder);
        
        private IList _webEvents = new ArrayList();

        private void SetupWebEvent(string pageUrl, WebEventReceiver receiver)
        {
            var webEvent = WebServer.SetupWebEvent(pageUrl);
            webEvent.WebEventReceived += (path, method, responder) =>
                {
                    Log("Responding to page request for " + path);
                    receiver(method, responder);
                };
            _webEvents.Add(webEvent);
        }
    }
}
