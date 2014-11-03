using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using PushSharp;
using System.Configuration;
using PushSharp.Apple;
using System.IO;

namespace CrowdNotificationService
{
    public partial class Service1 : ServiceBase
    {
        private bool isSandbox = false;
        private Timer runTimer = null;
        private int timerInterval = 0;

        private string p12FileSandbox;
        private string p12FileProduction;
        private string p12FilePassword;

        private PushBroker pushBroker;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string activityName = "OnStart:";
            Trace.TraceInformation("{0}Notification service starting...", activityName);

            this.isSandbox = bool.Parse(ConfigurationManager.AppSettings["IsSandbox"]);
            this.p12FileProduction = ConfigurationManager.AppSettings["p12FileName_Live"];
            this.p12FileSandbox = ConfigurationManager.AppSettings["p12FileName_Local"];
            this.p12FilePassword = ConfigurationManager.AppSettings["p12FilePassword"];
            this.timerInterval = int.Parse(ConfigurationManager.AppSettings["timerInterval"]);


            Trace.TraceInformation("{0}Started with: IsSandbox={1}, p12FileProduction={2}, p12FileSandbox={3}, p12FilePassword={4}, timerInterval={5}", activityName, this.isSandbox, this.p12FileProduction, this.p12FileSandbox, this.p12FilePassword, this.timerInterval);

            this.pushBroker = new PushBroker();
            this.pushBroker.OnChannelCreated += pushBroker_OnChannelCreated;
            this.pushBroker.OnChannelDestroyed += pushBroker_OnChannelDestroyed;
            this.pushBroker.OnChannelException += pushBroker_OnChannelException;
            this.pushBroker.OnNotificationFailed += pushBroker_OnNotificationFailed;
            this.pushBroker.OnNotificationRequeue += pushBroker_OnNotificationRequeue;
            this.pushBroker.OnNotificationSent += pushBroker_OnNotificationSent;
            this.pushBroker.OnServiceException += pushBroker_OnServiceException;

            string filePathToP12 = null;

            if (this.isSandbox)
            {
                //we need sandbox p12 file
                filePathToP12 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12FileSandbox);
            }
            else
            {
                //we need sandbox p12 file
                filePathToP12 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12FileProduction);

            }

            Trace.TraceInformation("{0}Reading p12 certificate frm path {1}", activityName, filePathToP12);
            //lets read the certificate file out
            var appleCert = File.ReadAllBytes(filePathToP12);
            pushBroker.RegisterAppleService(new ApplePushChannelSettings(appleCert, this.p12FilePassword));

            this.runTimer = new Timer();
            this.runTimer.Interval = timerInterval;
            this.runTimer.Elapsed += new ElapsedEventHandler(onRunTimerTick);
            this.runTimer.Enabled = true;

            Trace.TraceInformation("{0}Completed initialization of service", activityName);
        }

        void pushBroker_OnServiceException(object sender, Exception error)
        {

            Trace.TraceError("OnServiceException: {0}", error);
        }

        void pushBroker_OnNotificationSent(object sender, PushSharp.Core.INotification notification)
        {
            Trace.TraceInformation("OnNotificationSent: {0}", notification.Tag);

        }

        void pushBroker_OnNotificationRequeue(object sender, PushSharp.Core.NotificationRequeueEventArgs e)
        {
            Trace.TraceInformation("OnNotificationRequeue: {0} due to {1}", e.Notification.Tag, e.RequeueCause);
        }

        void pushBroker_OnNotificationFailed(object sender, PushSharp.Core.INotification notification, Exception error)
        {
            Trace.TraceError("OnNotificationFailed: {0} due to {1}", notification.Tag, error);
        }

        void pushBroker_OnChannelException(object sender, PushSharp.Core.IPushChannel pushChannel, Exception error)
        {
            Trace.TraceError("OnChannelException: {0}", error);
        }

        void pushBroker_OnChannelDestroyed(object sender)
        {
            Trace.TraceInformation("OnChannelDestroyed: Channel destroyed");
        }

        void pushBroker_OnChannelCreated(object sender, PushSharp.Core.IPushChannel pushChannel)
        {
            Trace.TraceInformation("OnChannelCreated: Channel created");
        }

        protected override void OnStop()
        {
            string activityName = "OnStop:";
            this.runTimer.Enabled = false;

            Trace.TraceInformation("{0}Stopping push broke service...", activityName);
            //we need to drain the queue if any outstanding notifications
            this.pushBroker.StopAllServices();

            Trace.TraceInformation("{0}Shutting down, goodbye", activityName);
            
        }

        private void onRunTimerTick(object sender, ElapsedEventArgs e)
        {
            string activityName = "onRunTimerTick:";

            DateTime start = DateTime.Now;
            DateTime dateTimeNow = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

            using (CrowdEntities context = new CrowdEntities())
            {
                //lets find all notifications that are pending
                var pendingNotifications = context.Notifications.Where(n => n.HasSent == false).ToList();

                if (pendingNotifications.Count() > 0)
                {
                    Trace.TraceInformation("{0}Begin processing of notifications at {0}", activityName, dateTimeNow.ToString());
                    Trace.TraceInformation("{0}Found {1} outstanding notifications to send", activityName, pendingNotifications.Count());

                }



                foreach (Notification notification in pendingNotifications)
                {
                    AppleNotification appleNotification = new AppleNotification();
                    appleNotification.DeviceToken = notification.DeviceToken;
                    appleNotification.Payload.Alert.Body = notification.PushMessage;
                    appleNotification.Payload.Badge = 1;
                    appleNotification.Payload.Sound = "notification.wav";

                    List<object> parameters = new List<object>();
                    parameters.Add(notification.SourceTable);


                    if (!string.IsNullOrEmpty(notification.UserID))
                    {
                        parameters.Add(notification.UserID);
                    }

                    if (!string.IsNullOrEmpty(notification.JobID))
                    {
                        parameters.Add(notification.JobID);
                    }

                    appleNotification.Payload.Alert.LocalizedArgs = parameters;
                    appleNotification.Tag = notification.ID;

                    pushBroker.QueueNotification(appleNotification);
                    Trace.TraceInformation("{0}Enqueued notification {1} sent to deviceToken {2}", activityName, notification.ID, notification.DeviceToken);

                    notification.HasSent = true;
                    notification.DateSent = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

                }

                context.SaveChanges();

            }

            DateTime end = DateTime.Now;
            Trace.TraceInformation("{0}Completed processing all outstanding notifications in {1} miliseconds", activityName, (end - start).TotalMilliseconds);

        }
    }
}
