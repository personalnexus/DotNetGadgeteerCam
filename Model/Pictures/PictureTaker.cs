using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using DotNetGadgeteerCam.Model.Status;
using Gadgeteer;

namespace DotNetGadgeteerCam.Model.Pictures
{
    public class PictureTaker : StatusProviderBase, IPictureTaker
    {
        public PictureTaker(Camera camera, IPictureArchive pictureArchive, IStatusChecker statusChecker): base(statusChecker, "Camera")
        {
            _camera = camera;
            _pictureArchive = pictureArchive;
            _dispatcher = Microsoft.SPOT.Dispatcher.CurrentDispatcher;
            var pictureTakingInterval = new TimeSpan(0, 0, 30);
            PictureTakingInterval = pictureTakingInterval.Seconds.ToString();
            _camera.PictureCaptured += PictureCaptured;
            new System.Threading.Timer(TakePictureAsync, null, new TimeSpan(0, 0, 1), pictureTakingInterval);
        }

        private Camera _camera;
        private IPictureArchive _pictureArchive;
        private Dispatcher _dispatcher;
        private bool _captureInProgress;

        private void TakePictureAsync(object state)
        {
            _dispatcher.BeginInvoke(TakePicture, state);
        }

        void PictureCaptured(Camera sender, Picture picture)
        {
            _captureInProgress = false;
            _pictureArchive.CurrentPicture = picture;
        }

        //
        // IPictureTaker
        //

        private object TakePicture(object state)
        {
            TakePicture();
            return state;
        }

        public void TakePicture()
        {
            if (_captureInProgress)
            {
                Log("Picture is already being taken, not taking another one right now");
            }
            else
            {
                if (!IsReady)
                {
                    Log("Camera not ready, no picture was taken");
                }
                else
                {
                    Log("Taking a picture...");
                    _captureInProgress = true;
                    _camera.TakePicture();
                }
            }
        }

        public string PictureTakingInterval { get; private set; }

        //
        // IStatusProvider
        //

        public override bool IsReady { get { return _camera.CameraReady; } set { base.IsReady = value; } }
    }
}
