﻿namespace RpR.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public string Email { get; set; }
    }
}