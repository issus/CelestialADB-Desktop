using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{

    public class UserRegistrationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<String> Errors { get; set; }
    }

    public class UserRegistrationRequest
    {
        //username, password, email, firstname, lastname, allowemail, usertype, company
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AllowEmail { get; set; }
        public UserType UserType { get; set; }
        public string Company { get; set; }
    }
}