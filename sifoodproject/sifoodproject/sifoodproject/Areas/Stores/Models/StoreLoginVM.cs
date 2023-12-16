using System.ComponentModel.DataAnnotations;

namespace sifoodproject.Areas.Stores.Models
{
    public class StoreLoginVM
    {
        public string? StoreAccount { get; set; }

        public string? SetPassword { get; set; }
    }
}
