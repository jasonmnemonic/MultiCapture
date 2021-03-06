﻿using System;

namespace CameraServer
{
    public static class SynchronousSocketListener
    {
        public static int Main(string[] args)
        {
            Logger.Logger log = new Logger.Logger();
            log.RemoveOldLogs(DateTime.Today.AddMonths(-1));

            Console.WriteLine("Starting");
            try
            {
                CameraSettings.Init();
                Listener listen = new Listener();
                listen.StartListening();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("CRITICAL EXCEPTION SHUTTING DOWN!!!");
            }
            return 0;
        }
    }
}