using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Modal;
using ImageService.Server;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        private EventLog eventLog;
        private int eventId = 1;
        private ILoggingModal logger;
        private ImageServer server;

        public ImageService()
        {
            InitializeComponent();
            //initialize event log
            eventLog = new EventLog();
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog.Source = "MySource";
            eventLog.Log = "MyNewLog";
        }
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            eventLog.WriteEntry("Starting service...");
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Monitor);
            timer.Start();
            //create logger
            logger = new LoggingModal();
            logger.MessageRecieved += onMsg;
            //create server
            server = new ImageServer(logger);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog.WriteEntry("Service started.");
        }

        private void Logger_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnStop()
        {
            eventLog.WriteEntry("Stopping service..");
            eventLog.WriteEntry("Service Stopped.");
        }

        public void Monitor(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);

        }

        public void onMsg(object sender, MessageRecievedEventArgs message)
        {
            eventLog.WriteEntry(message.Message);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
}
