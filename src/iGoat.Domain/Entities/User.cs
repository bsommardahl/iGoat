using System;

namespace iGoat.Domain
{
    public class User
    {
        public UserStatus Status { get; set; }

        public int Id { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}