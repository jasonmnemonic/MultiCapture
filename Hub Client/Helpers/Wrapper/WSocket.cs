﻿using System;
using System.Net;
using System.Net.Sockets;
using Hub.Networking;

namespace Hub.Helpers.Wrapper
{
    /// <summary>
    /// wrapper for the standard C# socket
    /// </summary>
    public class WSocket : ISocket
    {
        private Socket socket;

        public WSocket(AddressFamily family, SocketType type, ProtocolType protocal)
        {
            socket = new Socket(family, type, protocal);
        }
        public void Connect(IPEndPoint endPoint)
        {
            socket.Connect(endPoint);
        }

        public void Send(byte[] data)
        {
            socket.Send(data);
        }

        public int Receive(byte[] buffer)
        {
            return socket.Receive(buffer);
        }

        public void Shutdown(SocketShutdown shutdown)
        {
            socket.Shutdown(shutdown);
        }

        public void Close()
        {
            socket.Close();
        }

        public bool Poll(int timeout, SelectMode mode)
        {
            return socket.Poll(timeout, mode);
        }

        public int Available => socket.Available;
        public bool Connected => socket.Connected;

        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback,
            object state)
        {
            return socket.BeginSend(buffer, offset, size, socketFlags, callback, state);
        }

        public int EndReceive(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }

        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback,
            object state)
        {
            return BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        public IAsyncResult BeginConnect(EndPoint remoteEp, AsyncCallback callback, object state)
        {
            return socket.BeginConnect(remoteEp, callback, state);
        }
    }
}