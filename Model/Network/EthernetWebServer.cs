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
    public class EthernetWebServer: WebServerBase
    {
        public EthernetWebServer(Ethernet_J11D ethernetInterface, IPictureTaker pictureTaker, IPictureArchive pictureArchive, IStatusChecker statusChecker)
            : base(ethernetInterface, pictureTaker, pictureArchive, statusChecker, "Ethernet Web Server")
        {
        }
    }
}
