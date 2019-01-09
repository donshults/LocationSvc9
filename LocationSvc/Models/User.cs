namespace LocationSvc.Models
{
    public class User
    {
        public string Email { get; set; }
        public int LookupId { get; set; }
        public string LookupValue { get; set; }
        public string LoginName { get; set; }
        public bool IsAdmin { get; set; }

    }
}