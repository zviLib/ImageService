using System.Diagnostics;

namespace ImageService.Modal.Logging
{
    public enum MessageTypeEnum : int
    {
        INFO,
        WARNING,
        FAIL
    }

    static class MessageTypeEnumMethod
    {

        public static EventLogEntryType Translate(this MessageTypeEnum type)
        {
            switch (type)
            {
                case MessageTypeEnum.INFO:
                    return EventLogEntryType.Information;
                case MessageTypeEnum.FAIL:
                    return EventLogEntryType.FailureAudit;
                case MessageTypeEnum.WARNING:
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}
