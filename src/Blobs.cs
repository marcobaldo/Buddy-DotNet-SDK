using BuddyServiceClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Buddy
{
    /// <summary>
    /// Represents a class that can be used to add, retrieve or delete Blobs.
    /// <example>
    /// <code>
    ///     BuddyClient client = new BuddyClient("APPNAME", "APPPASS");
    ///     client.LoginAsync((user, state) => {
    ///       
    ///         user.);
    /// </code>
    /// </example>
    /// </summary>
    public class Blobs : BuddyBase
    {
        public Blobs(BuddyClient client, AuthenticatedUser user)
            : base(client, user) { }

        protected override bool AuthUserRequired{
            get{ return true; }
        }

        internal void AddInternal(string friendlyName, string mimeType, string appTag, double latitude, double longitude, byte[] blobData, Action<BuddyCallResult<long>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("FriendlyName", friendlyName);
            parameters.Add("AppTag", appTag);
            parameters.Add("Latitude", latitude);
            parameters.Add("Longitude", longitude);
            parameters.Add("BlobData", new BuddyFile { Data = new MemoryStream(blobData), 
                                                        ContentType = mimeType,
                                                        Name = "BlobData"});

            this.Client.Service.CallMethodAsync<string>("Blobs_Blob_AddBlob", parameters, (bcr) =>
            {
                long result = -1;
                if (bcr.Result != null)
                {
                    result = long.Parse(bcr.Result);
                }
                callback(BuddyResultCreator.Create(result, bcr.Error));
            });
        }

        internal void AddInternal(string friendlyName, string mimeType, string appTag, double latitude, double longitude, byte[] blobData, Action<BuddyCallResult<Blob>> callback)
        {
            AddInternal(friendlyName, mimeType, appTag, latitude, longitude, blobData, (bcr) =>
                {
                    this.GetInfoInternal(bcr.Result, (bdr) =>
                    {
                        callback(bdr);
                    });
                });
        }

        internal void DeleteInternal(long blobID, Action<BuddyCallResult<bool>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("BlobID", blobID);

            this.Client.Service.CallMethodAsync<string>("Blobs_Blob_DeleteBlob", parameters, (bcr) =>
            {
                bool result = false;
                if (bcr.Result != null)
                {
                    result = bcr.Result == "1";
                }
                callback(BuddyResultCreator.Create<bool>(result, bcr.Error));
            });
        }

        internal void GetInfoInternal(long blobID, Action<BuddyServiceClient.BuddyCallResult<Blob>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("BlobID", blobID);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Blob>("Blobs_Blob_GetBlobInfo", parameters, (bcr) =>
            {
                Blob result = null;
                if (bcr.Result != null)
                {
                    result = new Blob(this.Client, this.AuthUser, bcr.Result);
                }
                callback(BuddyResultCreator.Create((Blob)result, bcr.Error));
            });
        }

        internal void SearchBlobsInternal(string friendlyName, string mimeType, string appTag,
            int searchDistance, double searchLatitude, double searchLongitude, int timeFilter, int recordLimit, Action<BuddyCallResult<IEnumerable<Blob>>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("FriendlyName", friendlyName);
            parameters.Add("MimeType", mimeType);
            parameters.Add("AppTag", appTag);
            parameters.Add("SearchDistance", searchDistance);
            parameters.Add("SearchLatitude", searchLatitude);
            parameters.Add("SearchLongitude", searchLongitude);
            parameters.Add("TimeFilter", timeFilter);
            parameters.Add("RecordLimit", recordLimit);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Blob[]>("Blobs_Blob_SearchBlobs", parameters, (bcr) =>
            {
                List<Blob> result = null;
                if (bcr.Result != null)
                {
                    foreach (InternalModels.DataContract_Blob b in bcr.Result)
                    {
                        result.Add(new Blob(this.Client, this.AuthUser, b));
                    }
                }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Blob>)result, bcr.Error));
            });
        }

        internal void SearchMyBlobsInternal(string friendlyName, string mimeType, string appTag,
            int searchDistance, double searchLatitude, double searchLongitude, int timeFilter, int recordLimit, Action<BuddyCallResult<IEnumerable<Blob>>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("FriendlyName", friendlyName);
            parameters.Add("MimeType", mimeType);
            parameters.Add("AppTag", appTag);
            parameters.Add("SearchDistance", searchDistance);
            parameters.Add("SearchLatitude", searchLatitude);
            parameters.Add("SearchLongitude", searchLongitude);
            parameters.Add("TimeFilter", timeFilter);
            parameters.Add("RecordLimit", recordLimit);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Blob[]>("Blobs_Blob_SearchMyBlobs", parameters, (bcr) =>
            {
                List<Blob> result = null;
                if (bcr.Result != null)
                {
                    foreach (var b in bcr.Result)
                    {
                        result.Add(new Blob(this.Client, this.AuthUser, b));
                    }
                }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Blob>)result, bcr.Error));
            });
        }

        internal void GetListInternal(long userID, int recordLimit, Action<BuddyCallResult<IEnumerable<Blob>>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("UserID", userID);
            parameters.Add("RecordLimit", recordLimit);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Blob[]>("Blobs_Blob_GetBlobList", parameters, (bcr) =>
            {
                List<Blob> result = null;
                if (bcr.Result != null)
                {
                    foreach (var b in bcr.Result)
                    {
                        result.Add(new Blob(this.Client, this.AuthUser, b));
                    }
                }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Blob>)result, bcr.Error));
            });
        }

        internal void GetMyListInternal(int recordLimit, Action<BuddyCallResult<IEnumerable<Blob>>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("RecordLimit", recordLimit);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Blob[]>("Blobs_Blob_GetMyBlobList", parameters, (bcr) =>
            {
                List<Blob> result = null;
                if (bcr.Result != null)
                {
                    foreach (var b in bcr.Result)
                    {
                        result.Add(new Blob(this.Client, this.AuthUser, b));
                    }
                }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Blob>)result, bcr.Error));
            });
        }

        internal void GetInternal(long blobID, Action<BuddyCallResult<Stream>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("BlobID", blobID);

            this.Client.Service.CallMethodAsync<HttpWebResponse>("Blobs_Blog_GetBlob", parameters, (bcr) =>
            {
                Stream result = null;
                if (bcr.Result != null)
                {
                    result = bcr.Result.GetResponseStream();
                }
                callback(BuddyServiceClient.BuddyResultCreator.Create(result, bcr.Error));
            });
        }
    }
}
