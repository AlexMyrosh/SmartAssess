﻿namespace Data_Access_Layer.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
