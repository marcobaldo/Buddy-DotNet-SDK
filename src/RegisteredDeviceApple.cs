﻿using System;
using System.Net;
using BuddyServiceClient;

namespace Buddy
{
    public class RegisteredDeviceApple
    {
        protected AuthenticatedUser User { get; set; }

        public string APNSDeviceToken { get; protected set; }
        public string GroupName { get; protected set; }
        public DateTime LastUpdateDate { get; protected set; }
        public DateTime RegistrationDate { get; protected set; }
        public int UserID { get; protected set; }

        internal RegisteredDeviceApple(InternalModels.DataContract_AppleDeviceList device, AuthenticatedUser user)
        {
            if (device == null) throw new ArgumentNullException("device");
            if (user == null) throw new ArgumentNullException("user");

            this.UserID = Int32.Parse(device.UserID);
            this.APNSDeviceToken = device.APNSDeviceToken;
            this.GroupName = device.GroupName;
            this.LastUpdateDate = device.DeviceModified;
            this.RegistrationDate = device.DeviceRegistered;
            this.User = user;
        }
    }
}
