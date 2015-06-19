using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PottyTag.Model
{
    class CheckInResponse
    {
        public bool Success;
        public int Id;

        [JsonConstructor]
        public CheckInResponse(bool success, int id)
        {
            Success = success;
            Id = id;
        }
    }
}
