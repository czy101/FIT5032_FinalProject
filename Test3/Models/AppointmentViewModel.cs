using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test3.Models
{
    public class AppointmentViewModel
    {
        public int Appointment_id { get; set; }
        public string Patient_id { get; set; }
        public string Patient_name { get; set; }
        public string Patient_email { get; set; }
        public string Doctor_id { get; set; }
        public string Doctor_name { get; set; }
        public string Doctor_email { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}