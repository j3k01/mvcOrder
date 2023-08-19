using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Model.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? City { get; set; }
        public int? PostalCode { get; set; }

        public string? Role { get; set; }
    }
}
