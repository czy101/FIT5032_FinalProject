namespace Test3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Feedback
    {
        [Key]
        public int Feedback_id { get; set; }

        public int Appointment_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Feedback_Date { get; set; }

        [Required]
        public string Feedback_Content { get; set; }

        [Required]
        public string Doctor_id { get; set; }

        [Required]
        public string Patient_id { get; set; }

        public string Feedback_File_Path { get; set; }
    }
}
