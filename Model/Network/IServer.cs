using System;
using Microsoft.SPOT;
using DotNetGadgeteerCam.Model.Pictures;
using DotNetGadgeteerCam.Model.Status;

namespace DotNetGadgeteerCam.Model.Network
{
    public interface IServer
    {
        void Connect();
        bool IsReady { get; }
    }
}
