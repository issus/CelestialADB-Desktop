using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    public class AuthToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        [DeserializeAs(Name = ".issued")]
        public string issued { get; set; }
        [DeserializeAs(Name = ".expires")]
        public string expires { get; set; }
    }
}
