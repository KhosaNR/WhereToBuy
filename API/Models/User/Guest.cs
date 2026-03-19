namespace API.Models
{
    public class Guest : User
    {
        public DateTime TokenExpiryDate { get; set; }
    }
}
