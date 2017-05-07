﻿using System;
using System.ComponentModel;
using Hub.Helpers;
using Hub.Helpers.Interface;
using Hub.Threaded;
using Hub_Client.Desktop_Interconnect;

namespace Hub_Client.Util
{

    //responsible for correctly initializing all the resources required for correct operation
    class Deployer
    {
        public static Deployer Inst
        {
            get
            {
                if (inst == null) new Deployer();
                return inst;
            }
        }
        private static Deployer inst;

        public ICameraManager Manager { get; private set; }
        public SaveLoad.Data SysConfig { get; private set; }

        public Deployer()
        {
            inst = this;
            Logger.Logger logs = new Logger.Logger();
            logs.RemoveOldLogs(DateTime.Today.AddMonths(-1));

            SysConfig = SaveLoad.Load();
            new DesktopThread();
            Manager = new TaskManager(SysConfig);

        }
    }
}