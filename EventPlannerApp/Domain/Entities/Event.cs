using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndTime { get; set; }
        public string? Location { get; set; }
        public List<int>? ParticipantsIds { get; set; }
        public int ParticipantsCount => ParticipantsIds?.Count ?? 0;
        public int? OrganizerId { get; set; }
        public bool IsPublic { get; set; }
        public int? MaxParticipants { get; set; }
        public String? QRCode { get; set; }
        public List<String>? Tags { get; set; }
        public int ImageIndex { get; set; }

        public Dictionary<String, Feedback>? Feedbacks { get; set; }
        public int? ViewCount { get; set; }
        public int Rating => Feedbacks?.Count > 0 ? (int)Math.Round((double)Feedbacks.Sum(f => f.Value.Rating) / Feedbacks.Count) : 0;
    }

    public class Feedback
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
