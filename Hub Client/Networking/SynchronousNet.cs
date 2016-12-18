﻿using System;
using System.Text;
using Hub.Helpers;
using SharedDeviceItems;

namespace Hub.Networking
{
    public class SynchronousNet : INetwork
    {
        private const int bufferSize = 400000;
        private ISocket socket;

        public SynchronousNet(ISocket socket)
        {
            this.socket = socket;
        }

        public byte[] MakeRequest(CameraRequest request)
        {
            byte[] bytes = new byte[Constants.ByteArraySize],
                buffer = new byte[bufferSize];

            if (!socket.Connected) throw new Exception("Socket needs to be connnected");
            try
            {
                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes((int)request + Constants.EndOfMessage);
                socket.Send(msg);

                //grab the bytes
                bytes = new byte[Constants.ByteArraySize];
                int totalData = 0;
                do
                {
                    int bytesRec = socket.Receive(buffer);
                    Array.Copy(buffer, 0, bytes, totalData, bytesRec);
                    totalData += bytesRec;
                } while (!ByteManipulation.SearchEndOfMessage(bytes, totalData));

                return Helpers.Networking.TrimExcessByteData(bytes, totalData - 1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public byte[] MakeRequest(byte[] requestData)
        {
            byte[] bytes = new byte[Constants.ByteArraySize],
                buffer = new byte[bufferSize];

            if (!socket.Connected) throw new Exception("Socket needs to be connnected");
            try
            {
                // Encode the data string into a byte array.
                socket.Send(requestData);

                //grab the bytes
                bytes = new byte[Constants.ByteArraySize];
                int totalData = 0;
                do
                {
                    int bytesRec = socket.Receive(buffer);
                    Array.Copy(buffer, 0, bytes, totalData, bytesRec);
                    totalData += bytesRec;
                } while (!ByteManipulation.SearchEndOfMessage(bytes, totalData));

                return Helpers.Networking.TrimExcessByteData(bytes, totalData - 1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
