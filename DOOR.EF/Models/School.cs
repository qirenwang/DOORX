using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DOOR.EF.Models
{
    [Table("SCHOOL")]
    public partial class School
    {
        [Key]
        [Column("SCHOOL_ID", TypeName = "NUMBER")]
        public decimal SchoolId { get; set; }
        [Column("SCHOOL_NAME")]
        [StringLength(20)]
        [Unicode(false)]
        public string? SchoolName { get; set; }
    }
}
