using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Response
{
    public class TRq<T>
    {
        [JsonProperty(PropertyName = "result")]
        public T Result { get;set; }
    }
}
