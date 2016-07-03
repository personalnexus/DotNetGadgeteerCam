using System;
using Microsoft.SPOT;

namespace DotNetGadgeteerCam.Model.Pictures
{
    public interface IPictureTaker
    {
        void TakePicture();
        string PictureTakingInterval { get; }
    }
}
