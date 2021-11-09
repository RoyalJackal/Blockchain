using System;
using System.Text.Json.Serialization;

namespace Blockchain.Task2
{
    public class TimeStampDto
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("status_string")]
        public string StatusString { get; set; }

        [JsonPropertyName("time_stamp_token")]
        public TSToken TimeStampToken { get; set; }

        public class TSToken
        {
            [JsonPropertyName("ts")]
            public string Ts { get; set; }

            [JsonPropertyName("signature")]
            public string Signature { get; set; }
        }
    }
}
