﻿using System;
using SharedDeviceItems;

namespace Hub.Networking
{
    [Obsolete("Use the IRequester interface implementations instead", true)]
    internal interface INetwork
    {
        byte[] MakeRequest(CameraRequest request);
        byte[] MakeRequest(Byte[] requestData);
    }

    public class StateObject
    {
        public ISocket WorkSocket = null;
        public byte[] Buffer = new byte[Constants.HubBufferSize];

        public int Saved = 0;
    }
}
