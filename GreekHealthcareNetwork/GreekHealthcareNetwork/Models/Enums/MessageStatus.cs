using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageStatus
    {
        Read,
        Unread,
        Replied
    }
}