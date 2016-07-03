using System;
using System.IO;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using DotNetGadgeteerCam.Model.Status;
using Gadgeteer;

namespace DotNetGadgeteerCam.Model.Pictures
{
    public class PictureArchive : StatusProviderBase, IPictureArchive
    {
        public PictureArchive(SDCard sdCard, IStatusChecker statusChecker)
            : base(statusChecker, "Picture Archive")
        {
            _sdCard = sdCard;
            _sdCard.SDCardMounted += ProcessCardMounted;
            _sdCard.SDCardUnmounted += sender => IsReady = false;
            _sdCard.UnmountSDCard();
            _sdCard.MountSDCard();
        }

        void ProcessCardMounted(SDCard sender, StorageDevice SDCard)
        {
 	        _fileNamePrefix = _sdCard.GetStorageDevice().RootDirectory + "\\Pictures\\";
            Log("SD card mounted, files will be saved to " + _fileNamePrefix);
            IsReady = (_sdCard.IsCardInserted) && (_sdCard.IsCardMounted);
        }

        private SDCard _sdCard;
        private string _fileNamePrefix;
        private string _fileExtension = ".bmp";

        private Picture _currentPicture;
        private DateTime _currentPictureTaken;
        
        //
        // IPictureArchive
        //

        public Picture GetArchivedPicture(string name)
        {
            Picture result;
            string fileName = _fileNamePrefix + name + _fileExtension;
            if (!File.Exists(fileName))
            {
                result = CurrentPicture;
            }
            else
            {
                byte[] pictureData = File.ReadAllBytes(fileName);
                result = new Picture(pictureData, Picture.PictureEncoding.BMP);
            }
            return result;
        }

        public Picture CurrentPicture
        {
            get
            {
                return _currentPicture;
            }
            set
            {
                _currentPicture = value;
                _currentPictureTaken = DateTime.Now;
                string fileName = _fileNamePrefix + _currentPictureTaken.ToString("yyyy-MM-dd_hh-mm-ss") + _fileExtension;
                if (!IsReady)
                {
                    Log("Cannot save picture " + fileName + ", because SD card is not ready");
                }
                else
                {
                    Log("Saving picture " + fileName + "...");
                    System.IO.File.WriteAllBytes(fileName, _currentPicture.PictureData);
                    Log("Picture " + fileName + " saved");
                }
            }
        }

        public DateTime CurrentPictureTaken { get { return _currentPictureTaken; } }
    }
}
