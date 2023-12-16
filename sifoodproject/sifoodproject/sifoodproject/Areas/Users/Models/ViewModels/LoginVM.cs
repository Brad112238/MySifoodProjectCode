using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Users.Models.ViewModels
{
    public class LoginVM
    {
        public string? Account { get; set; }

        public string? Password { get; set; }
    }
}
