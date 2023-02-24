using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflowAPI_BAL.Models
{
    public class User:IdentityUser
    {
        [Required(ErrorMessage = "First Name is a required field.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is a required field.")]
        public string? LastName { get; set; }
    }
}
