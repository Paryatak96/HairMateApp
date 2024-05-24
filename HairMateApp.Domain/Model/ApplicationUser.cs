using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Inne właściwości użytkownika
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string Region { get; set; }
    public string ProfilePicture { get; set; }
}