using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Test3.Models
{
    public partial class FeedbackModel : DbContext
    {
        public FeedbackModel()
            : base("name=FeedbackModel1")
        {
        }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
