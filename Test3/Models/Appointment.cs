//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Test3.Models;

namespace Test3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appointment
    {
        public int Appointment_id { get; set; }
        public string Patient_id { get; set; }
        public string Doctor_id { get; set; }
        public string Patient_email { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }
}