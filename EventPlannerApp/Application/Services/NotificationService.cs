using EventPlannerApp.Application.Interfaces;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Application.Services
{
    public class NotificationService
    {
        private readonly IEventService _eventService;
        public NotificationService(IEventService eventService)
        {
            _eventService = eventService;
            LocalNotificationCenter.Current.NotificationReceived += OnNotificationReceived;
        }
        private void OnNotificationReceived(NotificationEventArgs e)
        {
            Console.WriteLine($"Notification received: {e.ToString()} - {e.ToString()}");
        }
        public void ScheduleNotification(string title, string message, DateTime notifyTime)
        {
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = title,
                Description = message,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                },
                CategoryType = NotificationCategoryType.Reminder,
                Image = new NotificationImage() { FilePath = "logo.png" },
            };
            LocalNotificationCenter.Current.Show(notification);
        }
    }
}
