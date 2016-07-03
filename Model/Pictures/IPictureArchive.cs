using System;
using Microsoft.SPOT;
using Gadgeteer;

namespace DotNetGadgeteerCam.Model.Pictures
{
    public interface IPictureArchive
    {
        Picture CurrentPicture { get; set; }
        DateTime CurrentPictureTaken { get; }

        Picture GetArchivedPicture(string name);
    }
}
