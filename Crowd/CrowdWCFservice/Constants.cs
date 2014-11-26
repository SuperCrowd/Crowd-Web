using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdWCFservice
{
    public static class Constants
    {
        public const int TWILIO_HEARTBEAT_DURATION = 3600;
        public const int CHECK_TWILIO_STATUS_INTERVAL = 60;
    }
}