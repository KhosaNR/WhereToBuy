namespace API.Models
{
    public class Guest: User
    {
        DateTime tokenExpiryDate { get; set; }
    }
}
