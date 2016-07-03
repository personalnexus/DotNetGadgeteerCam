using System;
using Microsoft.SPOT;

namespace DotNetGadgeteerCam.Model.Status
{
    public interface IStatusProvider
    {
        string Name { get; }
        bool IsReady { get; }
    }
}
