namespace SCML.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("tblUploadedResume")]
    public partial class EmailFormModel
    {
        public int id { get; set; }

        [StringLength(200)]
        [Required, Display(Name = "Full name")]
        public string full_name { get; set; }

        [StringLength(200)]
        [Required, Display(Name = "Applied For")]
        public string applied_for { get; set; }

        [StringLength(200)]
        [Required, Display(Name = "Email"), EmailAddress]
        public string email { get; set; }

        [StringLength(200)]
        public string file_path { get; set; }

        [NotMapped]
        public HttpPostedFileBase Upload { get; set; }

        [DefaultValue(true)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? create_date { get; set; }

    
    }
}


