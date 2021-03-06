﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hub.DesktopInterconnect;

[assembly: InternalsVisibleTo("Hub ClientTests")]
namespace Hub.ResponseSystem.Responses
{
    [ResponseType(ScannerCommands.ApiVersion)]
    internal class ApiResponse : BaseResponse
    {
        public override byte[] GenerateResponse(ScannerCommands command, Dictionary<string, string> parameters)
        {
            return ResponseConstants.ApiResponse;
        }
    }
}
