using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Domain.Entities
{
    public class Chat
    {
        public required int Id { get; set; }
        public required List<int> ParticipantIds { get; set; }
        public required List<Message> Messages { get; set; }
    }

    public class Message
    {
        public required int Id { get; set; }
        public required int ChatId { get; set; }
        public required int SenderId { get; set; }
        public required string Content { get; set; }
        public required DateTime SentAt { get; set; }
    }
}
