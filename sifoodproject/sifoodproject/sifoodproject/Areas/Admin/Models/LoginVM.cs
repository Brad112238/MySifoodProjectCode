using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Admin.Models
{
    public class LoginVM
    {
        public string? Account { get; set; }

        public string? Password { get; set; }
    }
}
