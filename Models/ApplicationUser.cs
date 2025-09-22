using Microsoft.AspNetCore.Identity;

namespace Maksiupedia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}
