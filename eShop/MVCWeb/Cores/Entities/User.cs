using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCWeb.Cores.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}