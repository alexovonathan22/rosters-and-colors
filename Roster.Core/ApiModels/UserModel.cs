using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Roster.Core.ApiModels
{
    public class UserModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public int Id { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class AdminPowers
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public bool PromoteOrDemote { get; set; }
    }
}
