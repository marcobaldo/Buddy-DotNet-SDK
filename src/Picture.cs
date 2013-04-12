using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using BuddyServiceClient;
using System.Collections.ObjectModel;

namespace Buddy
{
    /// <summary>
    /// Represents a single picture on the Buddy Platform. Pictures can be accessed through an AuthenticatedUser, either by using the PhotoAlbums property to retrieve
    /// Pictures that belong to the user, or using the SearchForAlbums method to find public Pictures.
    /// <example>
    /// <code>
    ///     BuddyClient client = new BuddyClient("APPNAME", "APPPASS");
    ///     client.LoginAsync((user, state) => {
    ///         user.SearchForAlbums((albums, state2 => {
    ///             Picture p = album.Pictures[0];
    ///         });
    ///     }, "username", "password");
    /// </code>
    /// </example>
    /// </summary>
    public class Picture : PicturePublic
    {
        protected override bool AuthUserRequired {
            get {
                return true;
            }
        }

        internal Picture (BuddyClient client, string fullUrl, string thumbnailUrl, double latitude, double longitude, string comment, string appTag,
                       DateTime addedOn, int photoId, AuthenticatedUser user)
            : base(client, fullUrl, thumbnailUrl, latitude, longitude, comment, appTag, addedOn, photoId, user)
        {

        }

        internal Picture (BuddyClient client, AuthenticatedUser user, DataContract_PhotoList photo)
            : base(client, photo.FullPhotoURL, photo.ThumbnailPhotoURL, client.TryParseDouble(photo.Latitude),
            client.TryParseDouble(photo.Longitude), photo.PhotoComment, photo.ApplicationTag, photo.AddedDateTime,
            Int32.Parse(photo.PhotoID), user)
        {

        }

      

        /// <summary>
        /// Delete this picture. Note that this object will no longer be valid after this method is called. Subsequent calls will fail.
        /// </summary>
        /// <param name="callback">The async callback to call on success or error. The first parameter is true if the picture was deleted, false otherwise.</param>
        /// <param name="state">An optional user defined object that will be passed to the callback.</param>
        /// <returns>An IAsyncResult handle that can be used to monitor progress on this call.</returns>
        #if AWAIT_SUPPORTED
	[Obsolete("This method has been deprecated, please call one of the other overloads of DeleteAsync.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
#endif
        public IAsyncResult DeleteAsync (Action<bool, BuddyCallbackParams> callback, object state = null)
        { 
            DeleteInternal ((bcr) => {
                if (callback == null)
                    return;
                callback (bcr.Result, new BuddyCallbackParams (bcr.Error)); });
            return null; 
        }

        internal void DeleteInternal (Action<BuddyCallResult<bool>> callback)
        {
            if (String.IsNullOrEmpty (this.AuthUser.Token))
                throw new Exception ("Can't delete photos of other users.");

            this.Client.Service.Pictures_Photo_Delete (this.Client.AppName, this.Client.AppPassword, this.AuthUser.Token,
                        this.PhotoID.ToString (), (bcr) =>
            {
                var result = bcr.Result;
                if (bcr.Error != BuddyError.None && bcr.Error != BuddyError.ServiceErrorNegativeOne) {
                    callback (BuddyResultCreator.Create (false, bcr.Error));
                    return;
                }
                callback (BuddyResultCreator.Create (result == "1", BuddyError.None));
                return;
            });
            return;

        }

       

        /// <summary>
        /// Returns a list of supported filters that can be applied to this picture. Example filters are: Hue Shift, Crop, etc.
        /// </summary>
        /// <param name="callback">The async callback to call on success or error. The first parameter is a list of supported filters.</param>
        /// <param name="state">An optional user defined object that will be passed to the callback.</param>
        /// <returns>An IAsyncResult handle that can be used to monitor progress on this call.</returns>
        #if AWAIT_SUPPORTED
	[Obsolete("This method has been deprecated, please call one of the other overloads of SupportedFiltersAsync.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
#endif
        public IAsyncResult SupportedFiltersAsync (Action<Dictionary<string, string>, BuddyCallbackParams> callback, object state = null)
        { 
            SupportedFiltersInternal ((bcr) => {
                if (callback == null)
                    return;
                callback (bcr.Result, new BuddyCallbackParams (bcr.Error)); });
            return null; 
        }

        internal void SupportedFiltersInternal (Action<BuddyCallResult<Dictionary<string, string>>> callback)
        {
            this.Client.Service.Pictures_Filters_GetList (this.Client.AppName, this.Client.AppPassword, (bcr) =>
            {
                var result = bcr.Result;
                if (bcr.Error != BuddyError.None) {
                    callback (BuddyResultCreator.Create<Dictionary<string, string>> (null, bcr.Error));
                    return;
                }
                Dictionary<string, string> lst = new Dictionary<string, string> ();
                foreach (var d in result)
                    lst.Add (d.FilterName, d.ParameterList);
                {
                    callback (BuddyResultCreator.Create (lst, bcr.Error));
                    return; }
                ;
            });
            return;

        }

       

        /// <summary>
        /// Apply a filter to this picture. A new picture is created and returned after the filter is applied. Note that this method internally does two web-service calls, and the IAsyncResult object
        /// returned is only valid for the first one.
        /// </summary>
        /// <param name="filterName">The name of the filter to apply. Cant't be null or empty.</param>
        /// <param name="filterParams">A semi-colon seperated list of filter parameter names and values. For example: "CropLeft=30;CropRight=40."</param>
        /// <param name="callback">The async callback to call on success or error. The first parameter is a new picture with the filter applied.</param>
        /// <param name="state">An optional user defined object that will be passed to the callback.</param>
        /// <returns>An IAsyncResult handle that can be used to monitor progress on this call.</returns>
        #if AWAIT_SUPPORTED
	[Obsolete("This method has been deprecated, please call one of the other overloads of ApplyFilterAsync.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
#endif
        public IAsyncResult ApplyFilterAsync (string filterName, string filterParams, Action<Picture, BuddyCallbackParams> callback, object state = null)
        { 
            ApplyFilterInternal (filterName, filterParams, (bcr) => {
                if (callback == null)
                    return;
                callback (bcr.Result, new BuddyCallbackParams (bcr.Error)); });
            return null; 
        }

        internal void ApplyFilterInternal (string filterName, string filterParams, Action<BuddyCallResult<Picture>> callback)
        {
            if (String.IsNullOrEmpty (filterName))
                throw new ArgumentException ("Can't be null or empty.", "filterName");
            if (filterParams == null)
                throw new ArgumentNullException ("filterParams");
            this.Client.Service.Pictures_Filters_ApplyFilter (this.Client.AppName, this.Client.AppPassword, this.AuthUser.Token, this.PhotoID.ToString (),
                    filterName, filterParams, "0", (bcr) =>
            {
                var result = bcr.Result;
                if (bcr.Error != BuddyError.None) {
                    callback (BuddyResultCreator.Create<Picture> (null, bcr.Error));
                    return;
                }
                int trash;
                if (Int32.TryParse (result, out trash)) {
                    this.AuthUser.GetPictureInternal (Int32.Parse (result), callback);
                } else {
                }

                {
                    callback (BuddyResultCreator.Create<Picture> (null, bcr.Error));
                    return; }
                ;
            });
            return;

        }

      

        /// <summary>
        /// Sets the app tag on this picture.
        /// </summary>
        /// <param name="appTag">The app tag.</param>
        /// <param name="callback">The async callback to call on success or error. The first parameter is true if the app tag was added, false otherwise.</param>
        /// <param name="state">An optional user defined object that will be passed to the callback.</param>
        /// <returns>An IAsyncResult handle that can be used to monitor progress on this call.</returns>
        #if AWAIT_SUPPORTED
	[Obsolete("This method has been deprecated, please call one of the other overloads of SetAppTagAsync.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
#endif
        public IAsyncResult SetAppTagAsync (Action<bool, BuddyCallbackParams> callback, string appTag, object state = null)
        { 
            SetAppTagInternal (appTag, (bcr) => {
                if (callback == null)
                    return;
                callback (bcr.Result, new BuddyCallbackParams (bcr.Error)); });
            return null; 
        }

        internal void SetAppTagInternal (string appTag, Action<BuddyCallResult<bool>> callback)
        {
            this.Client.Service.Pictures_Photo_SetAppTag (this.Client.AppName, this.Client.AppPassword, this.AuthUser.Token, this.PhotoID.ToString (), appTag, (bcr) =>
            {
                var result = bcr.Result;
                if (bcr.Error != BuddyError.None && bcr.Error != BuddyError.ServiceErrorNegativeOne) {
                    callback (BuddyResultCreator.Create (false, bcr.Error));
                    return;
                }
                callback (BuddyResultCreator.Create (result == "1", BuddyError.None));
                return;
            });
            return;

        }
    }
}
