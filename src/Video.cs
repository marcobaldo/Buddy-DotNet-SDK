using BuddyServiceClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Buddy
{
    public class Video : BuddyBase
    {
        protected override bool AuthUserRequired
        {
            get{ return true; }
        }

        public long VideoID { get; protected set; }

        public string FriendlyName { get; protected set; }

        public string MimeType { get; protected set; }

        public int FileSize { get; protected set; }

        public string AppTag { get; protected set; }

        public long Owner { get; protected set; }

        public double Latitude { get; protected set; }

        public double Longitude { get; protected set; }

        public DateTime UploadDate { get; protected set; }

        public DateTime LastTouchDate { get; protected set; }

        internal Video(BuddyClient client, AuthenticatedUser user, InternalModels.DataContract_Video video) : base(client)
        { 
            this.VideoID = long.Parse(video.VideoID, CultureInfo.InvariantCulture);
            this.FriendlyName = video.FriendlyName;
            this.MimeType = video.MimeType;
            this.FileSize = int.Parse(video.FileSize, CultureInfo.InvariantCulture);
            this.AppTag = video.AppTag;
            this.Owner = long.Parse(video.Owner, CultureInfo.InvariantCulture);
            this.Latitude = double.Parse(video.Latitude, CultureInfo.InvariantCulture);
            this.Longitude = double.Parse(video.Longitude, CultureInfo.InvariantCulture);
            this.UploadDate = DateTime.Parse(video.UploadDate, CultureInfo.InvariantCulture);
            this.LastTouchDate = DateTime.Parse(video.LastTouchDate, CultureInfo.InvariantCulture);
        }

        internal void EditInfoInternal(string friendlyName, string mimeType, string appTag,
            Action<BuddyCallResult<bool>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("VideoID", this.VideoID);
            parameters.Add("FriendlyName", friendlyName);
            parameters.Add("MimeType", mimeType);
            parameters.Add("AppTag", appTag);

            this.Client.Service.CallMethodAsync<string>("Videos_Video_EditInfo", parameters, (bcr) =>
            {
                this.Client.Service.CallOnUiThread((state) => 
                    callback(BuddyResultCreator.Create(bcr.Result == "1", bcr.Error)));
            });
        }

        internal void DeleteInternal(Action<BuddyCallResult<bool>> callback)
        {
            this.AuthUser.Videos.DeleteInternal(this.VideoID, callback);
        }

        internal void GetInternal(Action<BuddyCallResult<Stream>> callback)
        {
            this.AuthUser.Videos.GetInternal(this.VideoID, callback);
        }
    }
}
