using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SmartOvenV2.Services
{
    internal class NotificationService : INotificationService
    {
        public NotificationService()
        {

        }

        public static void Init()
        {
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        }

        private static void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
        }

        public void ShowNotification(string title, string body)
        {
            var notificationId = Guid.NewGuid().GetHashCode();

            try
            {
                var notification = new NotificationRequest
                {
                    NotificationId = notificationId,
                    Title = title,
                    Description = body,
                    ReturningData = notificationId.ToString(),
                };
                LocalNotificationCenter.Current.Show(notification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error sending exception " + ex.Message);
                return;
            }
        }
    }
}
