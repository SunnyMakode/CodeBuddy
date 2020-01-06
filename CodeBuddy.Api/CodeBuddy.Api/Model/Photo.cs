using System;

namespace CodeBuddy.Api.Model
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMainPhoto { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}