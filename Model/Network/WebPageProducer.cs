using System;
using Microsoft.SPOT;
using DotNetGadgeteerCam.Model.Pictures;
using DotNetGadgeteerCam.Model.Status;
using Gadgeteer;
using Gadgeteer.Networking;

namespace DotNetGadgeteerCam.Model.Network
{
    public class WebPageProducer: StatusProviderBase
    {
        public WebPageProducer(IPictureTaker pictureTaker, IPictureArchive pictureArchive, IStatusChecker statusChecker, string name)
            : base(statusChecker, name)
        {
            _pictureArchive = pictureArchive;
            _pictureTaker = pictureTaker;
        }

        private IPictureArchive _pictureArchive;
        private IPictureTaker _pictureTaker;

        public const string MainPageUrl = "index.htm";
        public const string ArchivePageUrl = "archive.htm";
        public const string StatusPageUrl = "status.htm";
        public const string ArchivedPictureUrl = "image.htm";
        public const string ArchivedPictureParameterName = "name";
        public const string CurrentPictureUrl = "CurrentPicture.bmp";

        private string GetPage(string pageTitle, string pageBody)
        {
            return @"
<!DOCTYPE html>
<html>
  <head>
    <title>DotNetGadgeteerCam - " + pageTitle + @"</title>
    <meta http-equiv=""refresh"" content=""" + _pictureTaker.PictureTakingInterval + @""">
  </head>
  <body>
   <h1>" + pageTitle + @"</h1>
        " + pageBody + @"
   <hr />
   <p><center><a href=""" + MainPageUrl + @""">Main Page</a> --- <a href=""" + ArchivePageUrl + @""">Picture Archive</a> --- <a href=""" + StatusPageUrl + @""">Status Overview</a></center></p>
  </body>
</html>";
        }

        public string GetMainPage()
        {
            string result = GetPage("Main Page", 
@"<p>This is what my DotNetGadgeteer was looking at at " + _pictureArchive.CurrentPictureTaken.ToString("dd MMM yyyy, hh:mm:ss") + @". Page refreshes every " + _pictureTaker.PictureTakingInterval + @" seconds.</p>
  <p><img src=""CurrentPicture.bmp"" alt=""The world as DotNetGadgeteer sees it""/></p>");
            return result;
        }

        public string GetArchivePage()
        {
            string result = GetPage("Archive", @"<p>Coming soon...</p>");
            return result;
        }

        public string GetStatusPage()
        {
            string pageBody = "<p><ul>\n";
            foreach (IStatusProvider statusProvider in StatusChecker.StatusProviders)
            {
                pageBody += "<li><b>" + statusProvider.Name + ":</b> " + (statusProvider.IsReady ? "Ready" : "Not Ready") + "</li>\n";
            }
            pageBody += "</ul></p>";
            string result = GetPage("Status Overview", pageBody);
            return result;
        }

        public Picture GetArchivedPicture(string name)
        {
            return _pictureArchive.GetArchivedPicture(name);
        }

        public Picture GetCurrentPicture()
        {
            return _pictureArchive.CurrentPicture;
        }
    }
}
