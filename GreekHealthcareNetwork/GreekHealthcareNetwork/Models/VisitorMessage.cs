using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class VisitorMessage : Message
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}