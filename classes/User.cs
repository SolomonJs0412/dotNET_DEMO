using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetEFAndJWT.classes
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public User(string Username, byte[] PasswordHash, byte[] PasswordSalt)
        {
            this.Username = Username;
            this.PasswordHash = PasswordHash;
            this.PasswordSalt = PasswordSalt;
        }
        public User() { }
    }
}