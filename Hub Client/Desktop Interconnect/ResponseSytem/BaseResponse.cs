﻿using System.Collections.Generic;

namespace Hub.DesktopInterconnect.Responses
{
    abstract class BaseResponse : IResponse
    {
        protected BaseResponse()
        {
            object[] responseTypes = GetType().GetCustomAttributes(typeof(ResponseTypeAttribute), false);

            foreach (ResponseTypeAttribute responseType in responseTypes)
            {
                if (!DesktopThread.Responders.ContainsKey(responseType.Response))
                {
                    DesktopThread.Responders.Add(responseType.Response, this);
                }
            }
        }

        public abstract byte[] GenerateResponse(ScannerCommands command, Dictionary<string, string> parameters);
        public abstract void Reset();
    }
}
