namespace SCML.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblJobDetails")]
    public partial class JobDetail
    {
        public int id { get; set; }

        [StringLength(100)]
        [Required, Display(Name = "Title")]
        public string title { get; set; }

        [StringLength(1000)]
        [Required, Display(Name = "Job Details")]
        public string details { get; set; }

        [StringLength(200)]
        [Required, Display(Name = "Qualification")]
        public string qualification { get; set; }

        [StringLength(20)]
        [Required, Display(Name = "Experience")]
        public string experience { get; set; }

        [Required, Display(Name = "Deadline")]
        [DefaultValue(true)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime deadline { get; set; }

        [StringLength(10)]
        public string create_by { get; set; }

        public DateTime? create_date { get; set; }
    }
}

