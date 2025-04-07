using EventPlannerApp.Domain.Entities;
using EventPlannerApp.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.Helpers
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? IncomingMessageTemplate { get; set; }
        public DataTemplate? OutgoingMessageTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Message message)
            {
                return message.SenderId == GetUserId() ? OutgoingMessageTemplate : IncomingMessageTemplate;
            }
            return null;
        }
        // Gets the UserId From the Main Page
        private int GetUserId()
        {
            int userId = MainViewModel.UserId;
            return userId;
        }
    }
}
