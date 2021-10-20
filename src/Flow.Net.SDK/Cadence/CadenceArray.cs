﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flow.Net.Sdk.Cadence
{
    public class CadenceArray : ICadence
    {
        public CadenceArray()
        {
            Value = new List<ICadence>();
        }

        public CadenceArray(IList<ICadence> value)
        {
            Value = value;
        }

        [JsonProperty("type")]
        public string Type => "Array";

        [JsonProperty("value")]
        public IList<ICadence> Value { get; set; }
    }
}
