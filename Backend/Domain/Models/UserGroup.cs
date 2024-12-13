namespace Domain.Models
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public bool? IsManager { get; set; }
        public bool? IsAcceptedByManager { get; set; }
        public bool? IsAcceptedByUser { get; set; }
        public string RequestMessage { get; set; }
    }
}