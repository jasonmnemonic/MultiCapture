﻿using System;
using System.Text;
using Hub.Networking;

namespace SharedDeviceItems.Networking.CameraHubConnection
{
    public class ChunkRequester : IRequester
    {
        private ISocket socket;
        private const int chunkSize = 20000;

        public ChunkRequester(ISocket socket)
        {
            this.socket = socket;
        }

        public byte[] Request(CameraRequest request)
        {
            return Request(Encoding.ASCII.GetBytes(((int)request).ToString()));
        }

        public byte[] Request(byte[] requestData)
        {
            if (!socket.Connected) throw new SocketNotConnectedException();

            //format the request data and send it to the camera
            {
                byte[] correct = InterconnectHelper.FormatSendData(requestData);
                socket.Send(correct);
            }

            //recieve data and return the result
            try
            {
                byte[] buffer = new byte[Constants.CameraBufferSize];
                int recieved = socket.Receive(buffer);
                byte[] sizeData = InterconnectHelper.RecieveData(buffer, recieved, socket);

                int dataSize = BitConverter.ToInt32(sizeData, 0);

                //send back the chunk size for the rest of the data
                sizeData = BitConverter.GetBytes(chunkSize);
                socket.Send(InterconnectHelper.FormatSendData(sizeData));

                buffer = new byte[Constants.HubBufferSize];

                //assemble the chunk array
                int chunkAmount = dataSize / chunkSize;
                if(dataSize % chunkSize != 0) ++chunkAmount;
                byte[] returnData = new byte[dataSize];

                for(int i = 0; i < chunkAmount; i++)
                {
                    socket.Send(InterconnectHelper.FormatSendData(BitConverter.GetBytes(i)));

                    recieved = socket.Receive(buffer);
                    sizeData = InterconnectHelper.RecieveData(buffer, recieved, socket);

                    Array.Copy(sizeData, 0, returnData, i * chunkSize, sizeData.Length);
                }

                return returnData;
            }
            finally
            {
                socket.Send(InterconnectHelper.FormatSendData(Constants.EndTransferBytes));
            }
        }

        public int ClearSocket()
        {
            int count = 0;

            byte[] throwAway = new byte[8000];
            while (socket.Available > 0) count += socket.Receive(throwAway);

            return count;
        }
    }
}