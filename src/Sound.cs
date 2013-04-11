using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;


namespace Buddy
{
    /// <summary>
    /// An object that can be used to save files. 
    /// </summary>
    public class Sounds : BuddyBase
    {

        internal Sounds(BuddyClient client)
            : base(client)
        {

        }

        /// <summary>Defines available sound quality levels for each sound file.</summary>
        public enum SoundQuality {
            Low,
            Medium,
            High
        }



    

        /// <summary>
        /// Retrieves a sound from the Buddy sound library, and returns a Stream.  Your application should perisist this stream locally in a location such as IsolatedStorage.
        /// </summary>
        /// <param name="callback">Callback that will be invoked upon completion.</param>
        /// <param name="soundName">The name of the sound file.  See the Buddy Developer Portal "Sounds" page to find sounds and get their names.</param>
        /// <param name="quality">The quality level of the file to retrieve.</param>  
        [Obsolete("This method has been deprecated, please call one of the other overloads of GetSoundAsync.")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public IAsyncResult GetSoundAsync(Action<Stream, BuddyCallbackParams> callback, string soundName, SoundQuality quality)
        {
            GetSoundInternal(soundName, quality, (bcr) =>
            {
                callback(bcr.Result, new BuddyCallbackParams(bcr.Error));
            });

            return null;
        }


        internal void GetSoundInternal(string soundName, SoundQuality quality, Action<BuddyServiceClient.BuddyCallResult<Stream>> callback)
        {

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("BuddyApplicationName", this.Client.AppName);
            parameters.Add("BuddyApplicationPassword", this.Client.AppPassword);
            parameters.Add("SoundName", soundName);
            parameters.Add("Quality", quality);

            this.Client.Service.CallMethodAsync<HttpWebResponse>("Sound_Sounds_GetSound", parameters, (bcr) =>
            {
                Stream result = null;
                if (bcr.Result != null)
                {
                    result = bcr.Result.GetResponseStream();
                }
                callback( BuddyServiceClient.BuddyResultCreator.Create(result, bcr.Error));
            });
        }
    }
}
