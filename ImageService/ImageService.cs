using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Modal;
using ImageService.Server;
using System.Configuration;
using SharedInfo.Messages;
using SharedInfo.Commands;
using System.Threading.Tasks;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        private EventLog eventLog;  //event log for the service
        private int eventId = 1;    //used to track number of events since initialization
        public ILoggingModal logger;
        private ImageServer server; // the server that listens to the directories
        private ServiceServer serviceServer; // the server that communicate with gui
        public static bool started = false;

        public ImageService()
        {
            InitializeComponent();
            //initialize event log
            eventLog = new EventLog();
            if (!EventLog.SourceExists(ConfigurationManager.AppSettings["SourceName"]))
            {
                EventLog.CreateEventSource(
                    ConfigurationManager.AppSettings["SourceName"], ConfigurationManager.AppSettings["LogName"]);
            }
            eventLog.Source = ConfigurationManager.AppSettings["SourceName"];
            eventLog.Log = ConfigurationManager.AppSettings["LogName"];
        }

        protected override void OnStart(string[] args)
        {

            //create logger
            logger = new LoggingModal();
            logger.MessageRecieved += OnMsg;
            LogArchive archive = new LogArchive();
            logger.MessageRecieved += archive.OnMsg;

            // Update the service state to Start Pending.
            logger.Log(new MessageRecievedEventArgs
            {
                Status = MessageTypeEnum.INFO,
                Message = "Starting service..."
            });
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_START_PENDING,
                dwWaitHint = 100000
            };
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //create directory server
            int size = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            server = new ImageServer(logger, ConfigurationManager.AppSettings["OutputDir"], size);

            logger.Log(new MessageRecievedEventArgs
            {
                Status = MessageTypeEnum.INFO,
                Message = "Starting Server."
            });

            //create gui server and listen for connections
            serviceServer = new ServiceServer(server, archive, logger);
            Task t = new Task(() => serviceServer.Listen());
            t.Start();
            logger.Log(new MessageRecievedEventArgs
            {
                Status = MessageTypeEnum.INFO,
                Message = "Server is listening."
            });

            ///start listening to folders
            string[] paths = ConfigurationManager.AppSettings["Handler"].Split(';');
            foreach (string s in paths)
                server.WatchDirectory(s);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            started = true;
            logger.Log(new MessageRecievedEventArgs
            {
                Status = MessageTypeEnum.INFO,
                Message = "Service started."
            });
        }

        protected override void OnStop()
        {
            logger.Log(new MessageRecievedEventArgs
            {
                Status = MessageTypeEnum.INFO,
                Message = "Stopping service.."
            });
            try
            {
                //close directory watchers
                server.SendCommand(new CommandRecievedEventArgs
                {
                    Type = CommandEnum.CloseCommand,
                    Args = new String[] { "All" }

                });

                //close gui server
               // serviceServer.Close();

                //notify about service closure
                logger.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.INFO,
                    Message = "Service Stopped."
                });
            }
            catch (Exception e)
            {
                logger.Log(new MessageRecievedEventArgs
                {
                    Status = MessageTypeEnum.FAIL,
                    Message = e.Message
                });
            }

            // Update the service state to Stopped.  
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_STOPPED,
                dwWaitHint = 100000
            };
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            started = false;
        }
        /// <summary>
        /// used to write messages to the service's event log
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="message">message to log</param>
        public void OnMsg(object sender, MessageRecievedEventArgs message)
        {

            EventLogEntryType type;
            switch (message.Status)
            {
                case MessageTypeEnum.FAIL:
                    type = EventLogEntryType.FailureAudit;
                    break;
                case MessageTypeEnum.INFO:
                    type = EventLogEntryType.Information;
                    break;
                case MessageTypeEnum.WARNING:
                    type = EventLogEntryType.Warning;
                    break;
                default:
                    type = EventLogEntryType.Error;
                    break;
            }
            eventLog.WriteEntry(message.Message, type, eventId++);
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
