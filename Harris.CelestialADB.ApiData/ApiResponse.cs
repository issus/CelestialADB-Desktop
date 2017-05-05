using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.ApiData
{
    public class ApiResponse
    {
        public ApiResponse()
        {

        }

        public ApiResponse(bool success, string msg)
        {
            Success = success;
            Message = msg;
        }

        public ApiResponse(bool success)
        {
            Success = success;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
