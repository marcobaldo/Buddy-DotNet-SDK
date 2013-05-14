using BuddyServiceClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Buddy
{
    public class Blob : BuddyBase
    {
        protected override bool AuthUserRequired
        {
            get{ return true; }
        }

        public long BlobID { get; protected set; }

        public string FriendlyName { get; protected set; }

        public string MimeType { get; protected set;}

        public int FileSize { get; protected set; }

        public string AppTag { get; protected set; }

        public long Owner { get; protected set; }

        public double Latitude { get; protected set; }

        public double Longitude { get; protected set; }

        public DateTime UploadDate { get; protected set; }

        public DateTime LastTouchDate { get; protected set; }

        internal Blob(BuddyClient client, AuthenticatedUser user, InternalModels.DataContract_Blob blob) : base(client)
        { 
            this.BlobID = long.Parse(blob.BlobID, CultureInfo.InvariantCulture);
            this.FriendlyName = blob.FriendlyName;
            this.MimeType = blob.MimeType;
            this.FileSize = int.Parse(blob.FileSize, CultureInfo.InvariantCulture);
            this.AppTag = blob.AppTag;
            this.Owner = long.Parse(blob.Owner, CultureInfo.InvariantCulture);
            this.Latitude = double.Parse(blob.Latitude, CultureInfo.InvariantCulture);
            this.Longitude = double.Parse(blob.Longitude, CultureInfo.InvariantCulture);
            this.UploadDate = DateTime.Parse(blob.UploadDate, CultureInfo.InvariantCulture);
            this.LastTouchDate = DateTime.Parse(blob.LastTouchDate, CultureInfo.InvariantCulture);
        }

        internal void  EditInfoInternal(string friendlyName, string mimeType, string appTag,
            Action<BuddyCallResult<bool>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("BlobID", this.BlobID);
            parameters.Add("FriendlyName", friendlyName);
            parameters.Add("MimeType", mimeType);
            parameters.Add("AppTag", appTag);

            this.Client.Service.CallMethodAsync<string>("Blobs_Blob_EditInfo", parameters, (bcr) =>
            {
                this.Client.Service.CallOnUiThread((state) => 
                    callback(BuddyResultCreator.Create(bcr.Result == "1", bcr.Error)));
            });
        }

        internal void DeleteInternal(Action<BuddyCallResult<bool>> callback)
        {
            this.AuthUser.Blobs.DeleteInternal(this.BlobID, callback);
        }

        internal void GetInternal(Action<BuddyCallResult<Stream>> callback)
        {
            this.AuthUser.Blobs.GetInternal(this.BlobID, callback);
        }
    }
}
