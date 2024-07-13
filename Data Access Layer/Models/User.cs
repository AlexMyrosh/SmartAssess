﻿using Microsoft.AspNetCore.Identity;

namespace Data_Access_Layer.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ClassGroup { get; set; }
    }
}