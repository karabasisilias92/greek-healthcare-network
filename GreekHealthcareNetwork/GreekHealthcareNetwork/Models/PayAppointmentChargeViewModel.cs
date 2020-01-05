using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class PayAppointmentChargeViewModel
    {
        public int AppointmentId { get; set; }
        public decimal AppointmentCharge { get; set; }
    }
}