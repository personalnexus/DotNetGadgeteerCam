using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;
using DotNetGadgeteerCam.Model.Status;
using DotNetGadgeteerCam.Model.Pictures;
using DotNetGadgeteerCam.Model;
using DotNetGadgeteerCam.Model.Network;
using DotNetGadgeteerCam.Model.Interaction;

namespace DotNetGadgeteerCam
{
    public partial class Program
    {
        void ProgramStarted()
        {
            var statusChecker = new StatusChecker(_led);
            var pictureArchive = new PictureArchive(_sdCard, statusChecker);
            var pictureTaker = new PictureTaker(_camera, pictureArchive, statusChecker);
            var interactor = new Interactor(_wifiButton, _ethernetButton, pictureTaker, statusChecker,
                                            () => new WifiWebServer(_wifi, pictureTaker, pictureArchive, statusChecker),
                                            () => new EthernetWebServer(_ethernet, pictureTaker, pictureArchive, statusChecker));
            statusChecker.Check();
        }
    }
}
