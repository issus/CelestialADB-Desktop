using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.Desktop.WebService
{
    public class TokenResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
    }

    public class TokenRequest
    {
        public string grant_type { get { return "password"; }  }
        public string username { get; set; }
        public string password { get; set; }
    }
}
