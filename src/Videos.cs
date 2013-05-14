using BuddyServiceClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Buddy
{
    public class Videos : BuddyBase
    {
        public Videos(BuddyClient client, AuthenticatedUser user)
            : base(client, user) { }

        protected override bool AuthUserRequired {
            get { return true; }
        }

        internal void AddInternal(string friendlyName, string mimeType, string appTag, double latitude, double longitude, byte[] videoData, Action<BuddyCallResult<long>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("FriendlyName", friendlyName);
            parameters.Add("MimeType", mimeType);
            parameters.Add("AppTag", appTag);
            parameters.Add("Latitude", latitude);
            parameters.Add("Longitude", longitude);
            parameters.Add("VideoData", new BuddyFile
            {
                Data = new MemoryStream(videoData),
                ContentType = "application/octet-stream",
                Name = "VideoData"
            });

            this.Client.Service.CallMethodAsync<string>("Videos_Video_AddVideo", parameters, (bcr) =>
            {
                long result = -1;
                if (bcr.Result != null)
                {
                    result = long.Parse(bcr.Result);
                }
                callback(BuddyResultCreator.Create(result, bcr.Error));
            });
        }

        internal void AddInternal(string friendlyName, string mimeType, string appTag, double latitude, double longitude, byte[] videoData, Action<BuddyCallResult<Video>> callback)
        {
            AddInternal(friendlyName, mimeType, appTag, latitude, longitude, videoData, (bcr) =>
                {
                    this.GetInfoInternal(bcr.Result, (bdr) =>
                        {
                            callback(bdr);
                        });
                });
        }

        internal void DeleteInternal(long videoID, Action<BuddyCallResult<bool>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("VideoID", videoID);

            this.Client.Service.CallMethodAsync<string>("Videos_Video_DeleteVideo", parameters, (bcr) =>
            {
                bool result = false;
                if (bcr.Result != null)
                {
                    result = bcr.Result == "1";
                }
                callback(BuddyResultCreator.Create<bool>(result, bcr.Error));
            });
        }

        internal void GetInfoInternal(long VideoID, Action<BuddyServiceClient.BuddyCallResult<Video>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("VideoID", VideoID);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Video>("Videos_Video_GetVideoInfo", parameters, (bcr) =>
            {
                Video result = null;
                if (bcr.Result != null)
                {
                    result = new Video(this.Client, this.AuthUser, bcr.Result);
                }
                callback(BuddyResultCreator.Create((Video)result, bcr.Error));
            });
        }

        internal void SearchVideosInternal(string friendlyName, string mimeType, string appTag,
            int searchDistance, double searchLatitude, double searchLongitude, int timeFilter, int recordLimit, Action<BuddyCallResult<IEnumerable<Video>>> callback)
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

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Video[]>("Videos_Video_SearchVideos", parameters, (bcr) =>
            {
                InternalModels.DataContract_Video[] result = null;
                if (bcr.Result != null)
                {
                    result = bcr.Result;
                }
                var lst = new List<Video>();
                foreach (var vid in result) { lst.Add(new Video(this.Client, this.AuthUser, vid)); }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Video>)lst, bcr.Error));
            });
        }

        internal void SearchMyVideosInternal(string friendlyName, string mimeType, string appTag,
            int searchDistance, double searchLatitude, double searchLongitude, int timeFilter, int recordLimit, Action<BuddyCallResult<IEnumerable<Video>>> callback)
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

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Video[]>("Videos_Video_SearchMyVideos", parameters, (bcr) =>
            {
                InternalModels.DataContract_Video[] result = null;
                if (bcr.Result != null)
                {
                    result = bcr.Result;
                }
                var lst = new List<Video>();
                foreach (var vid in result) { lst.Add(new Video(this.Client, this.AuthUser, vid)); }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Video>)lst, bcr.Error));
            });
        }

        internal void GetListInternal(long userID, int recordLimit, Action<BuddyCallResult<IEnumerable<Video>>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("UserID", userID);
            parameters.Add("RecordLimit", recordLimit);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Video[]>("Videos_Video_GetVideoList", parameters, (bcr) =>
            {
                InternalModels.DataContract_Video[] result = null;
                if (bcr.Result != null)
                {
                    result = bcr.Result;
                }
                var lst = new List<Video>();
                foreach (var vid in result) { lst.Add(new Video(this.Client, this.AuthUser, vid)); }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Video>)lst, bcr.Error));
            });
        }

        internal void GetMyListInternal(int recordLimit, Action<BuddyCallResult<IEnumerable<Video>>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("RecordLimit", recordLimit);

            this.Client.Service.CallMethodAsync<InternalModels.DataContract_Video[]>("Videos_Video_GetMyVideoList", parameters, (bcr) =>
            {
                InternalModels.DataContract_Video[] result = null;
                if (bcr.Result != null)
                {
                    result = bcr.Result;
                }
                var lst = new List<Video>();
                foreach (var vid in result) { lst.Add(new Video(this.Client, this.AuthUser, vid)); }
                callback(BuddyServiceClient.BuddyResultCreator.Create((IEnumerable<Video>)lst, bcr.Error));
            });
        }

        internal void GetInternal(long videoID, Action<BuddyCallResult<Stream>> callback)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("UserToken", this.AuthUser.Token);
            parameters.Add("VideoID", videoID);

            this.Client.Service.CallMethodAsync<HttpWebResponse>("Videos_Blog_GetVideo", parameters, (bcr) =>
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
